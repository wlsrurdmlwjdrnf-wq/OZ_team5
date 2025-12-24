using UnityEngine;

//배틀 씬의 시작/종료 흐름을 담당
// - 씬 진입 시 GameManager에게 "게임 시작"을 알림
public class BattleSceneManager : MonoBehaviour
{
    private void Start()
    {
        //배틀 씬에 GameManager가 반드시 있어야 함
        if (GameManager.Instance == null)
        {
            Debug.LogError("//GameManager.Instance가null임(배틀씬에GameManager오브젝트있는지확인)");
            return;
        }

        //게임 시작 선언
        GameManager.Instance.GameStart();
    }
}
