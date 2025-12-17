using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    private Dictionary<EnumData.InventoryType, Inventory> playerInven;

    [Tooltip("인벤토리별 아이템칸 설정")]
    [SerializeField] private int General = 10; //일반 인벤토리
    [SerializeField] private int Equipment = 6; // 장비칸
    [SerializeField] private int IngameSkill = 6; // 무기스킬칸
    [SerializeField] private int IngamePassive = 6; // 패시브칸


    protected override void Init()
    {          
        base.Init();
        playerInven = new Dictionary<EnumData.InventoryType, Inventory>
        {
            {EnumData.InventoryType.General, new Inventory(General) },
            {EnumData.InventoryType.Equipment, new Inventory(Equipment) },
            {EnumData.InventoryType.InGameWeapon, new Inventory(IngameSkill) },
            {EnumData.InventoryType.InGamePassive, new Inventory(IngamePassive) }
        };
    }

    // 아이템 추가
    public void AddItem(EnumData.InventoryType type, int id)
    {
        Inventory inven = playerInven[type];
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
        Inventory inven = playerInven[type];

        ItemData item = inven.GetItemInfo(slot);
        if (item == null) return;
        inven.RemoveItem(slot);
    }
    
    // 아이템 정보 반환
    public ItemData GetItemData(EnumData.InventoryType type, int slot)
    {
        Inventory inven = playerInven[type];
        return inven?.GetItemInfo(slot);
    }
}
