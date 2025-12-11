using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum sceneType
{
    TitleScene,
    LobbyScene,
    BattleScene
}


public class SceneLoader : Singleton<SceneLoader>
{

    public void LoadScene(sceneType sct)
    {
        // enum으로 정의된 씬 로드

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
    public AsyncOperation LoadSceneAsync(sceneType sct)
    {
        Time.timeScale = 1f;
        return SceneManager.LoadSceneAsync(sct.ToString());
    }

}
