using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemData itemData;
    private Vector3 tipPos;

    public Image itemImage;
    public TextMeshProUGUI itemPriceText;
    public Button buyButton;

    public void Setting(ItemData itemData)
    {
        this.itemData = itemData;
        itemImage.sprite = itemData.ItemImage;
        itemPriceText.text = itemData.ItemPrice.ToString();

        buyButton.onClick.AddListener(Buy);
    }

    void Buy()
    {
        UI_TooltipItem.Hide();
        TradeSystem.SpendCash(itemData.ItemPrice);
        ExplorePlayer.Instance.AddItemToInventory(itemData, 1);
        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI_TooltipItem.Show(transform.position + new Vector3(0, -50, 0), itemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_TooltipItem.Hide();
    }
}
