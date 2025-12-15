using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon = 1,
    Chest,
    Pants,
    Boots,
    Necklace,
    Gloves
}

public enum ItemTier
{
    Normal = 1,
    Rare,
    Epic
}
public class ItemData_ : MonoBehaviour
{
    public int ID; // 아이템의 넘버링 번호
    public string Name; // 아이템의 이름
    public ItemType Type; // 아이템의 종류
    public ItemTier Tier; // 아이템의 등급
    public int BaseATK; // 기본 공격력
    public int GrowthATK; // 성장 공격력
    public int BaseDEF; // 기본 방어력
    public int GrowthDEF; // 성장 방어력
    public int ConnectedSkill_ID; // 연결된 스킬 넘버링 번호
}
