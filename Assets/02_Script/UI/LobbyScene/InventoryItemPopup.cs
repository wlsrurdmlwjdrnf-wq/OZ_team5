using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EnumData;

//로비 인벤용 아이템 팝업
//-Bind로 데이터/슬롯/모드(장착or해제) 세팅 후 열어주는 구조
//-아이콘은 인벤 데이터에 null이 들어올 수 있어 DataManager에서 id로 재조회해서 표시
//-등급 배경이 2개(아이콘 뒤/등급텍스트 뒤)라 Resources에서 각각 따로 로드해서 적용
//-등급/스킬 텍스트가 5개라 같은 효과라도 공격력/체력 증가로 채움
public class InventoryItemPopup : UIPopup
{
    [Header("Main")]
    [SerializeField] private Image gradeBgForText;      //등급 텍스트 뒤 등급배경(Elite Grade)
    [SerializeField] private TextMeshProUGUI gradeText; //등급 텍스트(Nice/Epic...)
    [SerializeField] private TextMeshProUGUI nameText;  //아이템 이름
    [SerializeField] private Image gradeBgForIcon;      //아이템 아이콘 뒤 등급배경(Elite)
    [SerializeField] private Image itemIcon;            //아이템 아이콘

    [Header("Skill/Grade Text (5)")]
    [SerializeField] private TextMeshProUGUI[] infoTexts;//등급 스킬/설명 5줄

    [Header("Stat")]
    [SerializeField] private Image statIcon;            //공격/체력 아이콘(선택)
    [SerializeField] private TextMeshProUGUI statText;  //스탯 텍스트(+숫자만)

    [Header("Buttons")]
    [SerializeField] private Button equipButton;    //장착 버튼
    [SerializeField] private Button unequipButton;  //해제 버튼
    [SerializeField] private Button closeButton;    //닫기 버튼

    [Header("UI Asset Map")]
    [SerializeField] private UIAssetMap uiMap;      //스탯 아이콘 매핑(선택)

    private int slotIndex = -1;
    private bool fromGeneral;                       //true=가방(장착),false=장비(해제)
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

        //이미 켜져있는 상태에서 다른 슬롯 클릭으로 데이터만 바뀌는 케이스 방어
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
        if (cachedData == null)
        {
            Debug.LogError("//InventoryItemPopup OnOpen cachedData == null");
            CloseSelf();
            return;
        }

        if (cachedData.id == 0 || cachedData.type == EquipmentType.NONE)
        {
            Debug.Log($"//InventoryItemPopup OnOpen invalid item id:{cachedData.id} type:{cachedData.type}");
            CloseSelf();
            return;
        }

