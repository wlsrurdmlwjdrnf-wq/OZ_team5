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
    [SerializeField] private Image gradeBackground;     //등급 배경
    [SerializeField] private Image itemIcon;            //아이템 아이콘
    [SerializeField] private TextMeshProUGUI nameText;  //아이템 이름
    [SerializeField] private TextMeshProUGUI gradeText; //등급 텍스트(추가해달라고 했던거)

    [Header("Stat")]
    [SerializeField] private Image statIcon;            //공격/체력 아이콘
    [SerializeField] private TextMeshProUGUI statText;  //공격력/체력 증가 텍스트

    [Header("Buttons")]
    [SerializeField] private Button equipButton;        //장착 버튼
    [SerializeField] private Button unequipButton;      //해제 버튼
    [SerializeField] private Button closeButton;        //닫기 버튼

    [Header("UI Asset Map")]
    [SerializeField] private UIAssetMap uiMap;          //등급 배경/스탯 아이콘 매핑

    private int slotIndex = -1;
    private bool fromGeneral;           //true=가방슬롯(장착),false=장비슬롯(해제)
    private ItemData cachedData;

    //외부에서 열기 전에 데이터/슬롯정보를 넣어주는 함수
    public void Bind(ItemData data, int slot, bool isGeneralSlot)
    {
        if (data == null)
        {
            Debug.LogError("//InventoryItemPopup Bind data == null");
            return;
        }

        cachedData = data;
        slotIndex = slot;
        fromGeneral = isGeneralSlot;

        Debug.Log($"//InventoryItemPopup Bind id:{data.id} slot:{slot} fromGeneral:{isGeneralSlot}");

        //이미 켜져있으면 즉시 갱신(연속 클릭 UX)
        if (gameObject.activeInHierarchy)
        {
            RefreshUI(cachedData);
            ApplyButtonMode();
        }
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
        if (cachedData == null)
        {
            Debug.LogError("//InventoryItemPopup OnOpen cachedData == null");
            Close();
            return;
        }

        //빈 아이템이면 닫기
        if (cachedData.id == 0 || cachedData.type == EquipmentType.NONE)
        {
            Debug.Log($"//InventoryItemPopup OnOpen empty item id:{cachedData.id} type:{cachedData.type}");
            Close();
            return;
        }

        //UI 맵 연결 누락은 기능상 치명적이진 않지만, 작업 중에는 빨리 잡히게 로그
        if (uiMap == null)
        {
            Debug.LogWarning("//InventoryItemPopup uiMap == null (등급/스탯 아이콘이 비어보일 수 있음)");
        }

        RefreshUI(cachedData);
        ApplyButtonMode();
    }

    protected override void OnClose()
    {
        //다음 오픈에서 버튼 모드 꼬이는 걸 방지
        slotIndex = -1;
        fromGeneral = false;

        //데이터를 null로 지우면, Open이 호출되지 않는 케이스에서
        //Bind만 바뀌었는데 UI가 갱신되지 않는 상황이 생길 수 있음
        //cachedData = null;

        //원하면 UI만 비워서 잔상 방지
        //ClearUI();
    }

    //가방슬롯이면 장착 버튼만, 장비슬롯이면 해제 버튼만
    private void ApplyButtonMode()
    {
        if (equipButton != null)
        {
            equipButton.gameObject.SetActive(fromGeneral);
        }

        if (unequipButton != null)
        {
            unequipButton.gameObject.SetActive(!fromGeneral);
        }
    }

    private void RefreshUI(ItemData data)
    {
        if (data == null) return;

        if (nameText != null)
        {
            nameText.text = data.name;
        }
        if (gradeText != null)
        {
            gradeText.text = data.tier.ToString();
        }

        if (gradeBackground != null && uiMap != null)
        {
            Sprite bg = uiMap.GetGradeBackground(data.tier);
            gradeBackground.sprite = bg;
            gradeBackground.enabled = (bg != null);
        }

        if (itemIcon != null)
        {
            if (DataManager.Instance == null)
            {
                Debug.LogError("//InventoryItemPopup DataManager.Instance == null");
                itemIcon.sprite = null;
                itemIcon.enabled = false;
            }
            else
            {
                //아이콘은 DataManager 통해 가져오는게 안전(프로젝트 구조상)
                Sprite icon = data.icon;
                itemIcon.sprite = icon;
                itemIcon.enabled = (icon != null);
            }
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

        Debug.Log($"//InventoryItemPopup Equip slot:{slotIndex} id:{cachedData?.id}");

        //가방 슬롯 인덱스로 장착
        PlayerManager.Instance.EquipItem(slotIndex);

        Close();
    }

    private void OnClickUnequip()
    {
        if (PlayerManager.Instance == null) return;
        if (fromGeneral) return;
        if (slotIndex < 0) return;

        Debug.Log($"//InventoryItemPopup UnEquip slot:{slotIndex} id:{cachedData?.id}");

        //장비 슬롯 인덱스로 해제(0~5)
        PlayerManager.Instance.UnEquipItem(slotIndex);

        Close();
    }

    private void OnClickClose()
    {
        Close();
    }
}