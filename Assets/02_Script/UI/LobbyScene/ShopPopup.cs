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
    [SerializeField] private GameObject armyBoxPrefab;      //군지원상자 박스 프리팹
    [SerializeField] private GameObject earthBoxPrefab;     //지구방위 보급품 박스 프리팹

    [Header("Box Spawn Root (큰 박스가 표시될 자리)")]
    [SerializeField] private Transform boxSpawnRoot;        //프리팹을 Instantiate할 부모(Panel 안 빈 오브젝트 추천)

    [Header("Draw Result UI")]
    [SerializeField] private Image itemIconImage;           //뽑은 아이템 이미지
    [SerializeField] private Image gradeIconImage;          //뽑은 등급 이미지
    [SerializeField] private TextMeshProUGUI itemNameText;  //뽑은 아이템 이름
    [SerializeField] private TextMeshProUGUI gradeText;     //뽑은 아이템 등급 텍스트

    [Header("Close Rule")]
    [SerializeField] private float closeDelay = 2f;         //2초 후 닫기 허용

    private bool canClose;
    private Coroutine delayCo;
    private GameObject currentBox;

    //팝업이 열릴 때마다 호출
    protected override void OnOpen()
    {
        //열릴 때마다 닫기 딜레이 초기화
        canClose = false;

        if (delayCo != null)
        {
            StopCoroutine(delayCo);
        }
        delayCo = StartCoroutine(CoEnableClose());

        //팝업을 열면 기본 박스(원하면) 하나 보여주기
        ShowBox(armyBoxPrefab);

        //이벤트 중복 구독 방지
        if (GachaManager.Instance == null) return;
        GachaManager.Instance.OnDrawItem -= HandleDrawItem;
        GachaManager.Instance.OnDrawItem += HandleDrawItem;

        //이전에 결과가 남아있을 수 있으니 기본 초기화
        ClearResultUI();
    }

    //팝업이 닫힐 때마다 호출
    protected override void OnClose()
    {
        canClose = false;

        if (delayCo != null)
        {
            StopCoroutine(delayCo);
            delayCo = null;
        }

        //가챠 이벤트 해제(닫혔는데도 결과가 들어오면 UI가 꼬일 수 있음)
        if (GachaManager.Instance != null)
        {
            GachaManager.Instance.OnDrawItem -= HandleDrawItem;
        }

        //박스 프리팹 인스턴스 정리(다음에 열었을 때 깔끔하게)
        ClearBox();
    }

    private IEnumerator CoEnableClose()
    {
        yield return new WaitForSecondsRealtime(closeDelay);
        canClose = true;
    }

    private void Update()
    {
        //팝업이 켜져있는 동안만 입력 처리
        if (!gameObject.activeInHierarchy) return;
        if (!canClose) return;

        if (Input.GetMouseButtonDown(0))
        {
            //UIManager가 Close를 호출해주는 구조라면 CloseTopPopup으로 닫는 게 제일 안전
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

    //군지원상자 버튼
    public void OnClickArmyBox()
    {        
        ShowBox(armyBoxPrefab);

        if (GachaManager.Instance == null) return;
        GachaManager.Instance.DrawItemNormalBox();
    }

    //지구방위 보급품 버튼
    public void OnClickEarthBox()
    {
        ShowBox(earthBoxPrefab);

        if (GachaManager.Instance == null) return;
        GachaManager.Instance.DrawItemEpicBox();
    }

    //뽑기 결과가 나오면 호출됨(GachaManager 이벤트)
    private void HandleDrawItem(ItemData item)
    {
        if (item == null) return;

        //인벤토리에 저장
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.AddItemInven(item);
        }

        //아이콘 / 등급 이미지 표시
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

        //아이템 이름
        if (itemNameText != null)
        {
            itemNameText.text = item.name;
        }

        //아이템 등급 텍스트
        if (gradeText != null)
        {
            gradeText.text = item.tier.ToString(); //Nice / Rare / Epic 등
        }
    }

    //박스 프리팹 표시(기존 인스턴스가 있으면 교체)
    private void ShowBox(GameObject boxPrefab)
    {
        if (boxSpawnRoot == null) return;
        if (boxPrefab == null) return;

        ClearBox();
        currentBox = Instantiate(boxPrefab, boxSpawnRoot, false);
    }

    private void ClearBox()
    {
        if (currentBox == null) return;

        Destroy(currentBox);
        currentBox = null;
    }

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
