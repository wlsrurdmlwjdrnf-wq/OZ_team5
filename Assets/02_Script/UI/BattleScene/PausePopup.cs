using TMPro;
using UnityEngine;

//일시정지 팝업
// - UIPopup을 상속해야 UIManager/PopupRegistry가 인식함
public class PausePopup : UIPopup
{
    [Header("Sound Icons")]
    [SerializeField] private GameObject soundOnIcon;  //사운드 ON 아이콘(또는 이미지 오브젝트)
    [SerializeField] private GameObject soundOffIcon; //사운드 OFF 아이콘(또는 이미지 오브젝트)

    //팝업이 열릴 때마다 호출
    protected override void OnOpen()
    {
        RefreshSoundIcons();
    }

    //팝업이 닫힐 때 호출
    protected override void OnClose()
    {
        //필요하면 정리(현재는 없음)
    }
    //사운드 상태에 맞게 아이콘 표시
    private void RefreshSoundIcons()
    {
        bool isMuted = AudioListener.pause;

        if (soundOnIcon != null)
        {
            soundOnIcon.SetActive(!isMuted);
        }

        if (soundOffIcon != null)
        {
            soundOffIcon.SetActive(isMuted);
        }
    }
    //로비로 나가기 요청만
    public void OnClickLobby()
    {
        BattleUIController ui = FindObjectOfType<BattleUIController>();
        if (ui != null)
        {
            AudioManager.Instance.PlaySFX(EnumData.SFX.Button1SFX);
            ui.GoLobby();
        }
    }
    
    //소리 ON/OFF 버튼
    public void OnClickSoundToggle()
    {
        AudioManager.Instance.PlaySFX(EnumData.SFX.Button1SFX);
        //전체 사운드 일괄 음소거 토글
        //AudioListener.pause = !AudioListener.pause;

        //아이콘 갱신
        //RefreshSoundIcons();
    }
}
