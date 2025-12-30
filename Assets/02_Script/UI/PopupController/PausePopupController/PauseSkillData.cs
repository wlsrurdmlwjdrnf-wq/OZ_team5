using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseSkillData : MonoBehaviour
{
    [SerializeField] Image skillImage;
    [SerializeField] Image[] star;
    [SerializeField] Sprite garyStar;
    [SerializeField] Sprite yellowStar;


    public void SetData(IngameItemData item)
    {
        skillImage.sprite = item.icon;
        for (int i = 0; i < star.Length; i++)
        {
            if (i < item.level)
            {
                star[i].sprite = yellowStar;
            }
            else
            {
                star[i].sprite = garyStar;
            }
        }
    }
}
