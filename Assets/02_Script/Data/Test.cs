using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM(EnumData.BGM.LobbyBGM);
    }
}
