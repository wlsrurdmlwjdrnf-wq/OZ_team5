using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class _PlayerInventory : MonoBehaviour
{
    /*
    private Dictionary<int, EquipmentItemData> inventory = new Dictionary<int, EquipmentItemData>();
    private Dictionary<EnumData.EquipmentType, EquipmentItemData> equipInventory = new Dictionary<EnumData.EquipmentType, EquipmentItemData>();


    [SerializeField] private int invenMaxCount = 10;
    //[SerializeField] private int equipMaxCount = (int)EnumData.EquipmentType.COUNT - 1;

    [Tooltip("인벤토리의 정보가 바뀌면 외부에 알리는 액션")]
    public event Action<int, EquipmentItemData> OnAddInvenItem;
    public event Action<int, EquipmentItemData> OnRemoveInvenItem;
    public event Action<EnumData.EquipmentType, EquipmentItemData> OnAddEquipInvenItem;
    public event Action<EnumData.EquipmentType, EquipmentItemData> OnRemoveEquipInvenItem;

    // 빈슬롯 반환 없으면 -1
    public int GetEmptyInvenSlot()
    {
        for (int i = 0; i < invenMaxCount; i++)
        {
            if (!inventory.ContainsKey(i))
            {
                return i;
            }
        }
        return -1;
    }
    
    // 해당슬롯의 아이템 정보 반환 없으면 null
    public EquipmentItemData GetInvenItemData(int slot)
    {
        return inventory.ContainsKey(slot) ? inventory[slot] : null;
    }


    // 해당슬롯에 아이템 추가
    public void AddInven(int slot, EquipmentItemData data)
    {
        inventory[slot] = data;
        OnAddInvenItem?.Invoke(slot, data);
    }

    
    // 해당슬롯 아이템 제거
    public void RemoveInven(int slot)
    {
        if (inventory.ContainsKey(slot))
        {
            EquipmentItemData data = inventory[slot];
            inventory.Remove(slot);
            OnRemoveInvenItem?.Invoke(slot, data);
        }        
    }

    // 장비칸에 아이템장착
    // 기존에 착용하던 장비가 있다면 리턴 없으면 null
    public EquipmentItemData SetEquipItem(EnumData.EquipmentType type, EquipmentItemData data)
    {
        EquipmentItemData oldItem = null;

        if (equipInventory.ContainsKey(type))
        {
            oldItem = equipInventory[type];
        }

        equipInventory[type] = data;
        OnAddEquipInvenItem?.Invoke(type, data);
        return oldItem;
    }

    // 장비칸에 아이템장착 해제
    // 해제한 아이템의 정보를 반환
    public EquipmentItemData RemoveEquipItem(EnumData.EquipmentType type)
    {
        if (equipInventory.ContainsKey(type))
        {
            EquipmentItemData removed = equipInventory[type];
            equipInventory.Remove(type);
            OnRemoveEquipInvenItem?.Invoke(type, removed);
            return removed;
        }
        return null;
    }
    */
}
    
