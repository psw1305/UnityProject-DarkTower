using UnityEngine;
using TMPro;

public class UI_TooltipItem : MonoBehaviour
{
    public static UI_TooltipItem current { get; private set; }
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    public TextMeshProUGUI itemStat;
    private RectTransform rectTrnf;

    void Awake()
    { 
        current = this; 
        rectTrnf = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public static void Show(Vector3 pos, ItemData item)
    {
        current.rectTrnf.position = pos + new Vector3(0, -50, 0);
        current.gameObject.SetActive(true);

        current.itemName.text = item.ItemName;
        current.itemDesc.text = item.ItemDesc;

        if (item.TypeItem == ItemType.Equipment)
        {
            current.StatToText((ItemDataEquipment)item);
        }
    }

    public static void Hide()
    {
        current.gameObject.SetActive(false);

        current.itemName.text = "아이템 이름";
        current.itemDesc.text = "아이템 설명";
        current.itemStat.text = "아이템 스탯";
    }

    void StatToText(ItemDataEquipment equipment)
    {
        itemStat.text = "";

        for (int i = 0; i < equipment.GetLength(); i++)
        {
            itemStat.text += equipment.GetStatString(i);
            itemStat.text += "\n";
        }
    }
}
