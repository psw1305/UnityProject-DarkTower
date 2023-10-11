using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterEquipmentSlot : MonoBehaviour
{
    public int index;
    public Image skillImage;
    public RectTransform equipmentSlot;

    private RectTransform itemContainer;
    private UI_ItemEquipment _equipment;
    private Toggle _equipmentBtn;
    private bool isEquiped = false;

    void Start()
    {
        _equipmentBtn = GetComponent<Toggle>();
        _equipmentBtn.onValueChanged.AddListener(EquipmentSlotChanged);

        itemContainer = UI_Inventory.Instance.itemContainer.GetComponent<RectTransform>();
    }

    public UI_ItemEquipment GetEquipment()
    {
        return _equipment;
    }

    public bool GetEquiped()
    {
        return isEquiped;
    }

    public void EquipmentSlotChanged(bool isOn)
    {
        if (isOn) UI_Character.Instance.SetEquipmentSlot(this);
        else UI_Character.Instance.SetEquipmentSlot(null);
    }

    public void EquipmentToHide(RectTransform hideInventoryTrnf)
    {
        if (_equipment == null) return;

        _equipment.transform.SetParent(hideInventoryTrnf);
        _equipment = null;
        isEquiped = false;
    }

    public void ChangeSkill(UnitPlayer unitPlayer, int num)
    {
        skillImage.sprite = Resources.Load<Sprite>("Images/Skill/" + unitPlayer.className + "/Skill_" + unitPlayer.className + "_" + unitPlayer.skillData[num].skillName);
    }

    public void CharacterEquip(UI_ItemEquipment uiItem)
    {
        if (isEquiped) return;

        isEquiped = true;
        _equipment = uiItem;
        uiItem.SetEquip(equipmentSlot);
    }

    public void CharacterUnequip(UI_ItemEquipment uiItem)
    {
        if (!isEquiped) return;

        isEquiped = false;
        _equipment = null;
        uiItem.SetUnequip(itemContainer);
    }
}
