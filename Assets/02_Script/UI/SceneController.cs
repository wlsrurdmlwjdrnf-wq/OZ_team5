using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    //로딩 UI 전체 패널(Canvas > Layer_System에 두는걸 권장)
    [SerializeField] private GameObject loadingPanel;
    //로딩 진행도 슬라이더(0~1)
    [SerializeField] private Slider loadingBar;
    //로딩 퍼센트 텍스트
    [SerializeField] private TextMeshProUGUI loadingText;
    //"클릭해서 계속" 안내 UI(타이틀용, 선택)
    [SerializeField] private GameObject pressToContinue;

    //현재 비동기 씬 로딩 작업
    private AsyncOperation asyncOp;
    //로딩 완료 후 입력 대기 여부
    private bool waitForInput;

    private void Awake()
    {
        //시작 시 로딩 UI는 꺼둠
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }

        if (pressToContinue != null)
        {
            pressToContinue.SetActive(false);
        }
    }

    private void Update()
    {
        //로딩 완료 후 클릭 대기 처리
        if (!waitForInput) return;

        if (Input.GetMouseButtonDown(0))
        {
            waitForInput = false;

            //씬 활성화 허용
            if (asyncOp != null)
            {
                asyncOp.allowSceneActivation = true;
            }
        }
    }

    //일반적인 씬 이동(로비→배틀, 배틀→로비)
    public void LoadScene(EnumData.sceneType targetScene)
    {
        StartCoroutine(CoLoadScene(targetScene, false));
    }

    //로딩 완료 후 입력을 기다렸다가 씬 이동(타이틀용)
    public void LoadSceneWaitInput(EnumData.sceneType targetScene)
    {
        StartCoroutine(CoLoadScene(targetScene, true));
    }

    //씬을 비동기로 로드하는 코루틴
    private IEnumerator CoLoadScene(EnumData.sceneType targetScene, bool requireInput)
    {
        //이전 로딩 상태 초기화
        asyncOp = null;
        waitForInput = false;

        //게임이 멈춰있을 수 있으므로 리셋
        Time.timeScale = 1f;

        //로딩 UI 표시(null이어도 터지지 않게 방어)
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }
        if (pressToContinue != null)
        {
            pressToContinue.SetActive(false);
        }

        UpdateProgress(0f);

        //SceneLoader가 없으면 씬 로드 불가
        if (SceneLoader.Instance == null)
        {
            Debug.LogError("//SceneLoader.Instance가null임(SceneLoader오브젝트가씬에있는지확인)");
            yield break;
        }

        //실제 씬 로드는 SceneLoader에게 위임
        asyncOp = SceneLoader.Instance.LoadSceneAsync(targetScene);

        //씬 로드 실패 방어
        if (asyncOp == null)
        {
            Debug.LogError("//LoadSceneAsync가null반환(BuildSettings또는씬이름확인)");
            yield break;
        }

        //자동 씬 전환 차단(연출 제어용)
        asyncOp.allowSceneActivation = false;

        //로딩 진행 루프
        while (!asyncOp.isDone)
        {
            //AsyncOperation.progress는 0~0.9까지만 증가
            float progress = Mathf.Clamp01(asyncOp.progress / 0.9f);
            UpdateProgress(progress);

            //실질적 로딩 완료 시점
            if (asyncOp.progress >= 0.9f)
            {
                UpdateProgress(1f);

                if (requireInput)
                {
                    //입력 대기 모드
                    waitForInput = true;

                    if (pressToContinue != null)
                    {
                        pressToContinue.SetActive(true);
                    }

                    //입력 들어올 때까지 대기
                    while (waitForInput)
                        yield return null;
                }
                    //즉시 씬 전환
                else
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
        if (pressToContinue != null)
        {
            pressToContinue.SetActive(false);
        }

        asyncOp = null;
    }

    //로딩 진행도 UI 갱신
    private void UpdateProgress(float value01)
    {
        if (loadingBar != null)
        {
            loadingBar.value = value01;
        }

        if (loadingText != null)
        {
            int percent = Mathf.RoundToInt(value01 * 100f);
            loadingText.text = $"{percent}%";
        }
    }
}
