using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    [Header("Loading UI References (씬에 배치된 오브젝트 연결)")]
    [SerializeField] private GameObject loadingPanel;           //로딩 UI 전체 패널    
    [SerializeField] private Slider loadingBar;                 //로딩 진행도 슬라이더(0~1)
    [SerializeField] private TextMeshProUGUI loadingText;       //로딩 퍼센트 텍스트
    [SerializeField] private float minLoadingShowTime = 0.2f;   //로딩 UI가 너무 빨리 꺼졌다 켜지는걸 방지
    public event Action OnLoadLobbyScene; 
    private AsyncOperation asyncOp; //현재 비동기 씬 로딩 작업
    private bool isLoading;         //중복 로드 방지용

    private void Awake()
    {
        //시작 시 로딩 UI는 꺼둠
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }

        //싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    //버튼에서 이 함수만 호출하면 됨(타이틀 -> 로비 <-> 배틀 전부)
    public void LoadScene(EnumData.sceneType targetScene)
    {
        //로딩중에 또 누르면 중복 실행될 수 있어서 방어
        if (isLoading) return;
        GameManager.Instance.ResetGamePlayTime();
        OnLoadLobbyScene?.Invoke();
        StartCoroutine(CoLoadSceneAuto(targetScene));
    }

    //버튼 전용(무조건 OnClick에 뜸)
    public void LoadLobbyScene()
    {
        LoadScene(EnumData.sceneType.LobbyScene);
    }
    public void LoadBattleScene()
    {
        LoadScene(EnumData.sceneType.BattleScene);
    }

    //로딩이 끝나면 자동으로 씬을 넘기는 코루틴
    private IEnumerator CoLoadSceneAuto(EnumData.sceneType targetScene)
    {
        isLoading = true;
        asyncOp = null;
        Time.timeScale = 1f;
        //로딩 UI 표시
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }

        //슬라이더 초기화
        UpdateProgress(0f);

        //SceneLoader가 없으면 씬 로드 불가
        if (SceneLoader.Instance == null)
        {
            Debug.LogError("//SceneLoader.Instance가 null임(SceneLoader오브젝트가 씬에 있는지 확인)");
            isLoading = false;
            yield break;
        }

        //실제 씬 로드는 SceneLoader에게 위임
        asyncOp = SceneLoader.Instance.LoadSceneAsync(targetScene);

        //씬 로드 실패 방어
        if (asyncOp == null)
        {
            Debug.LogError("//LoadSceneAsync가 null반환(BuildSettings 또는 씬 이름 확인)");
            isLoading = false;
            yield break;
        }

        //씬 전환을 우리가 제어하기 위해 막아둠
        asyncOp.allowSceneActivation = false;

        //로딩 UI 최소 표시 시간(너무 빠르면 깜빡임이 생겨서)
        float elapsed = 0f;

        while (!asyncOp.isDone)
        {
            elapsed += Time.unscaledDeltaTime;

            //AsyncOperation.progress는 0~0.9까지만 증가(0.9가 로딩 완료 구간)
            float progress = Mathf.Clamp01(asyncOp.progress / 0.9f);
            UpdateProgress(progress);

            //로딩 완료 구간 도달 시
            if (asyncOp.progress >= 0.9f)
            {
                //UI상 100%로 고정
                UpdateProgress(1f);

                //최소 표시 시간이 지나면 자동으로 씬 활성화 허용
                if (elapsed >= minLoadingShowTime)
                {
                    asyncOp.allowSceneActivation = true;
                }
            }

            yield return null;
        }

        //씬 전환 완료 후 로딩 UI 정리
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }

        asyncOp = null;
        isLoading = false;
    }

    //로딩 진행도 UI 갱신
    private void ShowLoadingUI()
    {
        //로딩 패널 켜기
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }

        if (loadingText != null)
        {
            loadingText.gameObject.SetActive(true);
        }

        //슬라이더 초기화
        UpdateProgress(0f);
    }

    //진행도 표시(슬라이더만 갱신)
    private void UpdateProgress(float value01)
    {
        if (loadingBar != null) loadingBar.value = value01;
    }

}
