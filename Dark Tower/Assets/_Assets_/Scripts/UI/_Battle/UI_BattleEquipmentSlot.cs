using UnityEngine;
using UnityEngine.UI;

public class UI_BattleEquipmentSlot : MonoBehaviour
{
    public GameObject equipmentSlot;
    public Image equipmentImage;
    public Image unequipmentImage;

    public void SetSlot(UnitPlayer unitPlayer, int num)
    {
        equipmentImage.sprite = unitPlayer.GetEquipmentSprite(num);
        equipmentSlot.SetActive(true);
    }

    public void ResetSlot()
    {
        equipmentImage.sprite = unequipmentImage.sprite;
        equipmentSlot.SetActive(false);
    }
}
