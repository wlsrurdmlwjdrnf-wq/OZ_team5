using UnityEngine;

//로비 씬에서 "어떤 UI를 언제 보여줄지"를 결정하는 상위 UI 컨트롤러
//-인벤, 상점, 설정 등 하위 UI 컨트롤러들의 요청을 받아 팝업을 열고 닫는 책임을 가짐
//-UIManager는 단순히 "보여주기/스택 관리"만 하고, 판단은 여기서 한다
public class LobbyUIController : MonoBehaviour
{
    [Header("Refs")]
    //로비 인벤토리 UI 컨트롤러
    //-슬롯 클릭 시 팝업을 직접 열지 않고, 이벤트로 요청만 보냄
    [SerializeField] private LobbyInventoryUIController inventoryUI;

    //아이템 상세 팝업 인스턴스
    //-씬에 미리 배치된 InventoryItemPopup
    //-Bind로 데이터만 채우고, Open은 여기서 호출
    [SerializeField] private InventoryItemPopup itemPopup;

    private void OnEnable()
    {
        //인벤 컨트롤러가 없으면 팝업 요청을 받을 수 없음
        if (inventoryUI == null)
        {
            Debug.LogError("//LobbyUIController inventoryUI == null");
            return;
        }

        //인벤 UI에서 발생하는 "아이템 팝업 요청" 이벤트 구독
        inventoryUI.OnRequestItemPopup += HandleRequestItemPopup;

        Debug.Log("//LobbyUIController Subscribe Inventory Popup Request");
    }

    private void OnDisable()
    {
        //씬 전환/비활성화시 반드시 구독 해제
        if (inventoryUI == null) return;

        inventoryUI.OnRequestItemPopup -= HandleRequestItemPopup;

        Debug.Log("//LobbyUIController Unsubscribe Inventory Popup Request");
    }

    //인벤 UI에서 팝업 요청이 들어왔을 때 호출되는 함수
    //-여기서 실제로 팝업을 열지 말지 판단
    private void HandleRequestItemPopup(ItemData item, int slot, bool isEquipMode)
    {
        //요청 데이터 방어
        if (item == null) return;

        if (itemPopup == null)
        {
            Debug.LogError("//LobbyUIController itemPopup == null");
            return;
        }

        //팝업에 아이템 데이터 바인딩 (isEquipMode)
        //-true : 가방 슬롯 클릭(장착 가능)
        //-false: 장착 슬롯 클릭(해제 가능)
        itemPopup.Bind(item, slot, isEquipMode);

        //UIManager를 통해 팝업을 연다
        //-스택 관리와 Open 호출은 UIManager가 담당
        if (UIManager.Instance != null)
        {
            Debug.Log($"//LobbyUIController Show ItemPopup slot:{slot} id:{item.id} equipMode:{isEquipMode}");
            UIManager.Instance.ShowPopup(EnumData.PopupId.Item);
        }
        else
        {
            //비상용 처리(UIManager가 없는 경우)
            Debug.LogError("//LobbyUIController UIManager.Instance == null");
            //itemPopup.Open();
        }
    }
}
