using UnityEngine;

//상점 버튼 OnClick에 연결할 함수만 제공
public class ShopButtonBinder : MonoBehaviour
{
    public void OnClickOpenShopPopup()
    {
        Debug.Log("//상점 팝업 열기 버튼 클릭");

        if (UIManager.Instance == null)
        {
            Debug.LogError("//UIManager.Instance 없음");
            return;
        }

        UIManager.Instance.ShowPopup(EnumData.PopupId.Shop);
    }
}
