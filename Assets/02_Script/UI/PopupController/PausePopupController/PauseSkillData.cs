using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseSkillData : MonoBehaviour
{
    [SerializeField] Image skillImage;
    [SerializeField] Image[] star;
    [SerializeField] Sprite grayStar;
    [SerializeField] Sprite yellowStar;


    public void SetData(IngameItemData item)
    {
        skillImage.sprite = item.icon;
        if (item.id != 0)
        {
            for (int i = 0; i < star.Length; i++)
            {
                if (i < item.level)
                {
                    star[i].sprite = yellowStar;
                }
                else
                {
                    star[i].sprite = grayStar;
                }
            }
        }
        else
        {
            for (int i = 0;i < star.Length; i++)
            {
                star[i].sprite = grayStar;
            }
        }
    }
}
