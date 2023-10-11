using DG.Tweening;
using Newtonsoft.Json.Bson;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class UnitEnemy : UnitSystem
{
    [TitleGroup("����")]
    [TabGroup("����/����", "�⺻")] public SkillDataEnemy[] skillData;
    [TabGroup("����/����", "�⺻")] public string idleState = "Normal";

    protected override void Init()
    {
        var entity = BG_Monster_Stat.GetEntity(className + "_" + level);
        HP = entity.bg_HLT;
        SPD = entity.bg_ACT;

        STRMax.BaseValue = entity.bg_STR;
        STRMin.BaseValue = Mathf.CeilToInt(STRMax.BaseValue * 0.5f);
        DEF.BaseValue = entity.bg_DEF;
        HIT.BaseValue = entity.bg_ACR;
        AGI.BaseValue = entity.bg_AGI;
        CRIT.BaseValue = entity.bg_CRT;
    }

    public override void MyTurnStart()
    {
        unitUI.turnLight.DOFade(1, 0.4f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            StartCoroutine(nameof(EnemyTurnStart));
        });
    }

    IEnumerator EnemyTurnStart()
    {
        isTurn = true;
        yield return new WaitForSeconds(0.4f);

        switch (unitState)
        {
            case UnitState.NORMAL:
                TurnSystem.Instance.turnState = TurnState.ENEMY_TURN;
                EnemyUseSkill();
                break;
            case UnitState.STUN:
                MyTurnEnd();
                break;
        }
    }

    /// <summary>
	/// ���� ��ų ��� �� �÷��̾� ü�� Ȯ��
    /// �ϴ� �������� ��ų �ߵ� (���� ����)
	/// </summary>
    void EnemyUseSkill()
    {
        int ran = Random.Range(0, skillData.Length);
        skillData[ran].SetUser(this);
        skillData[ran].EnemyUseSkill();
    }
}
