using UnityEngine;

public class BattleSceneUI : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;

    public void OnClickBackToLobby()
    {
        if (sceneController == null)
        {
            Debug.LogError("//SceneController참조가null임(인스펙터에서연결필요)");
            return;
        }

        sceneController.LoadScene(EnumData.sceneType.LobbyScene);
    }
}
