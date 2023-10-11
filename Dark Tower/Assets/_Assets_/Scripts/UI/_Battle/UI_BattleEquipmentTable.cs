using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BattleEquipmentTable : MonoBehaviour
{
    public static UI_BattleEquipmentTable Instance { get; private set; }
    public UI_BattleEquipmentSlot[] equipmentSlots;

    void Awake() { Instance = this; }

    public void UIBattleEquipmentTableChange(UnitPlayer player)
    {
        // => 일단 첫번째 필살기 스킬은 제외, i = 1
        for (int i = 1; i <= equipmentSlots.Length; i++)
        {
            if (player.equipmentData[i] != null)
            {
                equipmentSlots[i - 1].SetSlot(player, i);
            }
            else
            {
                equipmentSlots[i - 1].ResetSlot();
            }
        }
    }
}
