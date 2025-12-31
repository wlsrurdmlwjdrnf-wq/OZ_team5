using UnityEngine;
using static EnumData;

/* 배틀 씬에서 "어떤 UI를 언제 보여줄지" 결정하는 컨트롤러
 - GameManager 이벤트를 구독해서 UI 반응 처리
 - UIManager는 표시/스택 관리만 담당(결정은 여기서)
 - 팝업은 PopupId(enum)로 호출(직접 참조 최소화)
*/
public class BattleUIController : MonoBehaviour
{
    private void OnEnable()
    {
        //GameManager 이벤트 구독(Enable/Disable에서 하는게 씬 전환에 안전)
        if (GameManager.Instance == null)
        {
            Debug.LogError("//GameManager.Instance가 배틀씬에 없음");
            return;
        }

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

    //게임매니저 Event Handlers
    //게임 시작 시(보통 배틀 시작)
    private void HandleGameStart()
    {
        //상시 HUD는 이미 켜져 있으므로
        //여기서는 추가 UI 연출이 필요할 때만 처리
        //예: Start 카운트다운, 튜토리얼 표시 등
    }

    //게임 일시정지시 호출
    private void HandleGamePause()
    {
        if (UIManager.Instance == null)
        {
            Debug.LogError("//UIManager.Instance가 없음(UIRoot에UIManager있는지확인)");
            return;
        }

        //Pause 팝업 표시
        UIManager.Instance.ShowPopup(EnumData.PopupId.Pause);
    }

    //게임 재개시 호출
    private void HandleGameResume()
    {
        //Pause 팝업 닫기
        if (UIManager.Instance == null) return;

        UIManager.Instance.CloseTopPopup();
    }

    //게임오버시 호출
    private void HandleGameOver()
    {
        if (UIManager.Instance == null) return;

        //게임오버 연출/팝업
        UIManager.Instance.ShowPopup(EnumData.PopupId.GameOver);
    }

    //게임 클리어시 호출
    private void HandleGameClear()
    {
        if (UIManager.Instance == null) return;

        //클리어 팝업
        UIManager.Instance.ShowPopup(EnumData.PopupId.GameClear);
    }

    //레벨업시 호출
    private void HandleLevelUp()
    {
        //레벨업은 게임을 멈추고 선택 UI를 띄움
        if (UIManager.Instance == null) return;

        UIManager.Instance.ShowPopup(EnumData.PopupId.LevelUp);
    }

    //보스 스폰 시
    private void HandleBossSpawn()
    {
        //여기서 토스트/경고 팝업/상단 배너 등으로 처리 가능
        //UIManager.Instance.ShowToast(...) 같은 흐름 추천
    }

    //HUD의 일시정지 버튼에서 직접 호출할 수 있는 함수
    public void OnClickPauseButton()
    {
        //UI 입력이 버튼에서 직접 들어옴
        //GameManager.GamePause()를 호출해서 이벤트 흐름을 통일
        if (GameManager.Instance == null || Time.timeScale == 0) return;
        //게임 상태 변경은 GameManager가 담당
        GameManager.Instance.GamePause();
    }
    //Resume Button에서 호출
    public void OnClickResumeGame()
    {
        //게임 재개
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameResume();
        }

        //열려있는 팝업 정리
        if (UIManager.Instance != null)
        {
            UIManager.Instance.CloseAllPopup();
        }
    }
    //BattleUIController에 로비 이동함수만 있음
    public void GoLobby()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.CloseAllPopup();
        }

        Time.timeScale = 1f;

        SceneController sceneController = FindObjectOfType<SceneController>();
        if (sceneController == null)
        {
            Debug.LogError("//SceneController가씬에없음");
            return;
        }

        sceneController.LoadScene(EnumData.sceneType.LobbyScene);
    }
}
