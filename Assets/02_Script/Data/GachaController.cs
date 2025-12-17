using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 뽑기 버튼을 누르면 룰렛이 돌아가고 선택된 아이템의 이미지가 화면에 나옴
 * 나온 아이템의 정보를 플레이어 인벤토리에 저장해야됨 (inventorymanager additem호출)
 * 뽑기에 필요한 돈의 보유여부는? ( playermanager에서 가진골드값 받아오기)
 */

public class GachaController : MonoBehaviour
{
    [SerializeField] public float legendaryRate = 1f;
    [SerializeField] public float epicRate = 9f;
    [SerializeField] public float rareRate = 90f;

    public event Action<ItemData> OnDrawItem;

    public void DrawItem()
    {
        // 돈이 부족한지 체크 부족하면 리턴

        EnumData.EquipmentTier tier = GetRarity();
        ItemData item = GetItemByRarity(tier);
        
        if (item != null)
        {
            //아이템을 얻었다고 알림
            OnDrawItem.Invoke(item);

            //인벤토리에 해당 아이템 추가
            InventoryManager.Instance.AddItem(EnumData.InventoryType.General, item.id);
        }       
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
