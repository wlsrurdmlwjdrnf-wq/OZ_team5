using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EnumData;

//인벤토리 아이템 툴팁(표시 전용)
//- 마우스 오버 시 Show, 마우스 아웃 시 Hide
//- ItemData(동료 테이블) 기반으로 UI만 갱신
//- 클릭/장착 로직은 여기서 하지 않음
public class ItemPopup : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject root; //전체 루트(켜기/끄기)

    [Header("Main")]
    [SerializeField] private Image gradeBackground;        //등급 배경
    [SerializeField] private Image itemIcon;               //아이템 아이콘(있으면)
    [SerializeField] private TextMeshProUGUI nameText;     //이름
    [SerializeField] private TextMeshProUGUI gradeText;    //등급

    [Header("Stat")]
    [SerializeField] private Image statIcon;               //공격/체력 아이콘
    [SerializeField] private TextMeshProUGUI statText;     //공격력/체력 증가 텍스트

    [Header("Effects")]
    [SerializeField] private TextMeshProUGUI effectText;   //특수효과/조합/진화 같은 정보(없으면 숨김)

    [Header("UI Asset Map")]
    [SerializeField] private UIAssetMap uiMap;             //등급 배경/스탯 아이콘 매핑

    private void Awake()
    {
        if (root != null) root.SetActive(false);
    }

    public void Hide()
    {
        if (root != null) root.SetActive(false);
    }

    //인벤토리 슬롯에서 ItemData를 받아서 툴팁 표시
    public void Show(ItemData data, Vector3 screenPos)
    {
        if (data == null) return;

        if (root != null) root.SetActive(true);

        transform.position = screenPos;

        if (nameText != null) nameText.text = data.name;
        if (gradeText != null) gradeText.text = data.tier.ToString();

        //등급 배경
        if (gradeBackground != null && uiMap != null)
        {
            Sprite bg = uiMap.GetGradeBackground(data.tier);
            gradeBackground.sprite = bg;
            gradeBackground.enabled = (bg != null);
        }

        //아이콘은 현재 ItemData에 iconPath가 없어서 당장은 비워둠
        //동료 테이블에 iconPath가 따로 있다면, 여기서 로드해서 넣으면 됨
        if (itemIcon != null)
        {
            itemIcon.enabled = false;
        }

        //장비 타입에 따라 공격/체력 표시 규칙 적용
        StatKind statKind = GetStatKind(data.type);

        //스탯 아이콘
        if (statIcon != null && uiMap != null)
        {
            Sprite sIcon = uiMap.GetStatIcon(statKind);
            statIcon.sprite = sIcon;
            statIcon.enabled = (sIcon != null);
        }

        //스탯 텍스트
        if (statText != null)
        {
            if (statKind == StatKind.Attack)
            {
                //공격 장비: 배수/퍼센트 같이 보여주기
                //atkMtp가 0이면 표시 안 하도록 방어
                string mtp = data.atkMtp > 0f ? $"x{data.atkMtp:0.##}" : "";
                string pct = data.atkPercent != 0 ? $"+{data.atkPercent}%" : "";

                //둘 다 없으면 "-"
                string value = (mtp == "" && pct == "") ? "-" : $"{pct} {mtp}".Trim();
                statText.text = $"공격력 {value}";
            }
            else
            {
                //체력 장비
                string value = data.hpPercent != 0 ? $"+{data.hpPercent}%" : "-";
                statText.text = $"체력 {value}";
            }
        }

        //특수효과/조합/진화 정보는 현재는 ID만 존재
        //나중에 SO/테이블 매핑해서 "텍스트"로 바꾸는 자리에 해당
        if (effectText != null)
        {
            string effectLine = BuildEffectLine(data);

            //표시할 내용이 없으면 숨김
            if (string.IsNullOrEmpty(effectLine))
            {
                effectText.gameObject.SetActive(false);
            }
            else
            {
                effectText.gameObject.SetActive(true);
                effectText.text = effectLine;
            }
        }
    }

    private StatKind GetStatKind(EquipmentType type)
    {
        //무기/목걸이/장갑 = 공격
        if (type == EquipmentType.Weapon) return StatKind.Attack;
        if (type == EquipmentType.Necklace) return StatKind.Attack;
        if (type == EquipmentType.Gloves) return StatKind.Attack;

        //갑옷/벨트/신발 = 체력
        return StatKind.Hp;
    }

    //ID 기반 정보 표시(나중에 실제 텍스트로 변환할 자리)
    private string BuildEffectLine(ItemData data)
    {
        bool hasSpecial = data.specialEffectID >= 0;
        bool hasEvolution = data.evolutionID >= 0;

        bool hasPair = false;
        if (data.pairID != null && data.pairID.Length > 0)
        {
            //-1만 있는 경우 제외하고 싶으면 추가 필터 가능
            hasPair = true;
        }

        if (!hasSpecial && !hasPair && !hasEvolution) return "";

        //지금은 "ID"만 표시
        //나중에 여기서 DataManager로 이름/설명 가져와서 예쁘게 표기하면 됨
        string line = "";

        if (hasSpecial) line += $"특수효과ID:{data.specialEffectID} ";
        if (hasPair) line += $"조합ID:{FormatPairIds(data.pairID)} ";
        if (hasEvolution) line += $"진화ID:{data.evolutionID}";

        return line.Trim();
    }

    private string FormatPairIds(int[] pairIds)
    {
        if (pairIds == null || pairIds.Length == 0) return "-";
        return string.Join(",", pairIds);
    }
}


