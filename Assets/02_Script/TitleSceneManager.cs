using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    //게임시작시 재생될 로고 애니메이션
    public Animation logoAnim;
    //시작로고 텍스트
    public TextMeshProUGUI logoText;

    //타이틀로 사용할 게임오브젝트
    public GameObject title;
    //로딩표시 슬라이더
    public Slider loadingBar;
    //퍼센트 텍스트
    public TextMeshProUGUI loadingText;

    private AsyncOperation _AsyncOperation;

    private void Awake()
    {
        //로고 애니메이션 실행 및 타이틀 비활성화
        logoAnim.gameObject.SetActive(true);
        title.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(LoadGame());    
    }

    private IEnumerator LoadGame()
    {
        //로고 애니메이션 재생후 길이만큼 대기
        logoAnim.Play();
        yield return new WaitForSeconds(logoAnim.clip.length);


        //로고 애니메이션 비활성화 및 타이틀 활성화
        logoAnim.gameObject.SetActive(false);
        title.SetActive(true);

        //로비로 전환되는 씬 로더 인스턴스 저장
        _AsyncOperation = SceneLoader.Instance.LoadSceneAsync(sceneType.Lobby);

        if(_AsyncOperation == null)
        {
            yield break;
        }

        //로딩바 구현을 위해 씬 전환 false (true시 씬 전환)
        _AsyncOperation.allowSceneActivation = false;

        //시작 로딩 50%
        loadingBar.value = 0.5f;
        loadingText.text = $"{(int)loadingBar.value * 100}%";
        yield return new WaitForSeconds(0.5f);
        
        //로딩 완료시까지 반복 (90%에서 정지)
        while(!_AsyncOperation.isDone)
        {
            //로딩바 게이지를 50%다 작으면 50%설정 아니면 로딩된만큼 설정
            loadingBar.value = _AsyncOperation.progress < 0.5f ? 0.5f : _AsyncOperation.progress;
            //동기화 된 만큼 로딩바 게이지 실시간 변화
            loadingText.text = $"{(int)loadingBar.value * 100}%";

            //로딩작업이 끝나면 씬 전환
            //추가 로직 필요시 수정
            if(_AsyncOperation.progress >= 0.9f)
            {                
                _AsyncOperation.allowSceneActivation = true;
                yield break;
            }
            yield return null;
        }
    }
}
