using UnityEngine;
using static EnumData;

/* 배틀 씬에서 "어떤 UI를 언제 보여줄지" 결정하는 컨트롤러
 - GameManager 이벤트를 구독해서 UI 반응 처리
 - UIManager는 표시/스택 관리만 담당(결정은 여기서)
 - 팝업은 PopupId(enum)로 호출(직접 참조 최소화)
*/
public class BattleUIController : MonoBehaviour
{
    [Header("Default Screen Instance (씬에 배치된 UI 참조)")]
    //배틀 HUD는 배틀 씬마다 다를 수 있어서 씬 인스턴스를 참조로 둠
    //Popup은 Registry에서 관리하므로 HUD만 직접 들고가도 됨(취향)
    [SerializeField] private UIScreen battleHUD;

    [Header("Options")]
    //배틀 씬 시작 시 HUD를 자동으로 보여줄지
    [SerializeField] private bool showHudOnStart = true;

    //Unity
    private void Awake()
    {
        //씬에 배치된 UI라면 시작은 꺼두는게 안전
        //UIManager.ShowScreen에서 켜줌
        if (battleHUD != null)
            battleHUD.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //GameManager 이벤트 구독(Enable/Disable에서 하는게 씬 전환에 안전)
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnGameStart += HandleGameStart;
        GameManager.Instance.OnGamePause += HandleGamePause;
        GameManager.Instance.OnGameResume += HandleGameResume;
        GameManager.Instance.OnGameOver += HandleGameOver;
        GameManager.Instance.OnGameClear += HandleGameClear;
        GameManager.Instance.OnLevelUp += HandleLevelUp;
        GameManager.Instance.OnBossSpawn += HandleBossSpawn;
    }

    private void OnDisable()
    {
        //해제는 반드시 필요(중복 구독/메모리 누수/씬 전환 버그 방지)
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnGameStart -= HandleGameStart;
        GameManager.Instance.OnGamePause -= HandleGamePause;
        GameManager.Instance.OnGameResume -= HandleGameResume;
        GameManager.Instance.OnGameOver -= HandleGameOver;
        GameManager.Instance.OnGameClear -= HandleGameClear;
        GameManager.Instance.OnLevelUp -= HandleLevelUp;
        GameManager.Instance.OnBossSpawn -= HandleBossSpawn;
    }

    private void Start()
    {
        //씬 진입 즉시 HUD를 띄우고 싶으면 사용
        //만약 GameManager.GameStart() 타이밍에 맞춰 띄우고 싶다면 false로 두고 HandleGameStart에서만 띄워도 됨
        if (!showHudOnStart) return;

        if (UIManager.Instance == null) return;
        if (battleHUD == null) return;

        //배틀 화면은 보통 단일 Screen으로 관리
        UIManager.Instance.ShowScreen(battleHUD, true);
    }

    //게임매니저 Event Handlers

    //게임 시작 시(보통 배틀 시작)
    private void HandleGameStart()
    {
        //HUD 표시(게임 시작 순간에만 HUD를 띄우고 싶으면 여기서만 처리)
        if (UIManager.Instance == null) return;
        if (battleHUD == null) return;

        UIManager.Instance.ShowScreen(battleHUD, true);

        //배틀 시작 시 혹시 남아있을 수 있는 팝업 정리(안전)
        UIManager.Instance.CloseAllPopup();
    }

    //일시정지 시
    private void HandleGamePause()
    {
        if (UIManager.Instance == null) return;

        //Pause 팝업 표시(등록되어 있어야 함)
        UIManager.Instance.ShowPopup(PopupId.Pause);
    }

    //재개 시
    private void HandleGameResume()
    {
        if (UIManager.Instance == null) return;

        //보통은 Pause 팝업이 최상단이므로 top을 닫아도 됨
        UIManager.Instance.CloseTopPopup();
    }

    //게임오버 시
    private void HandleGameOver()
    {
        if (UIManager.Instance == null) return;

        //게임오버 연출/팝업
        UIManager.Instance.ShowPopup(PopupId.GameOver);
    }

    //클리어 시
    private void HandleGameClear()
    {
        if (UIManager.Instance == null) return;

        //클리어 팝업
        UIManager.Instance.ShowPopup(PopupId.GameClear);
    }

    //레벨업 시
    private void HandleLevelUp()
    {
        //레벨업은 보통 timeScale=0으로 멈추고 선택 UI를 띄우는 경우가 많음
        //프로젝트 룰에 맞게 Confirm/LevelUp 팝업 중 하나로 연결하면 됨
        if (UIManager.Instance == null) return;

        UIManager.Instance.ShowPopup(PopupId.LevelUp);
    }

    //보스 스폰 시(선택)
    private void HandleBossSpawn()
    {
        //여기서 토스트/경고 팝업/상단 배너 등으로 처리 가능
        //UIManager.Instance.ShowToast(...) 같은 흐름 추천
    }

    //UI Button Hooks(선택)

    //HUD의 일시정지 버튼에서 직접 호출할 수 있는 함수
    public void OnClickPauseButton()
    {
        //UI 입력이 버튼에서 직접 들어오는 경우
        //GameManager.GamePause()를 호출해서 이벤트 흐름을 통일하는게 좋음
        if (GameManager.Instance == null) return;

        GameManager.Instance.GamePause();
    }

    //게임오버/클리어 팝업에서 "로비로" 버튼 누르면 호출(예시)
    public void OnClickGoLobby()
    {
        //로비<->배틀 전환은 SceneController가 담당한다는 네 룰에 맞춤
        if (UIManager.Instance != null)
            UIManager.Instance.CloseAllPopup();

        //SceneController가 DontDestroy로 존재한다는 전제
        if (SceneController.FindObjectOfType<SceneController>() == null)
        {
            //만약 씬 어디에도 없다면 에러 로그
            Debug.LogError("//SceneController가씬에없음");
            return;
        }

        //주의: FindObjectOfType은 비용이 있으니 실제 프로젝트에선 참조 캐싱 추천
        FindObjectOfType<SceneController>().LoadScene(EnumData.sceneType.LobbyScene);
    }
}
