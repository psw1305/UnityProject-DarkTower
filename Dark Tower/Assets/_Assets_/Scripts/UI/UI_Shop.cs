using UnityEngine;

public class UI_Shop : MonoBehaviour
{
    public GameObject itemSample;
    public ItemData[] itemLists;
    public RectTransform shopItemContainer;

    void Start()
    {
        foreach (ItemData itemData in itemLists)
        {
            UI_ShopItem shopItem = Instantiate(itemSample, shopItemContainer).GetComponent<UI_ShopItem>();
            shopItem.Setting(itemData);
        }
    }
}
