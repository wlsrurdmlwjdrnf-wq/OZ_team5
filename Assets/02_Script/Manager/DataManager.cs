using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{

    [Tooltip("아이템, 특수효과SO 컨테이너")]
    public Dictionary<int, ItemData> itemDataDic = new Dictionary<int, ItemData>();
    public Dictionary<int, EquipmentEffectSO> equipmentEffectDic = new Dictionary<int, EquipmentEffectSO>();

    [Tooltip("등급별 분류 컨테이너")]
    public Dictionary<EnumData.EquipmentTier, List<ItemData>> itemRarityDic = new Dictionary<EnumData.EquipmentTier, List<ItemData>>();


    protected override void Init()
    {
        base.Init();
        LoadItemData(); // 아이템 데이터 로드
        LoadEffectSO(); // 장비에 부여된 특수효과 데이터 로드
        LoadRarityItemData();
    }

    #region 데이터 로드 함수

    private void LoadEffectSO()
    {
        var effectSO = Resources.LoadAll<EquipmentEffectSO>("ItemEffect");
        foreach (var effect in effectSO)
        {
            equipmentEffectDic.TryAdd(effect.ID, effect);
        }
    }

    private void LoadItemData()
    {
        var dataList = CSVReader.Read("Data/DataTable");
        
        foreach (var d in dataList)
        {
            try
            {
                if (!d.ContainsKey("ID")) continue;
                ItemData data = new ItemData();

                data.id = Convert.ToInt32(d["ID"]);
                data.name = Convert.ToString(d["Name"]);
                data.type = (EnumData.EquipmentType)Enum.Parse(typeof(EnumData.EquipmentType), d["Type"].ToString(), true);
                data.tier = (EnumData.EquipmentTier)Enum.Parse(typeof(EnumData.EquipmentTier), d["Tier"].ToString(), true);
                
                // 없으면 0으로 처리
                data.atk = d.ContainsKey("Atk") ? Convert.ToInt32(d["Atk"]) : 0.0f;
                data.hp = d.ContainsKey("Hp") ? Convert.ToInt32(d["Hp"]) : 0.0f;               

                // 특수효과가 없는 장비는 -1로 처리
                if (d.ContainsKey("Effect"))
                {
                    data.specialEffectID = Convert.ToInt32(d["Effect"]);
                }
                else
                {
                    data.specialEffectID = -1;
                }

                // 조합이 없으면 -1
                if (d.ContainsKey("PairID"))
                {
                    data.pairID = Convert.ToInt32(d["PairID"]);
                }
                else
                {
                    data.pairID = -1;
                }

                //최종진화 형태가 없다면 -1
                if (d.ContainsKey("EvID"))
                {
                    data.evolutionID = Convert.ToInt32(d["EvID"]);
                }
                else
                {
                    data.evolutionID = -1;
                }

            }
            catch
            {
                //오류 로그
                return;
            }
        }
        //로드 완료
    }

    private void LoadRarityItemData()
    {
        itemRarityDic.Add(EnumData.EquipmentTier.Rare, new List<ItemData>());
        itemRarityDic.Add(EnumData.EquipmentTier.Epic, new List<ItemData>());
        itemRarityDic.Add(EnumData.EquipmentTier.Legendary, new List<ItemData>());

        foreach (var item  in itemDataDic.Values)
        {
            if (itemRarityDic.ContainsKey(item.tier))
            {
                itemRarityDic[item.tier].Add(item);
            }
        }
    }

    #endregion


    // 아이템 정보 리턴함수
    public ItemData GetItemData(int id)
    {
        if (itemDataDic.ContainsKey(id)) return itemDataDic[id];
            return null;        
    }

    // 등급별 아이템 리스트 리턴함수
    public List<ItemData> GetItemRarityList(EnumData.EquipmentTier tier)
    {
        if (itemRarityDic.ContainsKey(tier)) return itemRarityDic[tier];
            return null;
    }

    //장비 특수효과 리턴함수
    public EquipmentEffectSO GetEquipEffectData(int id)
    {
        if (equipmentEffectDic.ContainsKey(id)) return equipmentEffectDic[id];
            return null;        
    }
}

