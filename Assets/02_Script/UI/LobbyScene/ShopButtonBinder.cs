using UnityEngine;

//상점 버튼 OnClick에 연결할 함수만 제공
public class ShopButtonBinder : MonoBehaviour
{
    [SerializeField] private ShopPopup shopPopup; //씬의 ShopPopup 연결

    public void OnClickOpenShopPopup()
    {
        if (shopPopup == null) return;

        shopPopup.Open();
    }
}