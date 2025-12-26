using System.Collections.Generic;
using UnityEngine;
using static EnumData;
/*
 UI 전체를 관리하는 매니저
 - Screen(화면): 로비, 배틀HUD, 결과 등 "큰 화면" -> 보통 한 번에 1개만 유지
 - Popup(팝업): 일시정지/설정/보상/레벨업 선택 등 -> Stack으로 쌓고 최상단부터 닫기
 - toast(알림): 짧게 표시되는 알림창

 프로젝트 권장 원칙
 1) GameManager는 "게임 상태"를 관리(일시정지, 게임오버 등)
 2) UIManager는 씬에 배치된 UI를 "어디에 붙이고 어떻게 보여줄지"만 관리
    - UI 생성 책임 없음(Instantiate 안 함)
    - 씬에 배치된 UI 인스턴스를 관리
*/
public class UIManager : MonoBehaviour
{
    //전역 접근용 싱글톤
    public static UIManager Instance { get; private set; }

    [Header("Canvas Layer Roots (Canvas 아래 빈 오브젝트로 만들어서 연결)")]
    [SerializeField] private Transform screenRoot;  // 화면(UI Screen) 붙는 곳
    [SerializeField] private Transform popupRoot;   // 팝업(UIPopup) 붙는 곳
    [SerializeField] private Transform systemRoot;  // 로딩(페이드 인/아웃) 같은 시스템 UI
    [SerializeField] private Transform toastRoot;   // 알림창 UI

    //현재 활성화된 Screen 스택
    private readonly Stack<UIScreen> screenStack = new();

    //현재 열려있는 Popup 스택
    private readonly Stack<UIPopup> popupStack = new();

    //PopupId → UIPopup 매핑 테이블 (씬에 배치된 팝업들을 등록해서 사용)
    private Dictionary<EnumData.PopupId, UIPopup> popupTable = new();

    private void Awake()
    {
        //싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //씬 전환 시에도 UI 유지
        //DontDestroyOnLoad(gameObject);
    }

    //Screen
    //UIScreen 표시
    //clearStack=true : 기존 Screen 전부 제거(씬 전환 시 기본)
    //clearStack=false: 이전 Screen 위에 쌓기(특수 연출용)
    public void ShowScreen(UIScreen screen, bool clearStack = true)
    {
        //null 방어
        if (screen == null) return;

        //Screen은 항상 screenRoot 아래로 이동
        if (screenRoot != null && screen.transform.parent != screenRoot)
        {
            screen.transform.SetParent(screenRoot, false);
        }

        if (clearStack)
        {
            //기존 Screen 전부 숨김
            while (screenStack.Count > 0)
            {
                UIScreen top = screenStack.Pop();
                if (top != null) 
                {
                    top.Hide(); 
                }
            }
        }
        else
        {
            //이전 Screen만 숨김
            if (screenStack.Count > 0)
            {
                screenStack.Peek()?.Hide();
            }
        }

        //새 Screen 등록 및 표시
        screenStack.Push(screen);
        screen.Show();
    }

    //Popup(Register)
    //씬 시작 시 팝업을 UIManager에 등록
    //PopupPanel 아래에 있는 팝업을 한 번에 등록하는 용도
    public void RegisterPopup(UIPopup popup)
    {
        if (popup == null) return;

        EnumData.PopupId id = popup.PopupId;

        //같은 ID가 이미 있으면 덮어씀(씬별 팝업 교체 가능)
        popupTable[id] = popup;

        //시작 시에는 항상 비활성화
        popup.gameObject.SetActive(false);

        //부모를 popupRoot로 통일
        if (popupRoot != null && popup.transform.parent != popupRoot)
        {
            popup.transform.SetParent(popupRoot, false);
        }
    }

    //Popup(Open)
    //PopupId로 팝업 열기(외부에서 호출하는 메인 함수)
    public void ShowPopup(EnumData.PopupId id)
    {
        //등록되지 않은 팝업 방어
        if (!popupTable.TryGetValue(id, out UIPopup popup))
        {
            Debug.LogError($"//등록되지않은팝업:{id}");
            return;
        }
            popup.Open();

        //공통 팝업 표시 로직 호출
        ShowPopupInternal(popup);
    }

    //실제 팝업 표시 처리(내부 공통)
    private void ShowPopupInternal(UIPopup popup)
    {
        if (popup == null) return;

        //스택에 쌓고 표시
        popupStack.Push(popup);
        popup.Open();
    }

    //가장 위에 있는 팝업 닫기
    public void CloseTopPopup()
    {
        if (popupStack.Count == 0) return;

        UIPopup top = popupStack.Pop();
        top?.Close();
    }

    //모든 팝업 닫기
    //씬 전환 직전, 게임 리셋 시 사용
    public void CloseAllPopup()
    {
        while (popupStack.Count > 0)
        {
            UIPopup top = popupStack.Pop();
            top?.Close();
        }
    }

    //System
    //로딩 UI, 페이드 UI를 systemRoot로 이동
    //SceneController에서 주로 호출
    public void AttachSystemUI(MonoBehaviour systemUI)
    {
        if (systemUI == null || systemRoot == null) return;

        systemUI.transform.SetParent(systemRoot, false);
    }

    //Toast UI 표시
    //보통 일정 시간 후 자동으로 꺼짐
    public void ShowToast(MonoBehaviour toastUI)
    {
        if (toastUI == null || toastRoot == null) return;

        toastUI.transform.SetParent(toastRoot, false);
        toastUI.gameObject.SetActive(true);
    }
}