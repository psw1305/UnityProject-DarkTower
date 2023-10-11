using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Character : MonoBehaviour
{
    public static UI_Character Instance;
    public GameObject characterInfoPanel;
    public GameObject characterButtonTemplate;
    public Transform characterButtonContainer;
    public GameObject inventoryPanel;
    bool activePanel = false;

    [NonSerialized] public List<UI_CharacterButton> characterBtns = new List<UI_CharacterButton>();
    public ToggleGroup equipmentToggleGroup;
    public UI_CharacterEquipmentSlot[] equipmentSlots;
    public RectTransform hideEquipmentContainer;

    private UI_CharacterEquipmentSlot selectedEquipmentSlot;
    private UnitPlayer selectedPlayer;

    void Awake() { Instance = this; }

    void Start()
    {
        characterInfoPanel.SetActive(activePanel);
    }

    public void CharacterUIActive()
    {
        activePanel = !activePanel;
        GameSystem.Instance.State = (activePanel ? GameState.CHECKING : GameState.EXPLORE);

        if (!activePanel)
        {
            equipmentToggleGroup.SetAllTogglesOff();
            characterBtns[0].GetComponent<Toggle>().isOn = true;
            UI_TooltipItem.Hide();
        }

        characterInfoPanel.SetActive(activePanel);
        inventoryPanel.SetActive(activePanel);
    }

    public void SetCharacter(UnitPlayer unitPlayer)
    {
        equipmentToggleGroup.SetAllTogglesOff();

        if (selectedPlayer != null)
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                equipmentSlots[i].EquipmentToHide(hideEquipmentContainer);
            }
        }

        selectedPlayer = unitPlayer;

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].ChangeSkill(selectedPlayer, i);

            if (selectedPlayer.equipmentData[i] != null)
            {
                equipmentSlots[i].CharacterEquip(selectedPlayer.equipmentData[i]);
            }
        }
    }

    public void SetEquipmentSlot(UI_CharacterEquipmentSlot slot)
    {
        selectedEquipmentSlot = slot;
    }

    public void ItemEquip(UI_ItemEquipment uiItem)
    {
        if (selectedEquipmentSlot == null) return;
        
        // 장비칸에 아이템 비 존재 => Equip
        if (activePanel && selectedEquipmentSlot.GetEquiped() == false)
        {
            selectedPlayer.Equip(uiItem, selectedEquipmentSlot.index);
            selectedEquipmentSlot.CharacterEquip(uiItem);
        }
        // 장비칸에 아이템 존재 => Swap
        else if (activePanel && selectedEquipmentSlot.GetEquiped() == true)
        {
            UI_ItemEquipment preEquipment = selectedEquipmentSlot.GetEquipment();

            selectedPlayer.Unequip(preEquipment);
            selectedEquipmentSlot.CharacterUnequip(preEquipment);

            selectedPlayer.Equip(uiItem, selectedEquipmentSlot.index);
            selectedEquipmentSlot.CharacterEquip(uiItem);
        }
    }

    public void ItemUnequip(UI_ItemEquipment uiItem)
    {
        if (selectedEquipmentSlot == null) return;

        // 장비칸에 아이템 존재 => Unequip
        if (activePanel && selectedEquipmentSlot.GetEquiped() == true)
        {
            selectedPlayer.Unequip(uiItem);
            selectedEquipmentSlot.CharacterUnequip(uiItem);
        }
    }
}
