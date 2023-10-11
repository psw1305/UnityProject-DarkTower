using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TurnState
{
    DISABLE,
    READY,
    START,
    PLAYER_TURN,
    ENEMY_TURN,
    PLAYING,
    VICTORY,
    DEFEAT,
}


public enum TurnBuffType
{
    Disable,
    Early,
    Delay,
    Chance,
}

public class TurnSystem : MonoBehaviour
{
    private static TurnSystem instance;
    public static TurnSystem Instance
    {
        get { return instance; }
    }

    public List<TurnUnit> turnList = new List<TurnUnit>();
    public GridLayoutGroup turnTable;
    public Transform quickFixTable;
    public GameSystem battleSystem;
    public TurnState turnState;

    [HideInInspector] public TurnUnit currentTurn;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        turnState = global::TurnState.DISABLE;
    }

    public void TurnStart()
    {
        StartCoroutine(nameof(TurnCheck));
    }

    /// <summary>
	/// 전투 시작시 턴 생성
	/// </summary>
    public void Make()
    {
        foreach (UnitSystem unit in battleSystem.unitArray)
        {
            for (int i = 0; i < unit.turnCnt; i++)
            {
                if (unit.unitState != UnitState.DEAD)
                {
                    GameObject turnObject = Instantiate(unit.turnFlagPrefab, quickFixTable);
                    TurnUnit turnFlag = turnObject.GetComponent<TurnUnit>();
                    turnFlag.Init(unit);
                    turnList.Add(turnFlag);
                }
            }
        }

        Match();
    }

    /// <summary>
	/// 생성된 TurnUnit을 행동력에 맞게 turnTable 세팅
	/// </summary>
    public void Match()
    {
        turnList.Sort(SpeedCompare);

        foreach(TurnUnit turn in turnList)
        {
            turn.transform.SetParent(turnTable.transform);
        }

        battleSystem.BattleTurn(global::TurnState.START);
    }

    /// <summary>
	/// 두개의 TurnUnit 속도를 비교하여 오름차순으로 정렬
	/// </summary>
    int SpeedCompare(TurnUnit A, TurnUnit B)
    {
        if (A.spdLevel < B.spdLevel)
        {
            return 1;
        }
        else if (A.spdLevel > B.spdLevel)
        {
            return -1;
        }
        else if (A.spdLevel == B.spdLevel)
        {
            if (A.detailSpd < B.detailSpd)
            {
                return 1;
            }
            else if (A.detailSpd > B.detailSpd)
            {
                return -1;
            }
        }

        return 0;
    }

    public void TurnDestroy(TurnUnit turn = null)
    {
        if (currentTurn == null) return;
        if (turn == null) turn = currentTurn;

        turnList.Remove(turn);
        Destroy(turn.gameObject);
    }

    /// <summary>
	/// 턴 진행 확인
	/// </summary>
    IEnumerator TurnCheck()
    {
        yield return new WaitForSeconds(0.3f);

        // 승리나 패배시 턴 종료
        if (turnState == global::TurnState.VICTORY || turnState == global::TurnState.DEFEAT)
        {
            TurnClear();

            if (turnState == global::TurnState.VICTORY)
            {
                battleSystem.BattleVictory();
            }
            else if (turnState == global::TurnState.DEFEAT)
            {
                battleSystem.BattleDefeat();
            }
        }
        // TurnUnit 존재시 해당 유닛 진행
        else if (turnList.Count != 0)
        {
            currentTurn = turnList[0];
            currentTurn.Active();
            battleSystem.UnitTurnPlay(currentTurn.unit);
        }
        // 라운드 지속
        else
        {
            battleSystem.BattleUpdate();
        }
    }

    void TurnClear()
    {
        foreach (TurnUnit turn in turnList)
        {
            Destroy(turn.gameObject);
        }

        turnList.Clear();
    }


    ///-----------------------------------------------------------------------------------------------///
	///---------------------------------------- Turn Move --------------------------------------------///
	///-----------------------------------------------------------------------------------------------///
    
    public void TurnState(TurnBuffType type, UnitSystem unit)
    {
        switch (type)
        {
            case TurnBuffType.Disable:
                break;

            case TurnBuffType.Early:
                for (int i = 0; i < unit.turnUnits.Count; i++)
                {
                    Early(unit.turnUnits[i]);
                }
                break;

            case TurnBuffType.Delay:
                for (int i = unit.turnUnits.Count - 1; i >= 0; i--)
                {
                    Delay(unit.turnUnits[i]);
                }
                break;

            case TurnBuffType.Chance:
                Chance(unit);
                break;
        }
    }

    int Find(TurnUnit turn)
    {
        for (int i = 0; i < turnList.Count; i++)
        {
            if (turnList[i] == turn)
            {
                return i;
            }
        }

        return 0;
    }

    // 턴 앞으로 당기기
    void Early(TurnUnit turn)
    {
        int num = Find(turn);

        if (num > 0 && currentTurn != turnList[num - 1])
        {
            TurnUnit temp = turnList[num - 1];
            turnList[num - 1] = turnList[num];
            turnList[num] = temp;

            temp.transform.SetSiblingIndex(num);
        }
    }

    // 턴 뒤로 미루기
    void Delay(TurnUnit turn)
    {
        int num = Find(turn);

        if (num < turnList.Count - 1 && num != 0)
        {
            TurnUnit temp = turnList[num + 1];
            turnList[num + 1] = turnList[num];
            turnList[num] = temp;

            temp.transform.SetSiblingIndex(num);
        }
    }

    // 한 턴 생성
    void Chance(UnitSystem unit)
    {
        GameObject turnObject = Instantiate(unit.turnFlagPrefab, turnTable.transform);
        TurnUnit turnFlag = turnObject.GetComponent<TurnUnit>();
        turnFlag.Init(unit);
        turnList.Add(turnFlag);
    }
}
