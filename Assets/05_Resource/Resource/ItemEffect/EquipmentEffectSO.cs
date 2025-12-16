using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 모든 장비아이템 특수효과 부모
public abstract class EquipmentEffectSO : ScriptableObject
{
    [Tooltip("데이터 식별 ID ")]
    [SerializeField] protected int id;

    [Tooltip("특수효과 중첩 가능 여부")]
    [SerializeField] protected bool isStack;

    public int ID => id;
    public bool IsStack => isStack;


    /*
    [Tooltip("플레이어가 해당 효과를 얻고 빼는 함수")]
    public abstract void AddEffect(PlayerData player);
    public abstract void RemoveEffect(PlayerData player);
    */
}
