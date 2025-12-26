using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : Singleton<SceneLoader>
{
    protected override void Init()
    {
        _IsDestroyOnLoad = true; //씬 넘어가도 유지
        base.Init();
    }

    public void LoadScene(EnumData.sceneType sct)
    {
        // enum으로 정의된 씬 로드
        // 씬 이동시 호출하시면 됩니다

        Time.timeScale = 1f;
        SceneManager.LoadScene(sct.ToString()); 
    }

    public void ReloadScene()
    {
        // 해당 씬 리로드

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // 비동기 로드 함수
    public AsyncOperation LoadSceneAsync(EnumData.sceneType sct)
    {
        Time.timeScale = 1f;
        return SceneManager.LoadSceneAsync(sct.ToString());
    }

}
