using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public Dictionary<int, ItemData> items = new Dictionary<int, ItemData>();

    private int maxCapacity;

    public event Action<EnumData.InventoryType,int, ItemData> OnAddItem;
    public event Action<EnumData.InventoryType, int, ItemData> OnRemoveItem;

    public Inventory(int capacity)
    {
        this.maxCapacity = capacity;
    }

    public ItemData GetItem(int slot)
    {
        return items[slot];
    }

}
