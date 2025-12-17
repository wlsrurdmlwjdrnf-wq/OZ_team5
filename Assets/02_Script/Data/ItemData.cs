using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public int id;
    public string name;
    public EnumData.EquipmentType type;
    public EnumData.EquipmentTier tier;
    public float atk;
    public float hp;
    public int specialEffectID; // SO 매핑용 ID 없으면 -1
    public int pairID; // 조합 패시브 ID 없으면 -1
    public int evolutionID; // 최종진화 ID 없으면 -1

}
