using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumData
{
    //장비 아이템 Type
    public enum EquipmentType
    {
        Weapon = 1,
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

    //인게임 아이템 Type (무기스킬, 지원폼)
    public enum IngameItemType
    {
        Weapon = 1,
        Support
    }
}
