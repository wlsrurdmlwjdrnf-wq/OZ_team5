using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 인벤토리의 컨테이너 모음 생성자로 인벤토리칸 설정
public class Inventory
{
    public Dictionary<int, ItemData> items = new Dictionary<int, ItemData>();

    private int maxCapacity;

    public event Action<int, ItemData> OnAddItem;
    public event Action<int, ItemData> OnRemoveItem;

    public Inventory(int capacity)
    {
        this.maxCapacity = capacity;
    }

    // 제일 앞 빈슬롯 반환
    public int GetEmptySlot()
    {
        for (int i = 0; i < maxCapacity; i++)
        {
            if (!items.ContainsKey(i)) return i;
        }
        return -1;
    }

    // 슬롯의 아이템정보 반환
    public ItemData GetItemInfo(int slot)
    {
        return items.ContainsKey(slot) ? items[slot] : null;
    }

    // 슬롯에 아이템추가 action으로 추가됬다고 외부에 알림
    public void AddItem(int slot, ItemData item)
    {
        items[slot] = item;
        OnAddItem?.Invoke(slot, item);
    }

    // 슬롯에 아이템제거 action으로 제거됬다고 외부에 알림
    public void RemoveItem(int slot)
    {
        if (items.ContainsKey(slot))
        {
            ItemData item = items[slot];
            items.Remove(slot);
            OnRemoveItem?.Invoke(slot, item);
        }
    }

}
