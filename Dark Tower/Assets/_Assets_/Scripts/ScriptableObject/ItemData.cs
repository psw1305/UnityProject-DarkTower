using UnityEngine;

public enum ItemType
{
    Cash, Consumable, Equipment
}

public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public int amount = 1;

    public string itemName;
    public string itemDesc;
    public Sprite itemImage;
    public int itemPrice;

    public ItemType TypeItem
    {
        get
        {
            return itemType;
        }
    }

    public Sprite ItemImage
    {
        get
        {
            return itemImage;
        }
    }

    public string ItemName
    {
        get
        {
            return itemName;
        }
    }

    public string ItemDesc
    {
        get
        {
            return itemDesc;
        }
    }

    public int ItemPrice
    {
        get
        {
            return itemPrice;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Cash:
            case ItemType.Consumable:
                return true;
            case ItemType.Equipment:
                return false;
        }
    }
}
