using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillSystem : MonoBehaviour
{
    private static SkillSystem instance;
    public static SkillSystem Instance
    {
        get { return instance; }
    }

    [HideInInspector] public bool isToggle;
    [HideInInspector] public GameObject selectObject;
    [HideInInspector] public UI_BattleSkillButton selectSkillBtn;

    [BoxGroup("��ų")] public UI_BattleSkillButton[] skillBtn;
    private GameSystem gameSystem;
    private TurnSystem turnSystem;
    private UnitPlayer selectPlayer;
    private Dictionary<KeyCode, Action> skillKey;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameSystem = GameSystem.Instance;
        turnSystem = TurnSystem.Instance;

        skillKey = new Dictionary<KeyCode, Action>
        {
            {KeyCode.Z, skillBtn[0].SkillToggleKey },
            {KeyCode.Q, skillBtn[1].SkillToggleKey },
            {KeyCode.W, skillBtn[2].SkillToggleKey },
            {KeyCode.E, skillBtn[3].SkillToggleKey },
            {KeyCode.R, skillBtn[4].SkillToggleKey }
        };
    }

    void Update()
    {
        if (turnSystem.turnState == TurnState.PLAYER_TURN)
        {
            if (Input.anyKeyDown)
            {
                foreach (var dic in skillKey)
                {
                    if (Input.GetKeyDown(dic.Key)) dic.Value();
                }
            }

            // ��ų ��ư Ȱ��ȭ �ǰ� ��Ȳ�� �´� ������Ʈ Ŭ���� ��ų ����
            if (isToggle && selectSkillBtn != null)
            {
                if (Input.GetMouseButtonDown(0)) PlayerUseSkill();
            }
        }
    }

    void PlayerUseSkill()
    {
        Vector2 worldPoint = BattleCamera.Instance.ScreenToWorldPoint();
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (selectPlayer != null && hit.collider != null)
        {
            switch (selectSkillBtn.Data.targetType)
            {
                // �ڽſ��� ���� ���
                case TargetType.Self:
                    if (hit.collider.GetComponentInParent<UnitSystem>() == selectPlayer && hit.collider.CompareTag("battle_player"))
                    {
                        if (selectPlayer.isRestrict)
                        {
                            selectPlayer.NoticeSkillDisable();
                        }
                        else
                        {
                            isToggle = false;
                            selectSkillBtn.Data.SkillMotion();
                            selectSkillBtn.Data.UseSkillForSingle(selectPlayer);
                            selectSkillBtn.SkillToggleKey();
                        }
                    }
                    break;

                // �Ʊ����� ���� ���
                case TargetType.Player:
                    if (hit.collider.CompareTag("battle_player"))
                    {
                        if (selectPlayer.isRestrict)
                        {
                            selectPlayer.NoticeSkillDisable();
                        }
                        else
                        {
                            isToggle = false;
                            selectSkillBtn.Data.SkillMotion();
                            selectSkillBtn.Data.UseSkillForSingle(hit.collider.GetComponentInParent<UnitSystem>());
                            selectSkillBtn.SkillToggleKey();
                        }
                    }
                    break;

                // �Ʊ� ��ü���� ���� ���
                case TargetType.PlayerAll:
                    if (hit.collider.CompareTag("battle_player"))
                    {
                        if (selectPlayer.isRestrict)
                        {
                            selectPlayer.NoticeSkillDisable();
                        }
                        else
                        {
                            isToggle = false;
                            selectSkillBtn.Data.SkillMotion();
                            selectSkillBtn.Data.UseSkillForPlayers(gameSystem.playerUnits);
                            selectSkillBtn.SkillToggleKey();
                        }
                    }
                    break;

                // ������ ���� ���
                case TargetType.Enemy:
                    if (hit.collider.CompareTag("battle_enemy"))
                    {
                        if (selectPlayer.isRestrict)
                        {
                            selectPlayer.NoticeSkillDisable();
                        }
                        else
                        {
                            isToggle = false;
                            selectSkillBtn.Data.SkillMotion();
                            selectSkillBtn.Data.UseSkillForSingle(hit.collider.GetComponentInParent<UnitSystem>());
                            selectSkillBtn.SkillToggleKey();
                        };
                    }
                    break;

                // �� ��ü���� ���� ���
                case TargetType.EnemyAll:
                    if (hit.collider.CompareTag("battle_enemy"))
                    {
                        if (selectPlayer.isRestrict)
                        {
                            selectPlayer.NoticeSkillDisable();
                        }
                        else
                        {
                            isToggle = false;
                            selectSkillBtn.Data.SkillMotion();
                            selectSkillBtn.Data.UseSkillForEnemys(gameSystem.enemyUnits);
                            selectSkillBtn.SkillToggleKey();
                        }
                    }
                    break;
            }
        }
    }

    /// <summary>
	/// �ش� �÷��̾� Ŭ������ ���� ���� �̹����� ��ų ��ȯ
	/// </summary>
    public void UnitSkillChange(UnitPlayer playerUnit)
    {
        UI_BattleStatTable.Instance.SetUIText(playerUnit);

        for (int i = 0; i < skillBtn.Length; i++)
        {
            SkillData playerSkillData = playerUnit.skillData[i];
            skillBtn[i].Data = playerSkillData;
            skillBtn[i].Data.SetUser(playerUnit);
            skillBtn[i].icon.sprite = Resources.Load<Sprite>("Images/Skill/" + playerUnit.className + "/Skill_" + playerUnit.className + "_" + playerSkillData.skillName);
            skillBtn[i].SetUISkill();
        }

        UI_BattleEquipmentTable.Instance.UIBattleEquipmentTableChange(playerUnit);

        selectPlayer = playerUnit;
    }
}
