using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PausePopUIController : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI killText;
    [SerializeField] PauseSkillData[] atkInven;
    [SerializeField] PauseSkillData[] supInven;

    private void OnEnable()
    {
        for (int i = 0; i < 6; i++)
        {
            atkInven[i].SetData(player.PlayerStat().playerSkillInven[i]);
            supInven[i].SetData(player.PlayerStat().playerSupportInven[i]);
        }
    }

}
