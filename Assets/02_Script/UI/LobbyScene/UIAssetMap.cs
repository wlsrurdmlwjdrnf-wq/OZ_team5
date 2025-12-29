using UnityEngine;
using static EnumData;

//등급 배경/스탯 아이콘을 한 곳에서 관리하는 매핑(SO)
[CreateAssetMenu(menuName = "UI/UIAssetMap", fileName = "UIAssetMap")]
public class UIAssetMap : ScriptableObject
{
    [Header("Grade Backgrounds")]
    [SerializeField] private Sprite niceBg;      //Nice 배경
    [SerializeField] private Sprite rareBg;      //Rare 배경
    [SerializeField] private Sprite eliteBg;     //Elite 배경
    [SerializeField] private Sprite epicBg;      //Epic 배경
    [SerializeField] private Sprite legendaryBg; //Legendary 배경

    [Header("Stat Icons")]
    [SerializeField] private Sprite attackIcon;  //공격 아이콘
    [SerializeField] private Sprite hpIcon;      //체력 아이콘

    public Sprite GetGradeBackground(EquipmentTier tier)
    {
        if (tier == EquipmentTier.Nice) return niceBg;
        if (tier == EquipmentTier.Rare) return rareBg;
        if (tier == EquipmentTier.Elite) return eliteBg;
        if (tier == EquipmentTier.Epic) return epicBg;
        if (tier == EquipmentTier.Legendary) return legendaryBg;
        return null;
    }

    public Sprite GetStatIcon(StatKind kind)
    {
        if (kind == StatKind.Attack) return attackIcon;
        return hpIcon;
    }
}

//표시용 스탯 종류(아이콘 선택용)
public enum StatKind
{
    Attack,
    Hp
}
