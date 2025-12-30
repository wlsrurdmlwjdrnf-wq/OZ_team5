using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EnumData;

//로비 인벤용 아이템 팝업
//- 슬롯 클릭으로 열림
//- "장착/해제"는 팝업 버튼에서만 실행
public class InventoryItemPopup : UIPopup
{
    [Header("Main")]
    [SerializeField] private Image gradeBackground;//등급 배경
    [SerializeField] private Image itemIcon;//아이템 아이콘
    [SerializeField] private TextMeshProUGUI nameText;//아이템 이름
    [SerializeField] private TextMeshProUGUI gradeText;//등급 텍스트(추가해달라고 했던거)

    [Header("Stat")]
    [SerializeField] private Image statIcon;//공격/체력 아이콘
    [SerializeField] private TextMeshProUGUI statText;//공격력/체력 증가 텍스트

    [Header("Buttons")]
    [SerializeField] private Button equipButton;//장착 버튼
    [SerializeField] private Button unequipButton;//해제 버튼
    [SerializeField] private Button closeButton;//닫기 버튼(있으면)

    [Header("UI Asset Map")]
    [SerializeField] private UIAssetMap uiMap;//등급 배경/스탯 아이콘 매핑

    private int slotIndex = -1;
    private bool fromGeneral;//true=가방슬롯(장착),false=장비슬롯(해제)
    private ItemData cachedData;

    //외부에서 열기 전에 데이터/슬롯정보를 넣어주는 함수
    public void Bind(ItemData data, int slot, bool isGeneralSlot)
    {
        cachedData = data;
        slotIndex = slot;
        fromGeneral = isGeneralSlot;
    }

    protected override void OnInit()
    {
        //버튼은 한번만 연결
        if (equipButton != null)
        {
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(OnClickEquip);
        }

        if (unequipButton != null)
        {
            unequipButton.onClick.RemoveAllListeners();
            unequipButton.onClick.AddListener(OnClickUnequip);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnClickClose);
        }
    }

    protected override void OnOpen()
    {
        //데이터가 없으면 그냥 닫아버림
        if (cachedData == null) { Close(); return; }

        //빈 아이템이면 닫기
        if (cachedData.id == 0 || cachedData.type == EquipmentType.NONE) { Close(); return; }

        RefreshUI(cachedData);

        //가방슬롯이면 장착 버튼만,장비슬롯이면 해제 버튼만
        if (equipButton != null) equipButton.gameObject.SetActive(fromGeneral);
        if (unequipButton != null) unequipButton.gameObject.SetActive(!fromGeneral);
    }

    private void RefreshUI(ItemData data)
    {
        if (nameText != null) nameText.text = data.name;
        if (gradeText != null) gradeText.text = data.tier.ToString();

        if (gradeBackground != null && uiMap != null)
        {
            Sprite bg = uiMap.GetGradeBackground(data.tier);
            gradeBackground.sprite = bg;
            gradeBackground.enabled = (bg != null);
        }

        if (itemIcon != null)
        {
            //동료쪽 DataManager가 아이콘을 Resources에서 가져오는 구조라서
            //여기서는 DataManager를 통해 가져오는게 안전함
            Sprite icon = DataManager.Instance.GetItemIcon(data);
            itemIcon.sprite = icon;
            itemIcon.enabled = (icon != null);
        }

        StatKind statKind = GetStatKind(data.type);

        if (statIcon != null && uiMap != null)
        {
            Sprite sIcon = uiMap.GetStatIcon(statKind);
            statIcon.sprite = sIcon;
            statIcon.enabled = (sIcon != null);
        }

        if (statText != null)
        {
            if (statKind == StatKind.Attack)
            {
                string percent = data.atkPercent != 0 ? $"+{data.atkPercent}%" : "";
                string mtp = data.atkMtp > 0f ? $"x{data.atkMtp:0.##}" : "";
                string value = (percent == "" && mtp == "") ? "-" : $"{percent} {mtp}".Trim();
                statText.text = $"공격력 {value}";
            }
            else
            {
                string value = data.hpPercent != 0 ? $"+{data.hpPercent}%" : "-";
                statText.text = $"체력 {value}";
            }
        }
    }

    private StatKind GetStatKind(EquipmentType type)
    {
        if (type == EquipmentType.Weapon) return StatKind.Attack;
        if (type == EquipmentType.Necklace) return StatKind.Attack;
        if (type == EquipmentType.Gloves) return StatKind.Attack;
        return StatKind.Hp;
    }

    private void OnClickEquip()
    {
        if (PlayerManager.Instance == null) return;
        if (!fromGeneral) return;
        if (slotIndex < 0) return;

        //가방 슬롯 인덱스로 장착
        PlayerManager.Instance.EquipItem(slotIndex);

        //장착되면 인벤 갱신 이벤트가 돌고 UI가 바뀜(PlayerManager가 OnItemUpdata 호출함) :contentReference[oaicite:4]{index=4}
        Close();
    }

    private void OnClickUnequip()
    {
        if (PlayerManager.Instance == null) return;
        if (fromGeneral) return;
        if (slotIndex < 0) return;

        //장비 슬롯 인덱스로 해제(0~5)
        PlayerManager.Instance.UnEquipItem(slotIndex);

        Close();
    }

    private void OnClickClose()
    {
        Close();
    }
}
