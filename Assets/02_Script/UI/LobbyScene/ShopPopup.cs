using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//로비 상점 팝업
//- PopupId.Shop 으로 UIManager가 열고 닫음
//- 버튼 2개(군지원/지구방위) 중 누른 타입에 맞는 박스 프리팹을 보여줌
//- 뽑기 결과(아이템 아이콘/등급 아이콘/이름)를 표시
//- 팝업이 열린 뒤 2초가 지나면 화면 클릭으로 닫을 수 있음
public class ShopPopup : UIPopup
{
    [Header("Box Prefabs (프리팹 2개 연결)")]
    [SerializeField] private GameObject normalBoxPrefab;   //일반 상자 프리팹(군지원 상자)
    [SerializeField] private GameObject highBoxPrefab;     //고급 상자 프리팹(지구방위 보급품)

    [Header("Box Spawn Root (큰 박스가 표시될 자리)")]
    [SerializeField] private Transform boxSpawnRoot;        //프리펩이 붙을 부모(빈 오브젝트 추천)

    [Header("Draw Result UI")]
    [SerializeField] private Image itemIconImage;           //뽑은 아이템 이미지
    [SerializeField] private Image gradeIconImage;          //뽑은 등급 이미지
    [SerializeField] private TextMeshProUGUI itemNameText;  //뽑은 아이템 이름
    [SerializeField] private TextMeshProUGUI gradeText;     //뽑은 아이템 등급 텍스트

    [Header("Close Rule")]
    [SerializeField] private float closeDelay = 1f;         //1초 후 닫기 허용

    private bool canClose;
    private Coroutine delayCo;
    private GameObject currentBox;
    private bool subscribed;

    private void OnEnable()
    {
        Debug.Log($"//ShopPopup OnEnable active:{gameObject.activeInHierarchy}");

        //UIManager를 안 거쳐도(=OnOpen 미호출) 팝업이 켜지는 케이스를 커버
        SafeSetupIfNeeded();
    }

    //팝업이 열릴 때마다 호출
    protected override void OnOpen()
    {
        Debug.Log("//ShopPopup OnOpen");

        SafeSetupIfNeeded();

        //열릴 때 기본 박스 하나 보여주기(원하면 제거 가능)
        ShowBox(normalBoxPrefab);
    }

    //팝업이 닫힐 때마다 호출
    protected override void OnClose()
    {
        Debug.Log("//ShopPopup OnClose");

        canClose = false;

        if (delayCo != null)
        {
            StopCoroutine(delayCo);
            delayCo = null;
        }

        UnsubscribeGacha();
        ClearBox();
    }

    //OnOpen이 안 불리는 경우를 대비해서, 여기서도 동일하게 세팅되도록 묶음
    private void SafeSetupIfNeeded()
    {
        Debug.Log("//ShopPopup SafeSetupIfNeeded");

        ValidateRefs();

        canClose = false;

        if (delayCo != null)
        {
            StopCoroutine(delayCo);
        }
        delayCo = StartCoroutine(CoEnableClose());

        ClearResultUI();

        SubscribeGacha();
    }

    private void ValidateRefs()
    {
        //여기서 빠진 연결이 있으면 바로 로그로 잡힘
        if (boxSpawnRoot == null) Debug.LogError("//ShopPopup boxSpawnRoot == null (인스펙터 연결 필요)");
        if (normalBoxPrefab == null) Debug.LogError("//ShopPopup normalBoxPrefab == null (프리팹 연결 필요)");
        if (highBoxPrefab == null) Debug.LogError("//ShopPopup highBoxPrefab == null (프리팹 연결 필요)");

        if (itemIconImage == null) Debug.LogWarning("//ShopPopup itemIconImage == null");
        if (gradeIconImage == null) Debug.LogWarning("//ShopPopup gradeIconImage == null");
        if (itemNameText == null) Debug.LogWarning("//ShopPopup itemNameText == null");
        if (gradeText == null) Debug.LogWarning("//ShopPopup gradeText == null");
    }

