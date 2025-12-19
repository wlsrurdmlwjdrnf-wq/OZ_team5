using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
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
    [SerializeField] private Transform screenRoot; //화면(UI Screen) 붙는 곳
    [SerializeField] private Transform popupRoot;  //팝업(UIPopup) 붙는 곳
    [SerializeField] private Transform systemRoot; //로딩/페이드 같은 시스템 UI
    [SerializeField] private Transform toastRoot;  //토스트/알림

    [Header("Default UI Prefabs (원하면 비워두고 외부에서 Show 호출만 해도 됨)")]
    [SerializeField] private UIScreen battleHUDPrefab;
    [SerializeField] private UIPopup pausePopupPrefab;
    [SerializeField] private UIPopup gameOverPopupPrefab;  //결과 UI가 Screen이면 Screen으로 바꿔도 됨
    [SerializeField] private UIPopup gameClearPopupPrefab; //동일

    //현재 활성 화면
    private UIScreen currentScreen;

    //팝업은 여러 개 가능 -> Stack
    private readonly Stack<UIPopup> popupStack = new();



}
