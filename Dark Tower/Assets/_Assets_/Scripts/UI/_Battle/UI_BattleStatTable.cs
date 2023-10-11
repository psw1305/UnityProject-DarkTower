using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using DT.Stat;
using DT.StatUnit;

public class UI_BattleStatTable : MonoBehaviour
{
    private static UI_BattleStatTable instance;
    public static UI_BattleStatTable Instance
    {
        get { return instance; }
    }

    public TextMeshProUGUI textHP, textSP;
    public TextMeshProUGUI textHIT, textAGI, textCRIT, textSTR, textDEF, textSPD;

    void Awake() 
    { 
        instance = this; 
    }

    // 메인 : HP,SP
    // 서브 : HIT, AGI, CRIT, STR, DEF, SPD
    public void SetUIText(UnitSystem unit)
    {
        SetTextFloat(textHP, unit.HP, unit.BaseHP);
        SetTextStat(textHIT, unit.HIT);
        SetTextStat(textAGI, unit.AGI, "%");
        SetTextStat(textCRIT, unit.CRIT, "%");
        SetTextSTR(textSTR, unit.STRMin, unit.STRMax);
        SetTextStat(textDEF, unit.DEF);
        SetTextFloat(textSPD, unit.SPD, unit.BaseSPD);
    }

    public void StatAdd(UnitSystem unit, StatName statName, float modifyFlat, object source)
    {
        switch (statName)
        {
            case StatName.HIT:
                unit.HIT.AddModifier(new StatModifier(modifyFlat, StatModType.Flat, source));
                SetTextStat(textHIT, unit.HIT);
                break;
            case StatName.AGI:
                unit.AGI.AddModifier(new StatModifier(modifyFlat, StatModType.Flat, source));
                SetTextStat(textAGI, unit.AGI, "%");
                break;
            case StatName.CRIT:
                unit.CRIT.AddModifier(new StatModifier(modifyFlat, StatModType.Flat, source));
                SetTextStat(textCRIT, unit.CRIT, "%");
                break;
            case StatName.STR:
                unit.STRMin.AddModifier(new StatModifier(modifyFlat, StatModType.Flat, source));
                unit.STRMax.AddModifier(new StatModifier(modifyFlat, StatModType.Flat, source));
                SetTextSTR(textSTR, unit.STRMin, unit.STRMax);
                break;
            case StatName.DEF:
                unit.DEF.AddModifier(new StatModifier(modifyFlat, StatModType.Flat, source));
                SetTextStat(textDEF, unit.DEF);
                break;
        }
    }

    public void StatRemove(UnitSystem unit, StatName statName, object source)
    {
        switch (statName)
        {
            case StatName.HIT:
                unit.HIT.RemoveAllModifiersFromSource(source);
                SetTextStat(textHIT, unit.HIT);
                break;
            case StatName.AGI:
                unit.AGI.RemoveAllModifiersFromSource(source);
                SetTextStat(textAGI, unit.AGI, "%");
                break;
            case StatName.CRIT:
                unit.CRIT.RemoveAllModifiersFromSource(source);
                SetTextStat(textCRIT, unit.CRIT, "%");
                break;
            case StatName.STR:
                unit.STRMin.RemoveAllModifiersFromSource(source);
                unit.STRMax.RemoveAllModifiersFromSource(source);
                SetTextSTR(textSTR, unit.STRMin, unit.STRMax);
                break;
            case StatName.DEF:
                unit.DEF.RemoveAllModifiersFromSource(source);
                SetTextStat(textDEF, unit.DEF);
                break;
        }
    }

    public void SetTextStat(TextMeshProUGUI statUIText, StatUnit stat, string sign = "")
    {
        UITextColorState(statUIText, stat);
        statUIText.text = stat.Value + sign;
    }

    public void SetTextFloat(TextMeshProUGUI statUIText, float value, float baseValue)
    {
        statUIText.text = value + " / " + baseValue;
    }

    public void SetTextSTR(TextMeshProUGUI statUIText, StatUnit statMin, StatUnit statMax)
    {
        UITextColorState(statUIText, statMin);
        statUIText.text = statMin.Value + "-" + statMax.Value;
    }

    void UITextColorState(TextMeshProUGUI _statUIText, StatUnit stat)
    {
        if (stat.Value > stat.BaseValue) _statUIText.color = Color.green;
        else if (stat.Value < stat.BaseValue) _statUIText.color = Color.red;
        else _statUIText.color = Color.white;
    }
}
