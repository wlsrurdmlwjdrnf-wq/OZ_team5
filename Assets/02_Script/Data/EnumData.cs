using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumData
{
    //장비 아이템 Type
    public enum EquipmentType
    {
        Support = 1,
        Weapon,
        Amor,
        Necklace,
        Gloves,
        Belt,
        Boots
    }

    //장비 아이템 Tier
    public enum EquipmentTier
    {
        Rare = 1,
        Epic,
        Legendary
    }

    //인벤토리 종류
    public enum InventoryType
    {
        General = 1,
        Equipment,
        InGameWeapon,
        InGamePassive
    }
}
