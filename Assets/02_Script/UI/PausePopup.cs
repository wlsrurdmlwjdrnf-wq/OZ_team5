using UnityEngine;

//일시정지 팝업
// - UIPopup을 상속해야 UIManager/PopupRegistry가 인식함
public class PausePopup : UIPopup
{
    //팝업이 처음 열릴 때 한 번만 호출
    protected override void OnInit()
    {
        //초기 세팅이 필요하면 여기
        //예: 버튼 이벤트 연결
    }

    //팝업이 열릴 때마다 호출
    protected override void OnOpen()
    {
        //일시정지 연출, 사운드 등
    }

    //팝업이 닫힐 때 호출
    protected override void OnClose()
    {
        //필요하면 정리
    }

    //Button Hooks
    //계속하기 버튼
    public void OnClickResume()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.GameResume();
    }

    //로비로 나가기 버튼
    public void OnClickHome()
    {
        if (GameManager.Instance == null) return;

        //게임 재개 상태로 돌려놓고 로비 이동
        Time.timeScale = 1f;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.CloseAllPopup();
        }

        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadScene(EnumData.sceneType.LobbyScene);
        }
    }
}

