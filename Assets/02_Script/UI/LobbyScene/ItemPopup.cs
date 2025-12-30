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
    [SerializeField] private GameObject root;              //전체 루트(켜기/끄기)

    [Header("Main")]
    [SerializeField] private Image gradeBackground;        //등급 배경
    [SerializeField] private Image itemIcon;               //아이템 아이콘(있으면)
    [SerializeField] private TextMeshProUGUI nameText;     //이름
    [SerializeField] private TextMeshProUGUI gradeText;    //등급

    [Header("Stat")]
    [SerializeField] private Image statIcon;               //공격/체력 아이콘
    [SerializeField] private TextMeshProUGUI statText;     //공격력/체력 증가 텍스트

    [Header("UI Asset Map")]
    [SerializeField] private UIAssetMap uiMap;             //등급 배경/스탯 아이콘 매핑

    private Canvas parentCanvas;
    private RectTransform canvasRect;
    private RectTransform myRect;

    private void Awake()
    {
        if (root != null)
        {
            root.SetActive(false);
        }

        //부모 Canvas 캐싱(툴팁은 Canvas 아래에 있어야 함)
        parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasRect = parentCanvas.transform as RectTransform;
        }

        myRect = transform as RectTransform;
    }

    public void Hide()
    {
        if (root != null)
        {
            root.SetActive(false);
        }
    }

    //툴팁 표시(핵심: Screen Space - Camera 좌표 변환 포함)
    public void Show(ItemData data, Vector3 screenPos)
    {
        if (data == null) return;
        if (root == null) return;
        if (parentCanvas == null) return;
        if (canvasRect == null) return;
        if (myRect == null) return;

        //툴팁 켜기
        root.SetActive(true);

        //다른 UI에 가려지는 경우가 많아서 항상 최상단으로 올림
        transform.SetAsLastSibling();

        //Screen Space - Camera에서 좌표 변환에 사용할 카메라 선택
        Camera cam = parentCanvas.worldCamera;
        if (cam == null)
        {
            cam = Camera.main;
            Debug.LogWarning("//Canvas worldCamera가 null이라 Camera.main으로 대체");
        }

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            cam,
            out localPos
        );

        //Canvas 기준 로컬 위치로 배치
        myRect.anchoredPosition = localPos;

        //디버그(보일 때까지 잠깐만 켜두고 나중에 지워도 됨)
        Debug.Log($"//ItemPopup Show ok name:{data.name} screen:{screenPos} local:{localPos} active:{root.activeSelf}");

        //UI 갱신
        if (nameText != null)
        {
            nameText.text = data.name;
        }

        if (gradeText != null)
        {
            gradeText.text = data.tier.ToString();
        }

        //등급 배경(없으면 비활성)
        if (gradeBackground != null && uiMap != null)
        {
            Sprite bg = uiMap.GetGradeBackground(data.tier);
            gradeBackground.sprite = bg;
            gradeBackground.enabled = (bg != null);
        }

        //아이콘
        if (itemIcon != null)
        {
            //ItemData에 icon이 있다면 그대로 사용
            itemIcon.sprite = data.icon;
            itemIcon.enabled = (data.icon != null);
        }

        //아이템 타입 → 스탯 종류 결정(무기/목걸이/장갑:공격, 갑옷/벨트/신발:체력)
        StatKind statKind = GetStatKind(data.type);

        //스탯 아이콘
        if (statIcon != null && uiMap != null)
        {
            Sprite icon = uiMap.GetStatIcon(statKind);
            statIcon.sprite = icon;
            statIcon.enabled = (icon != null);
        }

        //스탯 텍스트(공격은 배수/퍼센트, 체력은 퍼센트)
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
        //무기/목걸이/장갑 = 공격
        if (type == EquipmentType.Weapon) return StatKind.Attack;
        if (type == EquipmentType.Necklace) return StatKind.Attack;
        if (type == EquipmentType.Gloves) return StatKind.Attack;

        //갑옷/벨트/신발 = 체력
        return StatKind.Hp;
    }
}


