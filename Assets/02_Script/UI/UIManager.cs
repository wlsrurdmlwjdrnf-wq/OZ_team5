using System.Collections.Generic;
using UnityEngine;
/*
 UI 전체를 관리하는 매니저
 - Screen(화면): 로비, 배틀HUD, 결과 등 "큰 화면" -> 보통 한 번에 1개만 유지
 - Popup(팝업): 일시정지/설정/보상/레벨업 선택 등 -> Stack으로 쌓고 최상단부터 닫기
 
 프로젝트 권장 원칙
 1) GameManager는 "게임 상태"를 관리(일시정지, 게임오버 등)
 2) UIManager는 "표시"만 담당(팝업 열기/닫기, 화면 전환)
 3) timeScale=0일 때도 UI 애니메이션이 돌게 하려면
    해당 UI Animator의 Update Mode = Unscaled Time 로 설정
*/
public class UIManager : Singleton<UIManager>
{
    [Header("Canvas Layer Roots (Canvas 아래 빈 오브젝트로 만들어서 연결)")]
    [SerializeField] private Transform screenRoot;  // 화면(UI Screen) 붙는 곳
    [SerializeField] private Transform popupRoot;   // 팝업(UIPopup) 붙는 곳
    [SerializeField] private Transform systemRoot;  // 로딩(페이드 인/아웃) 같은 시스템 UI
    [SerializeField] private Transform toastRoot;   // 알림창 UI

    [Header("Default UI Prefabs (원하면 비워두고 외부에서 Show 호출만 해도 됨)")]
    [SerializeField] private UIScreen battleHUDPrefab;      //배틀 HUD Screen프리팹
    [SerializeField] private UIPopup pausePopupPrefab;      //일시정지 Popup프리팹
    [SerializeField] private UIPopup gameOverPopupPrefab;   //게임오버 Popup프리팹
    [SerializeField] private UIPopup gameClearPopupPrefab;  //클리어 Popup프리팹

    //현재 활성화된 Screen(보통 1개만 유지)
    private UIScreen currentScreen;

    //Popup은 여러 개가 뜰 수 있어 Stack으로 관리(최근에 뜬 것부터 닫기)
    private readonly Stack<UIPopup> popupStack = new();

    protected override void Init()
    {
        //_IsDestroyOnLoad가 true면 씬이 바뀌어도 유지됨(UIRoot를 공용으로 쓰려면 true 권장)
        _IsDestroyOnLoad = true;
        base.Init();
    }

    private void OnEnable()
    {
        //GameManager 이벤트 구독(씬 로드 순서 때문에 GameManager가 아직 없을 수 있어 null체크)
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnGameStart += HandleGameStart;    //게임 시작 시 UI 처리
        GameManager.Instance.OnGamePause += HandleGamePause;    //일시정지 시 UI 처리
        GameManager.Instance.OnGameResume += HandleGameResume;  //재개 시 UI 처리
        GameManager.Instance.OnGameOver += HandleGameOver;      //게임오버 시 UI 처리
        GameManager.Instance.OnGameClear += HandleGameClear;    //클리어 시 UI 처리
        GameManager.Instance.OnLevelUp += HandleLevelUp;        //레벨업 시 UI 처리
    }

    private void OnDisable()
    {
        //GameManager 이벤트 구독 해제(메모리 누수/중복 호출 방지)
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnGameStart -= HandleGameStart;
        GameManager.Instance.OnGamePause -= HandleGamePause;
        GameManager.Instance.OnGameResume -= HandleGameResume;
        GameManager.Instance.OnGameOver -= HandleGameOver;
        GameManager.Instance.OnGameClear -= HandleGameClear;
        GameManager.Instance.OnLevelUp -= HandleLevelUp;
    }

    //Screen
    public T ShowScreen<T>(T screenPrefab, object param = null) where T : UIScreen
    {
        //화면 전환 시 남아있는 Popup이 있으면 꼬이기 쉬워서 정리하는게 안전
        CloseAllPopups();

        //기존 Screen 닫기
        if (currentScreen != null)
        {
            currentScreen.OnClose();    //이벤트 해제/정리 작업
            Destroy(currentScreen.gameObject);  //화면 오브젝트 삭제
            currentScreen = null;
        }

        //새 Screen 생성
        T screen = Instantiate(screenPrefab, screenRoot);
        currentScreen = screen;

        //Screen 초기화(데이터 전달이 필요하면 param으로 전달)
        screen.OnOpen(param);

        return screen;
    }

    public void CloseScreen()
    {
        //현재 Screen이 없으면 아무것도 안 함
        if (currentScreen == null) return;

        currentScreen.OnClose();    //정리 작업
        Destroy(currentScreen.gameObject);//오브젝트 삭제
        currentScreen = null;
    }

    //Popup
    public T ShowPopup<T>(T popupPrefab, object param = null) where T : UIPopup
    {
        //팝업 생성 후 Stack에 push
        T popup = Instantiate(popupPrefab, popupRoot);
        popupStack.Push(popup);

        //팝업 초기화(데이터 전달이 필요하면 param으로 전달)
        popup.OnOpen(param);
        return popup;
    }

    public void CloseTopPopup()
    {
        //열려있는 팝업이 없으면 종료
        if (popupStack.Count == 0) return;

        //가장 마지막에 열린 팝업부터 닫기
        UIPopup top = popupStack.Pop();
        top.OnClose();  //정리 작업
        Destroy(top.gameObject);    //오브젝트 삭제
    }

    public void CloseAllPopups()
    {
        //Stack이 빌 때까지 최상단 팝업을 반복해서 닫음
        while (popupStack.Count > 0)
        { 
            CloseTopPopup(); 
        }
    }

    //현재 팝업이 하나라도 떠있는지
    public bool HasPopup => popupStack.Count > 0;

    private void Update()
    {
        //ESC(PC) / Back(모바일) 공통 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //팝업이 있으면 팝업부터 닫음(뒤로가기 UX)
            if (HasPopup)
            {
                CloseTopPopup();
            }
            else
            {
                //팝업이 없으면 게임 일시정지 요청(실제 timeScale=0 처리는 GameManager가 담당)
                if (GameManager.Instance != null && GameManager.Instance.isPlay)
                { 
                    GameManager.Instance.GamePause(); 
                }
            }
        }
    }

    //GameManager Event Handlers
    private void HandleGameStart()
    {
        //게임 시작 시 HUD Screen 표시
        if (battleHUDPrefab != null)
        { 
            ShowScreen(battleHUDPrefab); 
        }
    }

    private void HandleGamePause()
    {
        //게임 일시정지 시 Pause 팝업 표시
        if (pausePopupPrefab == null) return;
        ShowPopup(pausePopupPrefab);
    }

    private void HandleGameResume()
    {
        //게임 재개 시 보통 Pause 팝업 하나만 닫으면 됨(상황에 따라 CloseAllPopups로 바꿔도 됨)
        if (HasPopup)
        { 
            CloseTopPopup(); 
        }
    }

    private void HandleGameOver()
    {
        //게임오버 시 결과 UI 표시(팝업으로 만들었을 때)
        if (gameOverPopupPrefab != null)
        { 
            ShowPopup(gameOverPopupPrefab); 
        }
    }

    private void HandleGameClear()
    {
        //게임클리어 시 결과 UI 표시(팝업으로 만들었을 때)
        if (gameClearPopupPrefab != null)
        { 
            ShowPopup(gameClearPopupPrefab); 
        }
    }

    private void HandleLevelUp()
    {
        //레벨업 시 선택 UI 표시 지점
        //ShowPopup(levelUpPopupPrefab, param);
    }
}
