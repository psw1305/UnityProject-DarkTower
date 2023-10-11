using DG.Tweening;
using Newtonsoft.Json.Bson;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class UnitEnemy : UnitSystem
{
    [TitleGroup("몬스터")]
    [TabGroup("몬스터/구분", "기본")] public SkillDataEnemy[] skillData;
    [TabGroup("몬스터/구분", "기본")] public string idleState = "Normal";

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
	/// 몬스터 스킬 사용 후 플레이어 체력 확인
    /// 일단 랜덤으로 스킬 발동 (추후 수정)
	/// </summary>
    void EnemyUseSkill()
    {
        int ran = Random.Range(0, skillData.Length);
        skillData[ran].SetUser(this);
        skillData[ran].EnemyUseSkill();
    }
}
