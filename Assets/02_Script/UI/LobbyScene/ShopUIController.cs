using System.Reflection;
using UnityEngine;

//로비 상점 화면(UI Screen) 컨트롤러
//- 군지원/지구방위 상자 버튼 입력 처리
//- 선택한 박스 타입을 ShopPopup에 전달
//- 골드 차감 + 뽑기 실행 책임
//- 뽑기 결과를 이벤트로 받아 "즉시 인벤에 추가"까지 여기서 처리
public class ShopUIController : MonoBehaviour
{
    [Header("Popup Ref")]
    [SerializeField] private ShopPopup shopPopup;//PopupPanel 안의 ShopPopup 연결

    [Header("Inventory UI Ref")]
    [SerializeField] private LobbyInventoryUIController lobbyInventoryUI;//로비 인벤 컨트롤러(강제갱신용)

    [Header("Gold Cost")]
    [SerializeField] private int normalBoxCost = 300;   //일반 상자(군지원 상자)
    [SerializeField] private int highBoxCost = 3000;    //고급 상자(지구방위 보급품)

    private EnumData.BoxType lastRequestType;           //마지막으로 요청한 박스 타입(결과 로그용)
    private bool waitingResult;                         //현재 뽑기 결과를 기다리는 중인지 여부
    private bool subscribed;                            //OnDrawItem 이벤트 구독 여부(중복 구독 방지)

    private void OnEnable()
    {
        //씬 켜질 때 가챠 이벤트 구독
        SubscribeGachaIfPossible();
    }

    private void OnDisable()
    {
        //씬 꺼질 때 구독 해제
        UnsubscribeGacha();
    }

    //군지원 상자 버튼
    public void OnClickNormalBox()
    {
        Debug.Log("//ShopUIController Click Normal Box");

        //골드 차감 + 팝업 오픈 준비
        if (TryPrepareDraw(EnumData.BoxType.Normal, normalBoxCost) == false)
        {
            return;
        }

        //뽑기 실행
        Debug.Log("//ShopUIController DrawItemNormalBox");
        GachaManager.Instance.DrawItemNormalBox();
    }

    //지구방위 보급품 버튼
    public void OnClickHighBox()
    {
        Debug.Log("//ShopUIController Click High Box");

        //골드 차감 + 팝업 오픈 준비
        if (TryPrepareDraw(EnumData.BoxType.High, highBoxCost) == false)
        {
            return;
        }

        //뽑기 실행
        Debug.Log("//ShopUIController DrawItemEpicBox");
        GachaManager.Instance.DrawItemEpicBox();
    }

    //뽑기 실행 전 공통 준비
    //- 참조 체크
    //- 골드 부족 체크/선차감
    //- 팝업 오픈 + 박스 타입 전달
    //- 결과 받을 준비 플래그 세팅
    private bool TryPrepareDraw(EnumData.BoxType type, float cost)
    {
        //필수 참조 방어
        if (shopPopup == null)
        {
            Debug.LogError("//ShopUIController shopPopup == null");
            return false;
        }

        if (UIManager.Instance == null)
        {
            Debug.LogError("//ShopUIController UIManager.Instance == null");
            return false;
        }

        if (GachaManager.Instance == null)
        {
            Debug.LogError("//ShopUIController GachaManager.Instance == null");
            return false;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("//ShopUIController GameManager.Instance == null");
            return false;
        }

        //뽑기 이벤트 구독 보장
        SubscribeGachaIfPossible();

        //골드 부족 체크
        if (GameManager.Instance.gameGold < cost)
        {
            Debug.Log($"//ShopUIController Not enough gold cost:{cost} cur:{GameManager.Instance.gameGold}");
            return false;
        }

        //골드 선차감
        GameManager.Instance.RemoveGold(cost);
        Debug.Log($"//ShopUIController SpendGold -{cost} cur:{GameManager.Instance.gameGold}");

        //상태 저장(결과 받을 준비)
        lastRequestType = type;
        waitingResult = true;

        //팝업이 열릴 때 어떤 박스를 보여줄지 미리 전달
        shopPopup.ShowSelectedBox(type);

        //결과 팝업 열기
        UIManager.Instance.ShowPopup(EnumData.PopupId.Shop);

        return true;
    }

    //뽑기 결과 수신(가챠 매니저 이벤트로 들어옴)
    private void HandleDrawItem(ItemData item)
    {
        //현재 뽑기 요청이 아닐 경우 무시(중복 호출 방어)
        if (!waitingResult) return;

        waitingResult = false;

        if (item == null)
        {
            Debug.LogError("//ShopUIController HandleDrawItem item == null");
            return;
        }

        Debug.Log($"//ShopUIController Draw Result id:{item.id} name:{item.name} tier:{item.tier} box:{lastRequestType}");

        if (PlayerManager.Instance == null)
        {
            Debug.LogError("//ShopUIController PlayerManager.Instance == null");
            return;
        }

        //즉시 인벤토리에 추가(여기서 이미 playerGeneralInven이 바뀌어야 함)
        PlayerManager.Instance.AddItemInven(item);
        Debug.Log("//ShopUIController AddItemInven OK");

        //-중요:인벤 UI를 즉시 다시 그려서 "늦게 보이는" 체감 제거
        if (lobbyInventoryUI != null)
        {
            Debug.Log("//ShopUIController Inventory UI ForceRefresh");
            lobbyInventoryUI.ForceRefresh();
        }
        else
        {
            Debug.LogWarning("//ShopUIController lobbyInventoryUI == null(skip ForceRefresh)");
        }
    }

    //뽑기 이벤트 구독
    private void SubscribeGachaIfPossible()
    {
        if (subscribed) return;

        if (GachaManager.Instance == null)
        {
            Debug.LogWarning("//ShopUIController GachaManager.Instance == null (Subscribe skipped)");
            return;
        }

        //중복 구독 방지
        GachaManager.Instance.OnDrawItem -= HandleDrawItem;
        GachaManager.Instance.OnDrawItem += HandleDrawItem;

        subscribed = true;
        Debug.Log("//ShopUIController Subscribe OnDrawItem");
    }

    private void UnsubscribeGacha()
    {
        if (!subscribed) return;

        if (GachaManager.Instance != null)
        {
            GachaManager.Instance.OnDrawItem -= HandleDrawItem;
        }

        subscribed = false;
        Debug.Log("//ShopUIController Unsubscribe OnDrawItem");
    }
}