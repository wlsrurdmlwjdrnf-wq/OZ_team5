using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 뽑기 버튼을 누르면 룰렛이 돌아가고 선택된 아이템의 이미지가 화면에 나옴
 * 나온 아이템의 정보를 플레이어 인벤토리에 저장해야됨 (inventorymanager additem호출)
 * 뽑기에 필요한 돈의 보유여부는? ( playermanager에서 가진골드값 받아오기)
 */

public class GachaManager : Singleton<GachaManager>
{

    [SerializeField] public float niceRate = 45f;
    [SerializeField] public float rareRate = 25f;
    [SerializeField] public float eliteRate = 20f;
    [SerializeField] public float epicRate = 9f;
    [SerializeField] public float legendaryRate = 1f;

    public event Action<ItemData> OnDrawItem;

    protected override void Init()
    {
        base.Init();
    }
    //뽑기 버튼을 누르면 이 함수를 호출하세요
    public void DrawItem()
    {
        // 돈이 부족한지 체크 부족하면 리턴


        //등급을 먼저 뽑고 뽑은 등급의 컨테이너에서 장비 한개 뽑아서 item 변수에 저장
        EnumData.EquipmentTier tier = GetRarity();
        ItemData item = GetItemByRarity(tier);


        //아이템 뽑았다고 외부에 알리기
        OnDrawItem?.Invoke(item);

    }


    // 무슨 등급인지 반환
    public EnumData.EquipmentTier GetRarity()
    {
        int value = UnityEngine.Random.Range(0, 100);

        if (value < legendaryRate) // 0이면 레전더리
        {
            return EnumData.EquipmentTier.Legendary;
        }
        else if (value <  epicRate) // 1 ~ 9면 에픽
        {
            return EnumData.EquipmentTier.Epic;
        }
        else // 나머지는 레어
        {
            return EnumData.EquipmentTier.Rare;
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
