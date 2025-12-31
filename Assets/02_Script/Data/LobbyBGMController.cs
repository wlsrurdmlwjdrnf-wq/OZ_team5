using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyBGMController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayBGM(EnumData.BGM.LobbyBGM);
    }
    private void OnDestroy()
    {
        AudioManager.Instance.StopBGM();
    }
}
