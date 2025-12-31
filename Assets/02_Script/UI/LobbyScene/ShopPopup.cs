using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//로비 상점 팝업
//- PopupId.Shop 으로 UIManager가 열고 닫음
//- 버튼 2개(군지원/지구방위) 중 누른 타입에 맞는 박스 프리팹을 보여줌
//- 뽑기 결과(아이템 아이콘/등급 아이콘/이름)를 표시
//- 팝업이 열린 뒤 2초가 지나면 화면 클릭으로 닫을 수 있음
public class ShopPopup : UIPopup
{
    [Header("Box Prefabs (프리팹 2개 연결)")]
    [SerializeField] private GameObject normalBoxPrefab;    //일반 상자 프리팹(군지원 상자)
    [SerializeField] private GameObject highBoxPrefab;      //고급 상자 프리팹(지구방위 보급품)

    [Header("Box Spawn Root (큰 박스가 표시될 자리)")]
    [SerializeField] private Transform boxSpawnRoot;        //프리팹이 붙을 부모(빈 오브젝트 추천)

    [Header("Draw Result UI")]
    [SerializeField] private Image itemIconImage;           //뽑은 아이템 이미지
    [SerializeField] private Image gradeIconImage;          //뽑은 등급 이미지
    [SerializeField] private TextMeshProUGUI itemNameText;  //뽑은 아이템 이름
    [SerializeField] private TextMeshProUGUI gradeText;     //뽑은 아이템 등급 텍스트

    [Header("Close Rule")]
    [SerializeField] private float closeDelay = 1f;         //이 시간 이후부터 배경 클릭으로 닫기 허용

    private EnumData.BoxType selectedBoxType = EnumData.BoxType.Normal; //선택된 상자 타입

    private bool canClose;          //닫기 가능 여부(딜레이 이후 true)
    private Coroutine delayCo;      //닫기 딜레이 코루틴
    private GameObject currentBox;  //현재 표시 중인 상자 프리팹
    private bool subscribed;        //가챠 이벤트 구독 여부

    //ShopUIController에서 호출
    //- 어떤 상자를 보여줄지 미리 세팅
    public void ShowSelectedBox(EnumData.BoxType type)
    {
        selectedBoxType = type;
        Debug.Log($"//ShopPopup SelectedBoxType = {type}");
    }

    //팝업이 열릴 때 호출(UIManager.ShowPopup)
    protected override void OnOpen()
    {
        Debug.Log("//ShopPopup Open");

        SafeSetup();
        ShowBox();
    }

    //팝업이 닫힐 때 호출(UIManager.ClosePopup)
    protected override void OnClose()
    {
        Debug.Log("//ShopPopup Close");

        canClose = false;

        if (delayCo != null)
        {
            StopCoroutine(delayCo);
            delayCo = null;
        }

        UnsubscribeGacha();
        ClearBox();
        ClearResultUI();
    }

    //팝업이 열린 상태에서 필요한 초기화 처리
    private void SafeSetup()
    {
        canClose = false;

        if (delayCo != null)
        {
            StopCoroutine(delayCo);
        }

        //닫기 딜레이 시작
        delayCo = StartCoroutine(CoEnableClose());

        ClearResultUI();
        SubscribeGacha();
    }

    //closeDelay 이후부터 닫기 가능 상태로 전환
    private IEnumerator CoEnableClose()
    {
        yield return new WaitForSecondsRealtime(closeDelay);

        canClose = true;
        Debug.Log("//ShopPopup canClose = true");
    }

    //1초 이후 화면 아무 곳이나 클릭 시 팝업 닫기
    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!canClose) return;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("//ShopPopup Screen Click -> Close");
            CloseSelf();
        }
    }

    //자기 자신(상점 팝업) 닫기
    private void CloseSelf()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ClosePopup(EnumData.PopupId.Shop);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    //선택한 상자 프리팹을 화면에 표시
    private void ShowBox()
    {
        if (boxSpawnRoot == null)
        {
            Debug.LogError("//ShopPopup boxSpawnRoot == null");
            return;
        }

        GameObject prefab = null;

        if (selectedBoxType == EnumData.BoxType.Normal)
        {
            prefab = normalBoxPrefab;
        }
        else if (selectedBoxType == EnumData.BoxType.High)
        {
            prefab = highBoxPrefab;
        }

        if (prefab == null)
        {
            Debug.LogError("//ShopPopup box prefab == null");
            return;
        }

        ClearBox();
        currentBox = Instantiate(prefab, boxSpawnRoot, false);

        Debug.Log($"//ShopPopup ShowBox -> {prefab.name}");
    }

    //가챠 결과 이벤트 구독(팝업이 열려있는 동안만)
    private void SubscribeGacha()
    {
        if (subscribed) return;
        if (GachaManager.Instance == null) return;

        GachaManager.Instance.OnDrawItem -= HandleDrawItem;
        GachaManager.Instance.OnDrawItem += HandleDrawItem;

        subscribed = true;
        Debug.Log("//ShopPopup SubscribeGacha");
    }

    //가챠 결과 이벤트 해제
    private void UnsubscribeGacha()
    {
        if (!subscribed) return;

        if (GachaManager.Instance != null)
        {
            GachaManager.Instance.OnDrawItem -= HandleDrawItem;
        }

        subscribed = false;
    }

    //가챠 결과 수신 후 UI 반영
    private void HandleDrawItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("//HandleDrawItem item == null");
            return;
        }

        Debug.Log($"//Draw Item: {item.name} ({item.tier})");

        //플레이어 인벤토리에 아이템 추가
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.AddItemInven(item);
        }

        //아이콘 및 등급 UI 갱신
        if (DataManager.Instance != null)
        {
            if (itemIconImage != null)
            {
                itemIconImage.sprite = DataManager.Instance.GetItemIcon(item);
                itemIconImage.enabled = true;
            }

            if (gradeIconImage != null)
            {
                gradeIconImage.sprite = DataManager.Instance.GetItemGrade(item);
                gradeIconImage.enabled = true;
            }
        }

        if (itemNameText != null)
        {
            itemNameText.text = item.name;
        }

        if (gradeText != null)
        {
            gradeText.text = item.tier.ToString();
        }
    }


    //기존에 표시 중이던 상자 제거
    private void ClearBox()
    {
        if (currentBox == null) return;

        Destroy(currentBox);
        currentBox = null;
    }

    //결과 UI 초기화
    private void ClearResultUI()
    {
        if (itemIconImage != null)
        {
            itemIconImage.sprite = null;
            itemIconImage.enabled = false;
        }

        if (gradeIconImage != null)
        {
            gradeIconImage.sprite = null;
            gradeIconImage.enabled = false;
        }

        if (itemNameText != null)
        {
            itemNameText.text = "";
        }

        if (gradeText != null)
        {
            gradeText.text = "";
        }
    }
}