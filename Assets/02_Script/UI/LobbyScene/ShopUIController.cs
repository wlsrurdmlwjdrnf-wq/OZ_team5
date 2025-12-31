using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//로비 상점 화면(UI Screen) 컨트롤러
//- 군지원/지구방위 상자 버튼 입력 처리
//- 선택한 박스 타입을 ShopPopup에 전달한다
//- 결과 팝업(ShopPopup)을 열고 가챠 실행
//- 결과 표시 책임은 ShopPopup이 담당
public class ShopUIController : MonoBehaviour
{
    [SerializeField] private ShopPopup shopPopup; //PopupPanel 안의 ShopPopup을 드래그로 연결

    //군지원 상자 버튼에서 호출
    public void OnClickNormalBox()
    {
        Debug.Log("//ShopUIController Click Normal Box");

        if (shopPopup == null)
        {
            Debug.LogError("//ShopUIController shopPopup == null");
            return;
        }

        if (UIManager.Instance == null)
        {
            Debug.LogError("//ShopUIController UIManager.Instance == null");
            return;
        }

        if (GachaManager.Instance == null)
        {
            Debug.LogError("//ShopUIController GachaManager.Instance == null");
            return;
        }

        //팝업이 열릴 때 ShowBox에서 사용할 타입을 먼저 세팅
        shopPopup.ShowSelectedBox(EnumData.BoxType.Normal);

        //결과 팝업 열기
        UIManager.Instance.ShowPopup(EnumData.PopupId.Shop);

        //가챠 실행(팝업이 열린 다음 호출하는게 안전)
        Debug.Log("//ShopUIController DrawItemNormalBox");
        GachaManager.Instance.DrawItemNormalBox();
    }

    //지구방위 보급품 버튼에서 호출
    public void OnClickHighBox()
    {
        Debug.Log("//ShopUIController Click High Box");

        if (shopPopup == null)
        {
            Debug.LogError("//ShopUIController shopPopup == null");
            return;
        }

        if (UIManager.Instance == null)
        {
            Debug.LogError("//ShopUIController UIManager.Instance == null");
            return;
        }

        if (GachaManager.Instance == null)
        {
            Debug.LogError("//ShopUIController GachaManager.Instance == null");
            return;
        }

        //팝업이 열릴 때 ShowBox에서 사용할 타입을 먼저 세팅
        shopPopup.ShowSelectedBox(EnumData.BoxType.High);

        //결과 팝업 열기
        UIManager.Instance.ShowPopup(EnumData.PopupId.Shop);

        //가챠 실행(팝업이 열린 다음 호출하는게 안전)
        Debug.Log("//ShopUIController DrawItemEpicBox");
        GachaManager.Instance.DrawItemEpicBox();
    }
}