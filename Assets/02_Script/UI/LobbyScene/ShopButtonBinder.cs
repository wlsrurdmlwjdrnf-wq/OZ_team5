using UnityEngine;

//상점 버튼 OnClick에 연결할 함수만 제공
public class ShopButtonBinder : MonoBehaviour
{
    public void OnClickOpenShopPopup()
    {
        if (UIManager.Instance == null) return;

        UIManager.Instance.ShowPopup(EnumData.PopupId.Shop);
    }
}
