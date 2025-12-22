using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    /*
    private Dictionary<EnumData.InventoryType, InventoryData> playerInven;

    [Tooltip("인벤토리별 아이템칸 설정")]
    [SerializeField] private int General = 10; //일반 인벤토리
    [SerializeField] private int Equipment = 6; // 장비칸
    [SerializeField] private int IngameSkill = 6; // 무기스킬칸
    [SerializeField] private int IngamePassive = 6; // 패시브칸

    protected override void Init()
    {          
        base.Init();
        playerInven = new Dictionary<EnumData.InventoryType, InventoryData>
        {
            {EnumData.InventoryType.General, new InventoryData(General) },
            {EnumData.InventoryType.Equipment, new InventoryData(Equipment) },
            {EnumData.InventoryType.InGameWeapon, new InventoryData(IngameSkill) },
            {EnumData.InventoryType.InGamePassive, new InventoryData(IngamePassive) }
        };
    }

    // 아이템 추가 뽑기에서 아이템을 먹으면 알맞은 종류의 인벤토리에 아이템(ID)을 넣을때 호출함
    public void AddItem(EnumData.InventoryType type, int id)
    {
        InventoryData inven = playerInven[type];
        if (inven == null) return;

        int emptySlot = inven.GetEmptySlot();
        if (emptySlot == -1) return;

        ItemData item = DataManager.Instance.GetItemData(id);
        if (item == null) return;

        inven.AddItem(emptySlot, item);
    }

    // 아이템 제거
    public void RemoveItem(EnumData.InventoryType type, int slot)
    {
        InventoryData inven = playerInven[type];

        ItemData item = inven.GetItemInfo(slot);
        if (item == null) return;
        inven.RemoveItem(slot);
    }
    
    // 아이템 정보 반환
    public ItemData GetItemData(EnumData.InventoryType type, int slot)
    {
        InventoryData inven = playerInven[type];
        return inven?.GetItemInfo(slot);
    }
    */
}
