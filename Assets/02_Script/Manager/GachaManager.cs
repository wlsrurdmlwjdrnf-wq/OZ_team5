using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaManager : Singleton<GachaManager>
{
    private int rareRate;
    private int eliteRate;
    private int epicRate;
    private int legendaryRate;

    public event Action<ItemData> OnDrawItem;
   
    protected override void Init()
    {
        base.Init();
        
    }
    //일반상자뽑기
    public void DrawItemNormalBox()
    {
        //if (GameManager.Instance.gameGold < 300) return;

        EnumData.EquipmentTier tier = GetRarity(0);
        ItemData item = GetItemByRarity(tier);

        OnDrawItem?.Invoke(item);
    }

    public void DrawItemEpicBox()
    {
       // if (GameManager.Instance.gameGold < 3000) return;

        EnumData.EquipmentTier tier = GetRarity(1);
        ItemData item = GetItemByRarity(tier);

        OnDrawItem?.Invoke(item);
    }


    // 무슨 등급인지 반환
    public EnumData.EquipmentTier GetRarity(int value)
    {
        if (value == 0)
        {
            int rate = UnityEngine.Random.Range(0, 100);
            rareRate = 30;
            eliteRate = 1;

            int  result = 0;

            result += eliteRate;
            if (rate < result)
            {
                return EnumData.EquipmentTier.Elite;
            }
            result += rareRate;
            if (rate < result)
            {
                return EnumData.EquipmentTier.Rare;
            }
            else
            {
                return EnumData.EquipmentTier.Nice;
            }
        }
        else
        {
            int rate = UnityEngine.Random.Range(0, 100);
            eliteRate = 69;
            epicRate = 30;
            legendaryRate = 1;

            int result = 0;

            result += legendaryRate;
            if (rate < result)
            {
                return EnumData.EquipmentTier.Legendary;
            }
            result += epicRate;
            if (rate < result)
            {
                return EnumData.EquipmentTier.Epic;
            }
            else
            {
                return EnumData.EquipmentTier.Elite;
            }
        }
    }

    //등급별로 분류된 컨테이너에서 랜덤아이템 뽑아서 반환
    public ItemData GetItemByRarity(EnumData.EquipmentTier tier)
    {       
        List<ItemData> item = DataManager.Instance.GetItemRarityList(tier);
        if (item == null || item.Count == 0) return null;
        int value = UnityEngine.Random.Range(0, item.Count);
        return item[value];
    }     
}
