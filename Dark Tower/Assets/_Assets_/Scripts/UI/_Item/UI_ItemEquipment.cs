using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DT.StatUnit;

public class UI_ItemEquipment : UI_Item
{
    public Image itemImage;

    bool isEquip = false;
    [SerializeField] StatStruct[] statStruct;

    public override void SetItem(ItemData item)
    {
        base.SetItem(item);

        ItemDataEquipment equipment = (ItemDataEquipment)item;

        int length = equipment.GetLength();
        statStruct = new StatStruct[length];

        for (int i = 0; i < length; i++)
        {
            statStruct[i].statName = equipment.GetStatName(i);
            statStruct[i].statValue = equipment.GetStatValue(i);
        }
    }

    public void SetEquip(RectTransform equipmentTrnf)
    {
        isEquip = true;
        transform.SetParent(equipmentTrnf);
        transform.localPosition = new Vector3(0, 0, 0);
        GetComponent<Image>().enabled = false;
        ExplorePlayer.Instance.RemoveItemToInventory(GetItem(), false);
    }

    public void SetUnequip(RectTransform inventoryTrnf)
    {
        isEquip = false;
        transform.SetParent(inventoryTrnf);
        GetComponent<Image>().enabled = true;
        ExplorePlayer.Instance.AddItemToInventory(GetItem(), 1, false);
    }

    public void AddStat(UnitSystem unitSystem)
    {
        for (int i = 0; i < statStruct.Length; i++)
        {
            UI_BattleStatTable.Instance.StatAdd(unitSystem, statStruct[i].statName, statStruct[i].statValue, this);
        }
    }

    public void RemoveStat(UnitSystem unitSystem)
    {
        for (int i = 0; i < statStruct.Length; i++)
        {
            UI_BattleStatTable.Instance.StatRemove(unitSystem, statStruct[i].statName, this);
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!isEquip) UI_Character.Instance.ItemEquip(this);
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isEquip) UI_Character.Instance.ItemUnequip(this);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        UI_TooltipItem.Show(transform.position, GetItem());
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        UI_TooltipItem.Hide();
    }
}
