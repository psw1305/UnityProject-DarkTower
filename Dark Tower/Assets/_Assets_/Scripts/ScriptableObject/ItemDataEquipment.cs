using UnityEngine;
using DT.Stat;
using DT.StatUnit;

public enum ItemPrefix
{
    NORMAL = 0, // ∆Ú±’ Ω∫≈»
    BROKEN = 1,
    DAMAGED = 2,
    WEAKEN = 3,
    GUARDING = 4,
    ARMORED = 5,
    PIERCING = 6,
    LUCKY = 7,
    SPIKE = 8,
    MENACING = 9,
    WILD = 10,
    VIOLENT = 11,
}

[CreateAssetMenu(menuName = "DT/Item/Equipment")]
public class ItemDataEquipment : ItemData
{
    public ItemPrefix itemPrefix;
    [SerializeField] StatStruct[] statStruct;

    public int GetLength()
    {
        return statStruct.Length;
    }

    public StatName GetStatName(int num)
    {
        return statStruct[num].statName;
    }

    public float GetStatValue(int num)
    {
        return statStruct[num].statValue;
    }

    public string GetStatString(int num)
    {
        string statNameText = "";

        switch (statStruct[num].statName)
        {
            case StatName.STR:
                statNameText = "STR";
                break;
            case StatName.HIT:
                statNameText = "HIT";
                break;
            case StatName.CRIT:
                statNameText = "CRIT";
                break;
            case StatName.DEF:
                statNameText = "DEF";
                break;
            case StatName.AGI:
                statNameText = "AGI";
                break;
            case StatName.SPD:
                statNameText = "SPD";
                break;
        }

        return statNameText + " " + statStruct[num].statValue.ToString();
    }
}
