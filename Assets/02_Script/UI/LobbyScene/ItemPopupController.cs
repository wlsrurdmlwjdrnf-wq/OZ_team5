using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static EnumData;

// 아이템 팝업
// - 일반슬롯에서 열리면 "장착" 버튼 활성
// - 장착슬롯에서 열리면 "해제" 버튼 활성
public class ItemPopupController : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject root; //팝업 전체 루트

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI nameText; //아이템 이름
    [SerializeField] private TextMeshProUGUI gradeText; //등급 텍스트
    [SerializeField] private Image gradeBackground; //등급 배경 이미지
    [SerializeField] private Image itemIcon; //아이템 아이콘
    [SerializeField] private Image statIcon; //공격/체력 아이콘
    [SerializeField] private TextMeshProUGUI statText; //공격/체력 수치 텍스트

    [Header("Buttons")]
    [SerializeField] private Button equipButton; //장착
    [SerializeField] private Button unEquipButton; //해제
    [SerializeField] private Button closeButton; //닫기

    [Header("Asset Maps")]
    [SerializeField] private UIAssetMap uiMap; //등급배경/스탯아이콘 매핑

    private RectTransform myRect;
    private Canvas parentCanvas;
    private RectTransform canvasRect;

    private enum OpenMode
    {
        None,
        FromGeneral, //일반 인벤 슬롯 클릭(장착)
        FromEquip    //장착 슬롯 클릭(해제)
    }

    private OpenMode mode;
    private int slotNum; //눌린 슬롯 번호
    private ItemData currentItem; //현재 표시중인 아이템

    private void Awake()
    {
        if (root != null) root.SetActive(false);

        myRect = transform as RectTransform;
        parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasRect = parentCanvas.transform as RectTransform;
        }

        if (equipButton != null) equipButton.onClick.AddListener(OnClickEquip);
        if (unEquipButton != null) unEquipButton.onClick.AddListener(OnClickUnEquip);
        if (closeButton != null) closeButton.onClick.AddListener(Hide);
    }

    public void Hide()
    {
        if (root != null) root.SetActive(false);

        mode = OpenMode.None;
        slotNum = -1;
        currentItem = null;
    }

    //일반 인벤 슬롯 클릭 → 장착 팝업
    public void OpenFromGeneralSlot(int generalSlotNum, Vector3 screenPos)
    {
        if (PlayerManager.Instance == null) return;
        if (root == null) return;

        ItemData item = PlayerManager.Instance.playerData.playerGeneralInven[generalSlotNum];
        if (item == null || item.id == 0)
        {
            Hide();
            return;
        }

        mode = OpenMode.FromGeneral;
        slotNum = generalSlotNum;
        currentItem = item;

        ApplyView(item);
        SetButtonMode();

        ShowAtScreenPos(screenPos);
    }

    //장착 슬롯 클릭 → 해제 팝업
    public void OpenFromEquipSlot(int equipSlotNum, Vector3 screenPos)
    {
        if (PlayerManager.Instance == null) return;
        if (root == null) return;

        EquipmentType type = (EquipmentType)equipSlotNum;
        ItemData item = PlayerManager.Instance.playerData.playerEquipInven[type];
        if (item == null || item.id == 0)
        {
            Hide();
            return;
        }

        mode = OpenMode.FromEquip;
        slotNum = equipSlotNum;
        currentItem = item;

        ApplyView(item);
        SetButtonMode();

        ShowAtScreenPos(screenPos);
    }

    //팝업을 클릭 위치 근처에 띄우기(Screen Space - Camera 대응)
    private void ShowAtScreenPos(Vector3 screenPos)
    {
        root.SetActive(true);
        transform.SetAsLastSibling();

        if (parentCanvas == null || canvasRect == null || myRect == null)
        {
            Debug.LogWarning("//ItemPopupController Canvas/RectTransform 연결이 없음");
            return;
        }

        Camera cam = parentCanvas.worldCamera;
        if (cam == null) cam = Camera.main;

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            cam,
            out localPos
        );

        myRect.anchoredPosition = localPos;
    }

    private void SetButtonMode()
    {
        if (equipButton != null) equipButton.gameObject.SetActive(mode == OpenMode.FromGeneral);
        if (unEquipButton != null) unEquipButton.gameObject.SetActive(mode == OpenMode.FromEquip);
    }

    private void ApplyView(ItemData data)
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
            //DataManager가 아이콘을 Resources에서 뽑아주는 구조가 이미 있음 :contentReference[oaicite:4]{index=4}
            Sprite icon = data.icon;
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
                string percent = data.atkPercent != 0 ? $"+{data.atkPercent}%" : "-";
                string mtp = data.atkMtp > 0f ? $" x{data.atkMtp:0.##}" : "";
                statText.text = $"공격력 {percent}{mtp}";
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
        //무기/목걸이/장갑 = 공격
        if (type == EquipmentType.Weapon) return StatKind.Attack;
        if (type == EquipmentType.Necklace) return StatKind.Attack;
        if (type == EquipmentType.Gloves) return StatKind.Attack;

        //갑옷/벨트/신발 = 체력
        return StatKind.Hp;
    }

    private void OnClickEquip()
    {
        if (mode != OpenMode.FromGeneral) return;
        if (PlayerManager.Instance == null) return;

        //일반 인벤 슬롯 번호로 장착 호출(이미 구현돼 있음) :contentReference[oaicite:5]{index=5}
        PlayerManager.Instance.EquipItem(slotNum);
        Hide();
    }

    private void OnClickUnEquip()
    {
        if (mode != OpenMode.FromEquip) return;
        if (PlayerManager.Instance == null) return;

        //장착 슬롯 번호로 해제 호출(이미 구현돼 있음) :contentReference[oaicite:6]{index=6}
        PlayerManager.Instance.UnEquipItem(slotNum);
        Hide();
    }
}
