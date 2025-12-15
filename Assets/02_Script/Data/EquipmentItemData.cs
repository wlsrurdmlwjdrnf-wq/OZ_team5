using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItemData
{
    public int id;
    public string name;
    public EnumData.EquipmentType type;
    public EnumData.EquipmentTier tier;
    public float atk;
    public float hp;

    public int specialEffectID; // SO 매핑용 ID

    //무기는 게임시작시 장착되므로 true로 설정 나머지 장비는 false
    public bool WeaponCheck;                              
}
