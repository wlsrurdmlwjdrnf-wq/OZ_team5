using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBGMController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayBGM(EnumData.BGM.BattleBGM);
    }
    private void OnDestroy()
    {
        AudioManager.Instance.StopBGM();
    }
}
