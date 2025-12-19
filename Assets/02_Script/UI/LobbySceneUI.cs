using UnityEngine;

public class LobbySceneUI : MonoBehaviour
{
    //씬에 있는 SceneController 직접 참조
    [SerializeField] private SceneController sceneController;

    public void OnClickGoBattle()
    {
        if (sceneController == null)
        {
            Debug.LogError("//SceneController참조가null임(인스펙터에서연결필요)");
            return;
        }

        sceneController.LoadScene(EnumData.sceneType.BattleScene);
    }
}
