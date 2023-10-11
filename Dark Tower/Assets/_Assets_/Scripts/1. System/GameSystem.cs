using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public enum GameState
{
	READY,
	PAUSE,
	EXPLORE,
	CHECKING,
	INTERACT,
	BATTLE,
}

public class GameSystem : MonoBehaviour
{
	private static GameSystem instance;
	public static GameSystem Instance
	{
		get { return instance; }
	}

	[TabGroup("시스템", "준비")] public List<GameObject> playerPrefabs;
	[TabGroup("시스템", "준비")] public Transform playerGround, enemyGround;

	[TabGroup("시스템", "진행중")] public ExplorePlayer explorePlayer;
	[TabGroup("시스템", "진행중")] public ExploreEnemy exploreEnemy;
	[TabGroup("시스템", "진행중")] public int playerUnitCount = 0;
	[TabGroup("시스템", "진행중")] public int enemyUnitCount = 0;

	[NonSerialized] public List<UnitSystem> unitArray = new List<UnitSystem>();
	[NonSerialized] public List<UnitPlayer> playerUnits = new List<UnitPlayer>();
	[NonSerialized] public List<UnitEnemy> enemyUnits = new List<UnitEnemy>();
	[NonSerialized] public UnitSystem selectedUnit;

	private GameState gameState;
    private ExploreSystem exploreSystem;
    private TurnSystem turnSystem;
    private SkillSystem skillSystem;
	private BattleCamera battleCamera;

	public GameState State
    {
        get
        {
			return gameState;
		}
        set
        {
			gameState = value;

			// 일시정지(ESC) 우선, 그다음 BATTLE 2순위
			// 나머지는 EXPLORE 상태 아니면 timeScale = 0 으로
			if (gameState == GameState.PAUSE)
			{
				Time.timeScale = 0;
				return;
			}
			else if (gameState == GameState.BATTLE)
			{
				Time.timeScale = 1;
				return;
			}
			else if (gameState != GameState.EXPLORE)
			{
				Time.timeScale = 0;
			}
			else
			{
				Time.timeScale = 1;
			}
		}
	}

	void Awake() 
	{
		instance = this;
		State = GameState.READY;
	}

	void Start()
    {
		exploreSystem = ExploreSystem.Instance;
		turnSystem = TurnSystem.Instance;
		skillSystem = SkillSystem.Instance;
		battleCamera = BattleCamera.Instance;

		float posX = 3f;

		foreach (GameObject playerPrefab in playerPrefabs)
		{
			GameObject playerGO = Instantiate(playerPrefab, playerGround);
			playerGO.transform.localPosition = new Vector3(posX, 0, 0);
			posX -= 2f;

			UnitPlayer playerUnit = playerGO.GetComponent<UnitPlayer>();
			playerUnit.Setting();

			unitArray.Add(playerUnit);
			playerUnits.Add(playerUnit);

			playerUnitCount += 1;

			// 탐험중 캐릭터 확인이 필요한 UI 생성
			UI_Character characterInfo = UI_Character.Instance;
            UI_CharacterButton characterButton = Instantiate(characterInfo.characterButtonTemplate, characterInfo.characterButtonContainer).GetComponent<UI_CharacterButton>();
            characterButton.Init(playerUnit);
            characterInfo.characterBtns.Add(characterButton);
        }
	}

	public void BattleStart()
    {	
		BattleTurn(TurnState.READY);
	}

	public void BattleTurn(TurnState state)
	{
		turnSystem.turnState = state;

		switch (state)
		{
			case TurnState.READY:
				BattleReady();
				turnSystem.Make();
				break;
			case TurnState.START:
				turnSystem.TurnStart();
				break;
			case TurnState.PLAYING:
                battleCamera.Action();
				break;
			case TurnState.VICTORY:
				break;
			case TurnState.DEFEAT:
				break;
		}
	}

	void BattleReady()
    {
		foreach (UnitSystem unit in unitArray)
		{
			unit.UnitUISetting();
        }
	}

	/// <summary>
	/// 한 라운드 끝날시 모든유닛 턴 시작 [0. 해당 TurnUI 시작]
	/// 라운드 업데이트 동안 유닛 상태이상 관리
	/// </summary>
	public void BattleUpdate()
	{
		foreach (UnitPlayer playerUnit in playerUnits)
		{
			playerUnit.SPD = playerUnit.BaseSPD;
			playerUnit.turnUnits.Clear();
		}

		foreach (UnitEnemy enemyUnit in enemyUnits)
		{
			enemyUnit.SPD = enemyUnit.BaseSPD;
			enemyUnit.turnUnits.Clear();
		}

		turnSystem.Make();
	}

	public void BattleResult()
	{
		if (playerUnitCount == 0)
		{
			BattleTurn(TurnState.DEFEAT);
		}
		else if (enemyUnitCount == 0)
		{
			BattleTurn(TurnState.VICTORY);
		}
	}

	public void BattleVictory()
    {
		foreach (UnitPlayer playerUnit in playerUnits)
		{
			playerUnit.SPD = playerUnit.BaseSPD;
			playerUnit.AllRemoveBuff();
			playerUnit.turnUnits.Clear();
		}

		exploreEnemy.Defeat();
		exploreSystem.BattleToExplore();
	}

	public void BattleDefeat()
	{
		Debug.Log("defeat");
	}

	///-----------------------------------------------------------------------------------------------///
	///---------------------------------------- Unit Turn --------------------------------------------///
	///-----------------------------------------------------------------------------------------------///

	public void UnitTurnPlay(UnitSystem turnUnit)
	{
		if (turnUnit.unitType == UnitType.Player)
        {
            PlayerTurn((UnitPlayer)turnUnit);
		}
		else if (turnUnit.unitType == UnitType.Enemy)
		{
            EnemyTurn((UnitEnemy)turnUnit);
		}
	}

	void PlayerTurn(UnitPlayer turnPlayer)
	{
        skillSystem.UnitSkillChange(turnPlayer); 
		selectedUnit = turnPlayer;
        selectedUnit.CheckBuff();
	}

    void EnemyTurn(UnitEnemy turnEnemy)
	{
		turnSystem.turnState = TurnState.ENEMY_TURN;
        selectedUnit = turnEnemy;
        selectedUnit.CheckBuff();
	}

	///-----------------------------------------------------------------------------------------------///
	///--------------------------------------- Dead Check --------------------------------------------///
	///-----------------------------------------------------------------------------------------------///

	public void PlayerDeadCheck(UnitPlayer deadPlayer)
	{
		for (int i = turnSystem.turnList.Count - 1; i >= 0; i--)
		{
			TurnUnit playerTurn = turnSystem.turnList[i];

			if (playerTurn.unit == deadPlayer)
			{
				turnSystem.TurnDestroy(playerTurn);
			}
		}

		for (int i = unitArray.Count - 1; i >= 0; i--)
        {
            if (unitArray[i] == deadPlayer)
            {
                unitArray.Remove(unitArray[i]);
            }
        }

		playerUnits.Remove(deadPlayer);
	}

	public void EnemyDeadCheck(UnitEnemy deadEnemy)
	{
		for (int i = turnSystem.turnList.Count - 1; i >= 0; i--)
		{
			TurnUnit enemyTurn = turnSystem.turnList[i];

			if (enemyTurn.unit == deadEnemy)
			{
				turnSystem.TurnDestroy(enemyTurn);
			}
		}

		for (int i = unitArray.Count - 1; i >= 0; i--)
        {
            if (unitArray[i] == deadEnemy)
            {
                unitArray.Remove(unitArray[i]);
            }
        }

        enemyUnits.Remove(deadEnemy);
	}
}
