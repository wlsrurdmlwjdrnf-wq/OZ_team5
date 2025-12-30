using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarUI : MonoBehaviour
{
    Player player;
    [SerializeField] EnumData.SkillType type;
    [SerializeField] Image[] haveSkill;

    public void GetPlayerInvenData()
    {
        if (type == EnumData.SkillType.Attack)
        {
            for (int i = 0; i < haveSkill.Length; i++)
            {
                haveSkill[i].sprite = player.PlayerStat().playerSkillInven[i].icon;
            }
        }
        else
        {
            for (int i = 0; i < haveSkill.Length; i++)
            {
                haveSkill[i].sprite = player.PlayerStat().playerSupportInven[i].icon;
            }
        }
    }
}
