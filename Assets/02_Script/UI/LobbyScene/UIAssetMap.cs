using UnityEngine;
using static EnumData;

//UI에서 사용하는 스프라이트 매핑용 ScriptableObject
//- enum → Sprite 변환만 담당
//- 로직은 UI 코드에서 처리
[CreateAssetMenu(menuName = "UI/UI Asset Map")]
public class UIAssetMap : ScriptableObject
{
    [Header("Grade Backgrounds")]
    [SerializeField] private Sprite niceBg;
    [SerializeField] private Sprite rareBg;
    [SerializeField] private Sprite eliteBg;
    [SerializeField] private Sprite epicBg;
    [SerializeField] private Sprite legendaryBg;

    [Header("Stat Icons")]
    [SerializeField] private Sprite attackIcon;
    [SerializeField] private Sprite hpIcon;

    //아이템 등급 → 배경 이미지
    public Sprite GetGradeBackground(EquipmentTier tier)
    {
        switch (tier)
        {
            case EquipmentTier.Nice: return niceBg;
            case EquipmentTier.Rare: return rareBg;
            case EquipmentTier.Elite: return eliteBg;
            case EquipmentTier.Epic: return epicBg;
            case EquipmentTier.Legendary: return legendaryBg;
            default: return null;
        }
    }

    //스탯 종류 → 아이콘
    public Sprite GetStatIcon(StatKind kind)
    {
        switch (kind)
        {
            case StatKind.Attack: return attackIcon;
            case StatKind.Hp: return hpIcon;
            default: return null;
        }
    }
}
