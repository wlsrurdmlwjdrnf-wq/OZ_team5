using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumData
{
    //장비 아이템 Type
    public enum EquipmentType
    {
        Weapon,
        Amor,
        Necklace,
        Belt,
        Gloves,
        Boots,
        NONE = -1
    }

    //장비 아이템 Tier
    public enum EquipmentTier
    {
        Nice = 1,
        Rare,
        Elite,
        Epic,
        Legendary,
        NONE
           
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
        Support,
        NONE
    }

    //씬 종류
    public enum sceneType
    {
        TitleScene,
        LobbyScene,
        BattleScene
    }

    //팝업 UI 식별 전용 ID
    public enum PopupId
    {
        Pause,       //일시정지
        Settings,    //설정
        Confirm,     //확인/취소
        GameOver,    //게임오버
        GameClear,   //클리어
        LevelUp      //레벨업 알림
    }
}
