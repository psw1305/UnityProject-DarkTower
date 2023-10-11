using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    public static UI_Inventory Instance;

    public Transform itemContainer;
    public Transform itemSlotMaterials;
    public Transform itemSlotEquipment;
    public Transform itemSlotConsumable;

    private Inventory _inventory;

    void Awake() { Instance = this; }

    public void SetInventory(Inventory inventory)
    {
        _inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemContainer)
        {
            if (child == itemSlotEquipment) continue;
            Destroy(child.gameObject);
        }

        foreach (ItemData item in _inventory.GetItemList())
        {
            RectTransform itemSlotRectTrnf = Instantiate(itemSlotEquipment, itemContainer).GetComponent<RectTransform>();
            itemSlotRectTrnf.gameObject.SetActive(true);

            itemSlotRectTrnf.GetComponent<UI_ItemEquipment>().SetItem(item);
            // UI 아이템 사용
            itemSlotRectTrnf.GetComponent<UI_ItemEquipment>().ClickFunc = () => { /*inventory.UseItem(item);*/ };
            // UI 아이템 드랍
            itemSlotRectTrnf.GetComponent<UI_ItemEquipment>().MouseRightClickFunc = () =>
            {
                ItemData duplicateItem = new ItemData { itemType = item.itemType, amount = item.amount };
                _inventory.RemoveItem(item);
                ItemWorld.DropItem(ExplorePlayer.Instance.transform.position, duplicateItem);
            };

            Image image = itemSlotRectTrnf.Find("Image").GetComponent<Image>();
            image.sprite = item.ItemImage;

            TextMeshProUGUI uiText = itemSlotRectTrnf.Find("Amount").GetComponent<TextMeshProUGUI>();
                      
            if(item.amount > 1) uiText.SetText(item.amount.ToString());
            else uiText.SetText("");
        }
    }
}
