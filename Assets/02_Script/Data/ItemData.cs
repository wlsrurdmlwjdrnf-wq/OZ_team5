using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public int id;
    public string name;
    public Sprite icon;
    public EnumData.EquipmentType type;
    public EnumData.EquipmentTier tier;
    public float atkMtp; // 등급에 따른 공격력 증가 배수 2배 3배 등등
    public int atkPercent; // 등급에 따른 공격력 증가량 10%증가 20%증가 등등
    public int hpPercent; // 등급에 따른 체력 증가량
    public int specialEffectID; // SO 매핑용 ID 없으면 -1
    public int[] pairID; // 조합 패시브 ID 없으면 -1
    public int evolutionID; // 최종진화 ID 없으면 -1
}
