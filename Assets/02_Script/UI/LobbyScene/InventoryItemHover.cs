using UnityEngine;
using UnityEngine.EventSystems;

//인벤토리 슬롯(버튼)에 붙임
//- 마우스가 올라가면 ItemData를 가져와서 툴팁 표시
//- 마우스가 나가면 툴팁 숨김
public class InventoryItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int itemId;        //슬롯이 들고 있는 아이템 ID(테이블 조회용)
    [SerializeField] private ItemPopup tooltip; //씬에 있는 ItemPopup 연결

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("//OnPointerEnter called");

        if (tooltip == null)
        {
            Debug.LogError("//tooltip이 null임(인스펙터 연결 확인)");
            return;
        }

        //여기서 DataManager로부터 ItemData를 가져와야 함(동료 테이블 방식에 맞게 수정)
        //예시 형태만 만들어둠
        ItemData data = DataManager.Instance.GetItemData(itemId);
        if (data == null)
        {
            Debug.LogError($"//ItemData null: id={itemId}");
            return;
        }

        Debug.Log($"//Tooltip Show id:{itemId} name:{data.name}");
        tooltip.Show(data, Input.mousePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.Hide();
        }
    }
}
