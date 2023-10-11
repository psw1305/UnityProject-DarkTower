using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DT.StatCalc;

public enum TargetType
{
    None,
    Self,
    Player,
    PlayerAll,
    Enemy,
    EnemyAll,
    All,
}

[CreateAssetMenu(menuName = "DT/Player/Skill")]
public class SkillData : ScriptableObject
{
    [BoxGroup("스킬 정보")] public string className, skillName, textName;

    [TitleGroup("설정")]
    [TabGroup("설정/구분", "기본")] public TargetType targetType;
    [TabGroup("설정/구분", "보정")] public DamageType damageType;
    [TabGroup("설정/구분", "기본")] public UnityEvent skillAction;

    [TabGroup("설정/구분", "보정")] public float editHIT = 0;
    [TabGroup("설정/구분", "보정")] public float editCRIT = 0;
    [TabGroup("설정/구분", "보정")] public float editSTR = 100;

    [NonSerialized] public float sHIT;
    [NonSerialized] public float sCRIT;
    [NonSerialized] public float sSTRMin, sSTRMax;
    [NonSerialized] public float sSTG;
    protected UnitSystem user, target;

    [TabGroup("설정/구분", "보정")][Button(ButtonSizes.Large)]
    public void DataBaseToInspector()
    {
        //var entity = BG_Class_Skill.GetEntity(className + "_" + skillName);

        //skill_Str = entity.bg_Skill_STR;
        //skill_Acr = entity.bg_Skill_ACR;
        //skill_Crt = entity.bg_Skill_CRT;
    }

    protected void EditStatValue()
    {
        sHIT = user.HIT.Value + editHIT;
        sCRIT = user.CRIT.Value + editCRIT;
        sSTRMin = Mathf.RoundToInt(user.STRMin.Value * (editSTR / 100f));
        sSTRMax = Mathf.CeilToInt(user.STRMax.Value * (editSTR / 100f));
    }

    public virtual void SetUser(UnitSystem user) 
    {
        this.user = user;
        BattleCamera.Instance.actionUnit = user;
    }

    void UseSkill(UnitSystem target)
    {
        this.target = target;
        BattleCamera.Instance.AddUnit(this.target); // 카메라 연출용 유닛 리스트 추가
        EditStatValue();
        skillAction.Invoke();
    }

    public void UseSkillForSingle(UnitSystem target) 
    {
        UseSkill(target);
        GameSystem.Instance.BattleTurn(TurnState.PLAYING);
    }

    public void UseSkillForPlayers(List<UnitPlayer> targets)
    {
        foreach (UnitPlayer target in targets)
        {
            UseSkill(target);
        }

        GameSystem.Instance.BattleTurn(TurnState.PLAYING);
    }

    public void UseSkillForEnemys(List<UnitEnemy> targets)
    {
        foreach (UnitEnemy target in targets)
        {
            UseSkill(target);
        }

        GameSystem.Instance.BattleTurn(TurnState.PLAYING);
    }

    public void SkillMotion()
    {
        user.Motion("Skill_" + skillName);
    }

    public void Damage()
    {
        if (StatCalculator.EvadeCheck(sHIT, target.AGI.Value))
        {
            target.UnitDamage(damageType, sSTRMin, sSTRMax, sCRIT);
        }
        else
        {
            target.Motion("Damaged");
            target.DamageText(DamageType.MISS, "빗나감!");
        }
    }

    public void DamageWithBuff(GameObject buff)
    {
        if (StatCalculator.EvadeCheck(sHIT, target.AGI.Value))
        {
            target.UnitDamage(damageType, sSTRMin, sSTRMax, sCRIT);
            target.AddBuff(buff);
        }
        else
        {
            target.Motion("Damaged");
            target.DamageText(DamageType.MISS, "빗나감!");
        }
    }

    public void Heal()
    {
        target.UnitHeal(sSTRMin, sSTRMax, sCRIT);
    }

    public void Shield()
    {
        target.UnitHeal(sSTRMin, sSTRMax, sCRIT);
    }

    public void Buff(GameObject buff)
    {
        target.AddBuff(buff);
    }

    // 버프 해제
    public void Dispel()
    {
    }
}