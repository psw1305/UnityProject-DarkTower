using System;
using System.Collections.Generic;

public class Inventory 
{
    public event EventHandler OnItemListChanged;

    private List<ItemData> itemList;
    private Action<ItemData> useItemAction;

    public Inventory(Action<ItemData> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<ItemData>();
    }

    public void AddItem(ItemData item, int amount, bool inventoryCheck = true) 
    {
        if (item.IsStackable()) 
        {
            bool itemAlreadyInInventory = false;
            foreach (ItemData inventoryItem in itemList) 
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += amount;
                    itemAlreadyInInventory = true;
                }
            }

            if (!itemAlreadyInInventory) 
            {
                itemList.Add(item);
            }
        }
        else 
        {
            itemList.Add(item);
        }

        if (inventoryCheck) OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(ItemData item, bool inventoryCheck = true) 
    {
        if (item.IsStackable())
        {
            ItemData itemInInventory = null;
            foreach (ItemData inventoryItem in itemList) 
            {
                if (inventoryItem.itemType == item.itemType) 
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }

            if (itemInInventory != null && itemInInventory.amount <= 0) 
            {
                itemList.Remove(itemInInventory);
            }
        }
        else 
        {
            itemList.Remove(item);
        }

        if (inventoryCheck) OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }


    public void UseItem(ItemData item)
    {
        useItemAction(item);
    }

    public List<ItemData> GetItemList() 
    {
        return itemList;
    }
}
