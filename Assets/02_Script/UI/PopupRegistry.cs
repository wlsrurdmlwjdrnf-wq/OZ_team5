using UnityEngine;

//PopupPanel 아래에 붙여두면
//하위에 있는 모든 UIPopup을 자동으로 UIManager에 등록
public class PopupRegistry : MonoBehaviour
{
    private void Awake()
    {
        //UIManager가 아직 없으면 등록 불가
        if (UIManager.Instance == null) return;

        //비활성화된 팝업까지 전부 검색
        UIPopup[] popups = GetComponentsInChildren<UIPopup>(true);

        for (int i = 0; i < popups.Length; i++)
        {
            UIManager.Instance.RegisterPopup(popups[i]);
        }
    }
}
