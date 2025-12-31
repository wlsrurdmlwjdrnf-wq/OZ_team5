using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SupSkillBoxUI : BaseSkillBoxUI
{
    [SerializeField] Transform cbTab;
    [SerializeField] TextMeshProUGUI cbText;
    [SerializeField] GameObject cbPrefab;

    public override void SetupSkillUIData(IngameItemData item)
    {
        base.SetupSkillUIData(item);
        IngameItemData[] temp = DataManager.Instance.GetPairList(item);

        if (temp.Length == 0)
        {
            cbPrefab.SetActive(false);
        }
        else
        {
            for (int i = 0; i < temp.Length; i++)
            {
                GameObject newCbPrefab = Instantiate(cbPrefab, cbTab);
                Image newCbImage = newCbPrefab.GetComponent<Image>();
                newCbImage.sprite = temp[i].icon;
            }
        }
    }
}
