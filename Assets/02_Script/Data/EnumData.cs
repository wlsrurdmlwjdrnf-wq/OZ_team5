using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumData
{
    //장비 아이템 Type
    public enum EquipmentType
    {
        Support,
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
}
