using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{

    [Tooltip("장비아이템, 인게임아이템(무기스킬/지원폼), 장비아이템 특수효과SO 컨테이너")]
    public Dictionary<int, EquipmentItemData> equimentItemDic = new Dictionary<int, EquipmentItemData>();
    public Dictionary<int, IngameItemData> ingameItemDic = new Dictionary<int, IngameItemData>();
    public Dictionary<int, EquipmentEffectSO> equipmentEffectDic = new Dictionary<int, EquipmentEffectSO>();

    protected override void Init()
    {
        base.Init();
        LoadEquipmentItemData(); // 장비아이템 데이터 로드
        LoadIngameItemData(); // 인게임아이템 데이터 로드
        LoadEffectSO(); // 장비에 부여된 특수효과 데이터 로드
    }

    private void LoadEffectSO()
    {
        var effectSO = Resources.LoadAll<EquipmentEffectSO>("ItemEffect");
        foreach (var effect in effectSO)
        {
            equipmentEffectDic.TryAdd(effect.ID, effect);
        }
    }

    private void LoadEquipmentItemData()
    {
        var dataList = CSVReader.Read("Data/EquipmentData");
        
        foreach (var d in dataList)
        {
            try
            {
                if (!d.ContainsKey("ID")) continue;
                EquipmentItemData data = new EquipmentItemData();

                data.id = Convert.ToInt32(d["ID"]);
                data.name = Convert.ToString(d["Name"]);
                data.type = (EnumData.EquipmentType)Enum.Parse(typeof(EnumData.EquipmentType), d["Type"].ToString(), true);
                data.tier = (EnumData.EquipmentTier)Enum.Parse(typeof(EnumData.EquipmentTier), d["Tier"].ToString(), true);
                
                // 없으면 0으로 처리
                data.atk = d.ContainsKey("Atk") ? Convert.ToInt32(d["Atk"]) : 0.0f;
                data.hp = d.ContainsKey("Hp") ? Convert.ToInt32(d["Hp"]) : 0.0f;               

                // 특수효과가 없는 장비는 0으로 처리
                if (d.ContainsKey("Effect"))
                {
                    data.specialEffectID = Convert.ToInt32(d["Effect"]);
                }
                else
                {
                    data.specialEffectID = 0;
                }

                // 없으면 false
                if (d.ContainsKey("WeaponCheck"))
                {
                    data.weaponCheck = Convert.ToBoolean(d["WeaponCheck"]);
                }
                else
                {
                    data.weaponCheck = false;
                }

                // 조합이 없으면 0
                if (d.ContainsKey("PairID"))
                {
                    data.pairID = Convert.ToInt32(d["PairID"]);
                }
                else
                {
                    data.pairID = 0;
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

    private void LoadIngameItemData()
    {
        var dataList = CSVReader.Read("Data/IngameItemData");

        foreach (var d in dataList)
        {
            try
            {
                if (!d.ContainsKey("ID")) continue;
                
                IngameItemData data = new IngameItemData();

                data.id = Convert.ToInt32(d["ID"]);
                data.name = Convert.ToString(d["Name"]);
                data.type = (EnumData.IngameItemType)Enum.Parse(typeof(EnumData.IngameItemType), d["Type"].ToString() ,true);


                // 조합이 없으면 0
                if (d.ContainsKey("PairID"))
                {
                    data.pairID = Convert.ToInt32(d["PairID"]);
                }
                else
                {
                    data.pairID = 0;
                }

                //최종진화 형태가 없다면 0
                if (d.ContainsKey("EvID"))
                {
                    data.evolutionID = Convert.ToInt32(d["EvID"]);
                }
                else
                {
                    data.evolutionID = 0;
                }
            }
            catch 
            {
                //에러로그
                return;
            }
        }
        //로드 완료
    }
}