    private IEnumerator CoEnableClose()
    {
        Debug.Log($"//ShopPopup CloseDelay Start: {closeDelay}");

        yield return new WaitForSecondsRealtime(closeDelay);

        canClose = true;
        Debug.Log("//ShopPopup canClose = true");
    }

    private void Update()
    {
        //팝업이 켜져있는 동안만 입력 처리
        if (!gameObject.activeInHierarchy) return;

        if (!canClose) return;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("//ShopPopup Background Click -> Close");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.CloseTopPopup();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    //일반 상자 버튼(군지원)
    public void OnClickNormalBox()
    {
        Debug.Log("//ShopPopup OnClickNormalBox");

        SafeSetupIfNeeded(); //혹시 OnOpen 없이 들어와도 세팅됨

        ShowBox(normalBoxPrefab);

        if (GachaManager.Instance == null)
        {
            Debug.LogError("//GachaManager.Instance == null");
            return;
        }

        Debug.Log("//Call GachaManager.DrawItemNormalBox");
        GachaManager.Instance.DrawItemNormalBox();
    }

    //고급 상자 버튼(지구방위)
    public void OnClickHighBox()
    {
        Debug.Log("//ShopPopup OnClickHighBox");

        SafeSetupIfNeeded();

        ShowBox(highBoxPrefab);

        if (GachaManager.Instance == null)
        {
            Debug.LogError("//GachaManager.Instance == null");
            return;
        }

        Debug.Log("//Call GachaManager.DrawItemEpicBox");
        GachaManager.Instance.DrawItemEpicBox();
    }

    private void SubscribeGacha()
    {
        if (subscribed) return;

        if (GachaManager.Instance == null)
        {
            Debug.LogError("//SubscribeGacha fail: GachaManager.Instance == null");
            return;
        }

        GachaManager.Instance.OnDrawItem -= HandleDrawItem;
        GachaManager.Instance.OnDrawItem += HandleDrawItem;

        subscribed = true;
        Debug.Log("//SubscribeGacha OK");
    }

    private void UnsubscribeGacha()
    {
        if (!subscribed) return;

        if (GachaManager.Instance != null)
        {
            GachaManager.Instance.OnDrawItem -= HandleDrawItem;
        }

        subscribed = false;
        Debug.Log("//UnsubscribeGacha OK");
    }

    private void HandleDrawItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("//HandleDrawItem item == null");
            return;
        }

        Debug.Log($"//HandleDrawItem id:{item.id} name:{item.name} tier:{item.tier}");

        //인벤 저장은 너가 다른쪽에서 한다면 여기 빼도 됨
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.AddItemInven(item);
            Debug.Log("//AddItemInven OK");
        }

        if (DataManager.Instance != null)
        {
            if (itemIconImage != null)
            {
                itemIconImage.sprite = DataManager.Instance.GetItemIcon(item);
                itemIconImage.enabled = (itemIconImage.sprite != null);
            }

            if (gradeIconImage != null)
            {
                gradeIconImage.sprite = DataManager.Instance.GetItemGrade(item);
                gradeIconImage.enabled = (gradeIconImage.sprite != null);
            }
        }
        else
        {
            Debug.LogError("//DataManager.Instance == null");
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

    private void ShowBox(GameObject boxPrefab)
    {
        if (boxSpawnRoot == null)
        {
            Debug.LogError("//ShowBox fail: boxSpawnRoot == null");
            return;
        }

        if (boxPrefab == null)
        {
            Debug.LogError("//ShowBox fail: boxPrefab == null");
            return;
        }

        Debug.Log($"//ShowBox prefab:{boxPrefab.name}");

        ClearBox();

        currentBox = Instantiate(boxPrefab, boxSpawnRoot, false);
        Debug.Log($"//Box Instantiated -> {currentBox.name}");
    }

    private void ClearBox()
    {
        if (currentBox == null) return;

        Debug.Log($"//ClearBox destroy:{currentBox.name}");

        Destroy(currentBox);
        currentBox = null;
    }

    private void ClearResultUI()
    {
        Debug.Log("//ClearResultUI");

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