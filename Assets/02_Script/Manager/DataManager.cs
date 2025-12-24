using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    // 종류별 아이템 컨테이너
    private Dictionary<int, ItemData> itemDataDic = new Dictionary<int, ItemData>();
    private Dictionary<int, EquipmentEffectSO> equipmentEffectDic = new Dictionary<int, EquipmentEffectSO>();
    private Dictionary<int, IngameItemData> ingameItemDataDic = new Dictionary<int, IngameItemData>();

    //이미지 파일 컨테이너
    private Dictionary<string, Sprite> itemIcon = new Dictionary<string, Sprite>();

    //뽑기를 위한 티어별 아이템 컨테이너
    private Dictionary<EnumData.EquipmentTier, List<ItemData>> itemRarityDic = new Dictionary<EnumData.EquipmentTier, List<ItemData>>();

    public PlayerData playerData {  get; private set; }

    protected override void Init()
    {
        base.Init();
        LoadItemData(); // 아이템 데이터 로드
        LoadEffectSO(); // 장비에 부여된 특수효과 데이터 로드
        LoadIngameItemData(); // 무기스킬, 지원폼 데이터 로드
        LoadRarityItemData(); // 티어별 데이터 로드
        LoadPlayerStat();
        // LogData(); // 테스트용 로그함수
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
                    //중복ID 로그
                    Debug.LogError($"ID : {data.id}는 이미 존재하는 ID 입니다."); 
                    continue;
                }
                data.name = Convert.ToString(d["Name"]);
                data.type = (d.ContainsKey("Type") && Enum.TryParse(d["Type"].ToString(), out EnumData.EquipmentType ety)) ? ety : EnumData.EquipmentType.NONE;
                data.tier = (d.ContainsKey("Tier") && Enum.TryParse(d["Tier"].ToString(), out EnumData.EquipmentTier etr)) ? etr : EnumData.EquipmentTier.NONE;
                data.atkMtp = (d.ContainsKey("AtkMtp") && float.TryParse(d["AtkMtp"].ToString(), out float atkmtp)) ? atkmtp : -1.0f;
                data.atkPercent = (d.ContainsKey("AtkPercent") && int.TryParse(d["AtkPercent"].ToString(), out int atkper)) ? atkper : -1;
                data.hpPercent = (d.ContainsKey("HpPercent") && int.TryParse(d["HpPercent"].ToString(), out int hpper)) ? hpper : -1;
                data.specialEffectID = (d.ContainsKey("Effect") && int.TryParse(d["Effect"].ToString(), out int efc)) ? efc : -1;
                data.evolutionID = (d.ContainsKey("EvID") && int.TryParse(d["EvID"].ToString(), out int eid)) ? eid : -1;
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

                itemDataDic.Add(data.id, data);
            }
            catch
            {
                //로드실패로그
                Debug.LogError($"DataTable 파일 {excelCount}번째 데이터 로드 실패 잘못된 데이터값");
                continue;
            }
        }
        //로드 완료
        Debug.Log("장비 아이템 로드완료");
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

                if (ingameItemDataDic.ContainsKey(data.id))
                {
                    //중복ID로그
                    Debug.LogError($"ID : {data.id}는 이미 존재하는 ID 입니다.");
                    continue;
                }

                data.name = Convert.ToString(d["Name"]);
                data.type = (d.ContainsKey("Type") && Enum.TryParse(d["Type"].ToString(), out EnumData.SkillType skty)) ? skty : EnumData.SkillType.NONE;
                data.damage = (d.ContainsKey("Damage") && int.TryParse(d["Damage"].ToString(), out int dmg)) ? dmg : -1;
                data.level = (d.ContainsKey("Level") && int.TryParse(d["Level"].ToString(), out int lv)) ? lv : 1;
                data.ptCount = (d.ContainsKey("PtCount") && int.TryParse(d["PtCount"].ToString(), out int ptc)) ? ptc : -1;
                data.ptSpeed = (d.ContainsKey("PtSpeed") && int.TryParse(d["PtSpeed"].ToString(), out int pts)) ? pts : -1;
                data.specialEffectID = (d.ContainsKey("Effect") && int.TryParse(d["Effect"].ToString(), out int efso)) ? efso : -1;
                data.EvID = (d.ContainsKey("EvID") && int.TryParse(d["EvID"].ToString(), out int ev)) ? ev : -1;


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
                

                ingameItemDataDic.Add(data.id, data);
            }
            catch
            {
                //로드실패로그
                Debug.LogError($"IngameItemData 파일 {excelCount}번째 데이터 로드 실패 잘못된 데이터값");
                continue;
            }
        }
        //로드완료
        Debug.Log("무기스킬 / 지원폼 데이터 로드완료");
    }

    public void LoadPlayerStat()
    {
        playerData = new PlayerData();
    }


    #endregion

    #region 데이터 겟 함수

    public ItemData GetItemData(int id)
    {
        if (itemDataDic.ContainsKey(id)) return itemDataDic[id];
        return null;
    }

    public IngameItemData GetIngameItemData(int id)
    {
        if (ingameItemDataDic.ContainsKey(id)) return ingameItemDataDic[id];
        return null;
    }

    public EquipmentEffectSO GetEquipEffectData(int id)
    {
        if (equipmentEffectDic.ContainsKey(id)) return equipmentEffectDic[id];
        return null;
    }


    // 등급별 아이템 리스트 리턴함수
    public List<ItemData> GetItemRarityList(EnumData.EquipmentTier tier)
    {
        if (itemRarityDic.ContainsKey(tier)) return itemRarityDic[tier];
        return null;
    }

    //아이템 아이콘 스프라이트 리턴함수
    public Sprite GetItemIcon(string icon)
    {
        if (itemIcon.TryGetValue(icon, out Sprite sprite))
        {
            return sprite;
        }

        Sprite newSprite = Resources.Load<Sprite>($"Icons/{icon}");
        if (newSprite != null)
        {
            itemIcon.Add(icon, newSprite);
            return sprite;
        }
        return null;
    }

    #endregion

    //테스트용 로그함수
    public void LogData()
    {
        foreach (var data in itemDataDic)
        {
            int key = data.Key;
            var value = data.Value;

            Debug.Log($"{key}번 데이터 로드... ID : {value.id}, Name : {value.name}, Type : {value.type}, Tier : {value.tier}, AtkMtp : {value.atkMtp}, AtkPercent : {value.atkPercent}, HpPercent : {value.hpPercent}, Effect {value.specialEffectID}, EvID : {value.evolutionID}, PairID : {value.pairID.Length}");
        }
    }
}

