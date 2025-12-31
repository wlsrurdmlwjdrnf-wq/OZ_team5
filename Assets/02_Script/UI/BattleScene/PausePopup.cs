using TMPro;
using UnityEngine;

//일시정지 팝업
// - UIPopup을 상속해야 UIManager/PopupRegistry가 인식함
public class PausePopup : UIPopup
{
    [Header("Sound Icons")]
    [SerializeField] private GameObject soundOnIcon;  //사운드 ON 아이콘(또는 이미지 오브젝트)
    [SerializeField] private GameObject soundOffIcon; //사운드 OFF 아이콘(또는 이미지 오브젝트)
    [SerializeField] bool isMute = false;
    //팝업이 열릴 때마다 호출
    protected override void OnOpen()
    {
        //RefreshSoundIcons();
    }

    //팝업이 닫힐 때 호출
    protected override void OnClose()
    {
        //필요하면 정리(현재는 없음)
    }
    //사운드 상태에 맞게 아이콘 표시
    private void RefreshSoundIcons()
    {

        if (isMute)
        {
            soundOnIcon.SetActive(true);
            soundOffIcon.SetActive(false);
            AudioListener.volume = 0.2f;
            isMute = false;
        }
        else
        {
            soundOnIcon.SetActive(false);
            soundOffIcon.SetActive(true);
            AudioListener.volume = 0;
            isMute = true;
        }
    }
    //로비로 나가기 요청만
    /*
    public void OnClickLobby()
    {
        BattleUIController ui = FindObjectOfType<BattleUIController>();
        if (ui != null)
        {
            ui.GoLobby();
        }
    }
    */
    //소리 ON/OFF 버튼
    public void OnClickSoundToggle()
    {
        RefreshSoundIcons();
    }
}