        RefreshUI(cachedData);
        ApplyButtonMode();
    }

    protected override void OnClose()
    {
        //다음 오픈에서 버튼 모드/인덱스 꼬임 방지
        slotIndex = -1;
        fromGeneral = false;

        //cachedData는 일부러 null로 지우지 않음
        //- 열려있는 상태에서 Bind만 바뀌는 케이스에서 UI 갱신이 필요할 수 있음
    }

    //가방이면 장착 버튼만, 장비면 해제 버튼만 표시
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

    //DataManager 수정 불가 조건이라 아이콘/이름은 id로 재조회해서 안정화
    private ItemData ResolveViewData(ItemData data)
    {
        if (data == null) return null;

        if (DataManager.Instance == null) return data;

        ItemData dmData = DataManager.Instance.GetItemData(data.id);
        if (dmData != null)
        {
            return dmData;
        }

        return data;
    }

    private void RefreshUI(ItemData data)
    {
        if (data == null) return;

        ItemData viewData = ResolveViewData(data);

        ApplyTexts(viewData);
        ApplyItemIcon(viewData);
        ApplyGradeSprites(viewData);
        ApplyStat(viewData);
        ApplyInfoTexts(viewData);

        //정상 갱신 로그(필요 최소)
        Debug.Log($"//InventoryItemPopup RefreshUI id:{viewData.id} name:{viewData.name} tier:{viewData.tier}");
    }

    private void ApplyTexts(ItemData data)
    {
        if (nameText != null)
        {
            nameText.text = data.name;
        }

        if (gradeText != null)
        {
            gradeText.text = data.tier.ToString();
        }
    }

    //아이콘 적용(기본으로 넣었던 이미지가 뜨는 문제 방어)
    private void ApplyItemIcon(ItemData data)
    {
        if (itemIcon == null) return;

        Sprite icon = data.icon;

        //혹시 viewData.icon도 null이면 원본 cachedData에서 마지막 시도
        if (icon == null && cachedData != null && cachedData.id == data.id)
        {
            if (cachedData.icon != null)
            {
                icon = cachedData.icon;
            }
        }

        itemIcon.sprite = icon;
        itemIcon.enabled = (icon != null);
        itemIcon.preserveAspect = true;

        if (icon == null)
        {
            //아이콘 리소스 경로는 프로젝트마다 다를 수 있어서 상세경로는 로그에서 빼고 id/name만 남김
            Debug.LogWarning($"//InventoryItemPopup icon null id:{data.id} name:{data.name}");
        }
    }

    //등급 배경 2개 적용
    //-아이콘 뒤: Elite 같은 이미지
    //-텍스트 뒤: Elite Grade 같은 이미지
    private void ApplyGradeSprites(ItemData data)
    {
        //DataManager.GetItemGrade는 기존 구조 유지(수정 불가)
        Sprite baseGrade = null;

        if (DataManager.Instance != null)
        {
            baseGrade = DataManager.Instance.GetItemGrade(data);
        }
        else
        {
            Debug.LogError("//InventoryItemPopup DataManager.Instance == null");
        }

        //프로젝트에서 "Elite"와 "Elite Grade"가 따로라서
        //Resources/Icons 대신 너가 만든 경로로 로드해야함(너가 이미 해결했다고 했으니)
        //여기선 이름 규칙만 강제:
        //-아이콘 배경: Icons/Grade/{Tier} (예: Elite)
        //-텍스트 배경: Icons/Grade/{Tier} Grade (예: Elite Grade)
        Sprite iconBg = LoadGradeSprite($"{data.tier}");
        Sprite textBg = LoadGradeSprite($"{data.tier} Grade");

        //만약 별도 스프라이트 로드가 실패하면, 기존 DataManager 등급 스프라이트로 폴백
        if (iconBg == null)
        {
            iconBg = baseGrade;
        }

        if (textBg == null)
        {
            textBg = baseGrade;
        }

        ApplyImageSprite(gradeBgForIcon, iconBg);
        ApplyImageSprite(gradeBgForText, textBg);
    }

    private Sprite LoadGradeSprite(string fileName)
    {
        //Resources/Icons/Grade/Elite 같은 형태로 관리한다는 가정
        //너가 폴더를 Grade로 정리했다고 했던 로그 형식과 맞춤
        Sprite s = Resources.Load<Sprite>($"Icons/Grade/{fileName}");
        return s;
    }

    private void ApplyImageSprite(Image target, Sprite sprite)
    {
        if (target == null) return;

        target.sprite = sprite;
        target.enabled = (sprite != null);
        target.preserveAspect = true;
    }

    //스탯 텍스트는 +숫자만
    private void ApplyStat(ItemData data)
    {
        StatKind statKind = GetStatKind(data.type);

        if (statIcon != null)
        {
            if (uiMap != null)
            {
                Sprite sIcon = uiMap.GetStatIcon(statKind);
                statIcon.sprite = sIcon;
                statIcon.enabled = (sIcon != null);
            }
            else
            {
                //아이콘 매핑이 없으면 그냥 숨김
                statIcon.enabled = false;
            }
        }

        if (statText != null)
        {
            if (statKind == StatKind.Attack)
            {
                //요구사항: 같은 효과라도 공격/체력 올라가게끔 -> 일단 공격은 atkPercent 우선, 없으면 atkMtp 표시
                string value = "";

                if (data.atkPercent != 0)
                {
                    value = $"+{data.atkPercent}%";
                }
                else if (data.atkMtp > 0f)
                {
                    value = $"+{data.atkMtp:0.##}";
                }
                else
                {
                    value = "-";
                }

                statText.text = value;
            }
            else
            {
                string value = data.hpPercent != 0 ? $"+{data.hpPercent}%" : "-";
                statText.text = value;
            }
        }
    }

    //infoTexts(5개) 채우기
    //-0:SkillInfo(있으면) / 없으면 "설명 없음"
    //-1:등급
    //-2:타입
    //-3:스탯(+수치만)
    //-4:추가효과(임시)
    private void ApplyInfoTexts(ItemData data)
    {
        if (infoTexts == null || infoTexts.Length == 0) return;

        for (int i = 0; i < infoTexts.Length; i++)
        {
            if (infoTexts[i] != null)
            {
                infoTexts[i].text = "";
            }
        }

        string skillInfo = null;

        if (DataManager.Instance != null && DataManager.Instance.SkillInfo != null)
        {
            if (DataManager.Instance.SkillInfo.TryGetValue(data.id, out string info))
            {
                skillInfo = info;
            }
        }

        if (infoTexts.Length >= 1 && infoTexts[0] != null)
        {
            infoTexts[0].text = string.IsNullOrEmpty(skillInfo) ? "설명 없음" : skillInfo;
        }

        if (infoTexts.Length >= 2 && infoTexts[1] != null)
        {
            infoTexts[1].text = data.tier.ToString();
        }

        if (infoTexts.Length >= 3 && infoTexts[2] != null)
        {
            infoTexts[2].text = data.type.ToString();
        }

        if (infoTexts.Length >= 4 && infoTexts[3] != null)
        {
            StatKind statKind = GetStatKind(data.type);

            if (statKind == StatKind.Attack)
            {
                if (data.atkPercent != 0)
                {
                    infoTexts[3].text = $"+{data.atkPercent}%";
                }
                else if (data.atkMtp > 0f)
                {
                    infoTexts[3].text = $"+{data.atkMtp:0.##}";
                }
                else
                {
                    infoTexts[3].text = "-";
                }
            }
            else
            {
                infoTexts[3].text = data.hpPercent != 0 ? $"+{data.hpPercent}%" : "-";
            }
        }

        if (infoTexts.Length >= 5 && infoTexts[4] != null)
        {
            //프로젝트 룰이 확정되면 여기 교체
            infoTexts[4].text = "-";
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

    //UIManager 스택과 동기화된 닫기
    private void CloseSelf()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ClosePopup(EnumData.PopupId.Item);
            return;
        }

        Close();
    }
}
