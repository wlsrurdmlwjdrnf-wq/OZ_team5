using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EnumData;

//로비 인벤용 아이템 팝업
//- 슬롯 클릭으로 열림
//- "장착/해제"는 팝업 버튼에서만 실행
//- 닫을 때는 반드시 UIManager를 통해 닫아서 스택을 정리함
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

    //외부에서 팝업을 열기 전에 반드시 호출
    //데이터와 슬롯 정보를 캐싱
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

        //이미 열려있는 상태라면 즉시 UI만 갱신
        if (gameObject.activeInHierarchy)
        {
            RefreshUI(cachedData);
            ApplyButtonMode();
        }
    }

    //최초 1회 초기화
    protected override void OnInit()
    {
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

    //팝업이 열릴 때마다 호출
    protected override void OnOpen()
    {
        //데이터가 없으면 그냥 닫아버림
        if (cachedData == null)
        {
            if (cachedData == null)
            {
                Debug.LogError("//InventoryItemPopup OnOpen cachedData == null");
                CloseSelf();
                return;
            }

            //빈 아이템이면 닫기
            if (cachedData.id == 0 || cachedData.type == EquipmentType.NONE)
            {
                Debug.Log($"//InventoryItemPopup OnOpen invalid item id:{cachedData.id}");
                CloseSelf();
                return;
            }

            //UI 맵 연결 누락은 기능상 치명적이진 않지만, 작업 중에는 빨리 잡히게 로그
            if (uiMap == null)
            {
                Debug.LogWarning("//InventoryItemPopup uiMap == null");
            }

            RefreshUI(cachedData);
            ApplyButtonMode();
        }
    }

    //팝업이 닫힐 때 호출
    protected override void OnClose()
    {
        //다음 오픈에서 상태 꼬임 방지
        slotIndex = -1;
        fromGeneral = false;

        //cachedData는 유지
        //Bind만 갱신되고 Open이 다시 안 불리는 케이스 방어용
    }

    //가방 슬롯이면 장착 버튼만, 장비 슬롯이면 해제 버튼만 표시
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

    //아이템 정보 UI 갱신
    private void RefreshUI(ItemData data)
    {
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
                statText.text = $"{percent}";
            }
            else
            {
                string value = data.hpPercent != 0 ? $"+{data.hpPercent}%" : "-";
                statText.text = $"{value}";
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

    private void CloseSelf()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ClosePopup(PopupId.Item);
            return;
        }

        Close();
    }

    private void OnClickEquip()
    {
        if (PlayerManager.Instance == null) return;
        if (!fromGeneral) return;
        if (slotIndex < 0) return;

        Debug.Log($"//InventoryItemPopup Equip slot:{slotIndex} id:{cachedData?.id}");

        PlayerManager.Instance.EquipItem(slotIndex);

        CloseSelf();
    }

    private void OnClickUnequip()
    {
        if (PlayerManager.Instance == null) return;
        if (fromGeneral) return;
        if (slotIndex < 0) return;

        Debug.Log($"//InventoryItemPopup UnEquip slot:{slotIndex} id:{cachedData?.id}");

        PlayerManager.Instance.UnEquipItem(slotIndex);

        CloseSelf();
    }

    private void OnClickClose()
    {
        Debug.Log("//InventoryItemPopup Click Close");
        CloseSelf();
    }
}