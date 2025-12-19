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
        Belt,
        Gloves,
        Boots,
    }

    //장비 아이템 Tier
    public enum EquipmentTier
    {
        Nice = 1,
        Rare,
        Elite,
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

    public enum SkillType
    {
        Attack = 1,
        Support
    }

    //씬 종류
    public enum sceneType
    {
        TitleScene,
        LobbyScene,
        BattleScene
    }
}
