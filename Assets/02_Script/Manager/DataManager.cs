using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{

    [Tooltip("아이템, 특수효과SO, 인게임무기스킬,지원폼 컨테이너")]
    public Dictionary<int, ItemData> itemDataDic = new Dictionary<int, ItemData>();
    public Dictionary<int, EquipmentEffectSO> equipmentEffectDic = new Dictionary<int, EquipmentEffectSO>();
    public Dictionary<int, IngameItemData> ingameItemDataDic = new Dictionary<int, IngameItemData>();


    [Tooltip("등급별 분류 컨테이너")]
    public Dictionary<EnumData.EquipmentTier, List<ItemData>> itemRarityDic = new Dictionary<EnumData.EquipmentTier, List<ItemData>>();


    protected override void Init()
    {
        base.Init();
        LoadItemData(); // 아이템 데이터 로드
        LoadEffectSO(); // 장비에 부여된 특수효과 데이터 로드
        LoadIngameItemData(); // 무기스킬, 지원폼 데이터 로드
        LoadRarityItemData(); // 티어별 데이터 로드
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
        int excelCount = 1;
        itemDataDic.Clear();

        foreach (var d in dataList)
        {
            excelCount++;
            try
            {
                if (!d.ContainsKey("ID") || string.IsNullOrEmpty(d["ID"].ToString())) continue;
                ItemData data = new ItemData();

                data.id = Convert.ToInt32(d["ID"]);
                
                if (itemDataDic.ContainsKey(data.id))
                {
                    Debug.LogError($"ID : {data.id}는 이미 존재하는 ID 입니다."); 
                    continue;
                }
                data.name = Convert.ToString(d["Name"]);
                data.type = (EnumData.EquipmentType)Enum.Parse(typeof(EnumData.EquipmentType), d["Type"].ToString(), true);
                data.tier = (EnumData.EquipmentTier)Enum.Parse(typeof(EnumData.EquipmentTier), d["Tier"].ToString(), true);
                
                // 없으면 0으로 처리 음수처리에대한 예외?
                data.atkMtp = d.ContainsKey("AtkMtp") ? Convert.ToSingle(d["AtkMtp"]) : 0.0f;
                data.atkPercent = d.ContainsKey("AtkPercent") ? Convert.ToInt32(d["AtkPercent"]) : 0;
                data.hpPercent = d.ContainsKey("HpPercent") ? Convert.ToInt32(d["HpPercent"]) : 0;
                data.specialEffectID = d.ContainsKey("Effect") ? Convert.ToInt32(d["Effect"]) : -1;

                // 조합이 없으면 -1
                // 조합이 여러개면 개수에 따라 배열로 받습니다
                if (d.ContainsKey("PairID"))
                {
                    string tempPair = d["PairID"].ToString();

                    if (string.IsNullOrEmpty(tempPair))
                    {
                        data.pairID = new int[] { -1 };
                    }
                    else
                    {
                        string[] tempData = tempPair.Split('|');
                        data.pairID = new int[tempData.Length];

                        for (int i = 0; i < tempData.Length; i++)
                        {
                            data.pairID[i] = int.Parse(tempData[i]);
                        }                                                       
                    }
                }

                //최종진화 형태가 없다면 -1
                data.evolutionID = d.ContainsKey("EvID") ? Convert.ToInt32(d["EvID"]) : -1; 

                itemDataDic.Add(data.id, data);
            }
            catch
            {
                Debug.LogError($"DataTable 파일 {excelCount}번째 데이터 로드 실패 부적절한 데이터값");
                continue;
            }
        }
        //로드 완료
    }

    private void LoadRarityItemData()
    {
        itemRarityDic.Add(EnumData.EquipmentTier.Nice, new List<ItemData>());
        itemRarityDic.Add(EnumData.EquipmentTier.Rare, new List<ItemData>());
        itemRarityDic.Add(EnumData.EquipmentTier.Elite, new List<ItemData>());
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

    private void LoadIngameItemData()
    {
        var dataList = CSVReader.Read("Data/IngameItemData");
        int excelCount = 1;
        ingameItemDataDic.Clear();

        foreach (var d in dataList)
        {
            excelCount++;

            try
            {
                if (!d.ContainsKey("ID") || string.IsNullOrEmpty(d["ID"].ToString())) continue;

                IngameItemData data = new IngameItemData();

                data.id = Convert.ToInt32(d["ID"]);

                if (itemDataDic.ContainsKey(data.id))
                {
                    Debug.LogError($"ID : {data.id}는 이미 존재하는 ID 입니다.");
                    continue;
                }

                data.name = Convert.ToString(d["Name"]);
                data.type = (EnumData.SkillType)Enum.Parse(typeof(EnumData.SkillType), d["Type"].ToString(), true);
                data.damage = d.ContainsKey("Damage") ? Convert.ToInt32(d["Damage"]) : 0;
                data.level = d.ContainsKey("Level") ? Convert.ToInt32(d["Level"]) : 1;
                data.ptCount = d.ContainsKey("PtCount") ? Convert.ToInt32(d["PtCount"]) : 0;
                data.ptSpeed = d.ContainsKey("PtSpeed") ? Convert.ToInt32(d["PtSpeed"]) : 0;

                if (d.ContainsKey("PairID"))
                {
                    string tempPair = d["PairID"].ToString();

                    if(string.IsNullOrEmpty(tempPair))
                    {
                        data.pairID = new int[] { -1 };
                    }
                    else
                    {
                        string[] tempData = tempPair.Split('|');
                        data.pairID = new int[tempData.Length];

                        for (int i = 0; i < tempData.Length; i++)
                        {
                            data.pairID[i] =int.Parse(tempData[i]);
                        }
                    }
                }

                if (d.ContainsKey("EvID"))
                {
                    data.EvID = Convert.ToInt32(d["EvID"]);
                }
                else
                {
                    data.EvID = -1;
                }

                ingameItemDataDic.Add(data.id, data);
            }
            catch
            {
                Debug.LogError($"IngameItemData 파일 {excelCount}번째 데이터 로드 실패 부적절한 데이터값");
                continue;
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

