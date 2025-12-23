using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{

    protected override void Init()
    {
        base.Init();        
    }

    public void EquipItem()
    {

    }

    public void UnEquipItem()
    {

    }

    public void ResetPlayerStat()
    {
        DataManager.Instance.LoadPlayerStat();
        PlayerData data = DataManager.Instance.playerData;

        foreach (var itemId in data.playerEquipInven)
        {
            if (itemId == -1) continue;

            ItemData item = DataManager.Instance.GetItemData(itemId);

            data.resultAtkPer += item.atkPercent;
            data.resultAtkMtp += item.atkMtp;
            
        }
        
    }

}
