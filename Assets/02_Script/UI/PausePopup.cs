using TMPro;
using UnityEngine;

//일시정지 팝업
// - UIPopup을 상속해야 UIManager/PopupRegistry가 인식함
public class PausePopup : UIPopup
{
    [Header("Result Texts")]
    [SerializeField] private TextMeshProUGUI goldText; //코인 표시
    [SerializeField] private TextMeshProUGUI killText; //처치 수 표시

    [Header("Sound Icons")]
    [SerializeField] private GameObject soundOnIcon;  //사운드 ON 아이콘(또는 이미지 오브젝트)
    [SerializeField] private GameObject soundOffIcon; //사운드 OFF 아이콘(또는 이미지 오브젝트)

    private BattleHUDNumbers hud;   //처치 수 가져오기
    private PlayerData playerData;  //코인 가져오기

    //팝업이 처음 열릴 때 한 번만 호출
    protected override void OnInit()
    {
        //1회 캐싱(없으면 OnOpen에서 다시 찾음)
        hud = FindObjectOfType<BattleHUDNumbers>();

        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            playerData = player.PlayerStat();
        }
    }

    //팝업이 열릴 때마다 호출
    protected override void OnOpen()
    {
        RefreshResult();
        RefreshSoundIcons();
    }

    //팝업이 닫힐 때 호출
    protected override void OnClose()
    {
        //필요하면 정리(현재는 없음)
    }

    //현재 코인/처치 수 UI 갱신
    private void RefreshResult()
    {
        if (hud == null)
        {
            hud = FindObjectOfType<BattleHUDNumbers>();
        }

        if (playerData == null)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                playerData = player.PlayerStat();
            }
        }
        //골드
        int gold = 0;
        if (playerData != null)
        {
            gold = (int)playerData.playerGold;
        }

        if (goldText != null)
        {
            goldText.text = $"{gold}";
        }
        //킬수
        int killCount = 0;
        if (hud != null)
        {
            killCount = hud.GetKillCount();
        }

        if (killText != null)
        {
            killText.text = $"{killCount}";
        }
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

    //계속하기 버튼
    public void OnClickResume()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.GameResume();
    }

    //로비로 나가기 요청만
    public void OnClickHome()
    {
        BattleUIController ui = FindObjectOfType<BattleUIController>();
        if (ui != null)
        {
            ui.GoLobby();
        }
    }
    
    //소리 ON/OFF 버튼
    public void OnClickSoundToggle()
    {
        //전체 사운드 일괄 음소거 토글
        //AudioListener.pause = !AudioListener.pause;

        //아이콘 갱신
        //RefreshSoundIcons();
    }
}
