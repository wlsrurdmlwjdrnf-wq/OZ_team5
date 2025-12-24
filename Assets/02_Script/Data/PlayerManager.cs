using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerData playerData {  get; private set; }
    private BaseInventorySlotUI slotUI;


    public event Action OnItemUpdata;

    protected override void Init()
    {
        base.Init();
        DataManager.Instance.LoadPlayerStat();
        playerData = DataManager.Instance.playerData;
        slotUI.OnClickEquipSlot += UnEquipItem;
        slotUI.OnClickGeneralSlot += EquipItem;
    }

    //장비 장착
    public void EquipItem(int slot)
    {
        /* 장착할 아이템 : item
         * 빈아이템으로 채우기 위한 아이템 : empty
         * 장착할 아이템과 같은 타입의 장착중인 아이템 : curItem
         * 장착하기위한 타입 : type
         */
        ItemData item = playerData.playerGeneralInven[slot];
        if (item.id == 0) return;
        ItemData empty = DataManager.Instance.GetItemData(0);

        ItemData curItem = playerData.playerEquipInven[item.type];

        //장비칸이 비었다면
        if (curItem.id == 0)
        {
            playerData.playerEquipInven[item.type] = item;
            playerData.playerGeneralInven[slot] = empty;
        }
        else
        {
            playerData.playerEquipInven[item.type] = item;
            playerData.playerGeneralInven[slot] = curItem;

        }
        //장비착용로직이 끝난 후 장비에 따른 스탯변화를 위해 resetstat 호출
        ResetPlayerStat();
        //변경된 인벤토리들의 아이템icon을 띄우라고 action 호출
        OnItemUpdata?.Invoke();
    }

    //장비 해제
    public void UnEquipItem(int slot)
    {
        //장착된 아이템 curitem
        ItemData curitem = playerData.playerEquipInven[(EnumData.EquipmentType)slot];
        ItemData empty = DataManager.Instance.GetItemData(0);

        if (curitem.id == 0) return;

        //장비 해제시 들어간 빈 인벤토리가 있는지 확인
        int index = playerData.playerGeneralInven.FindIndex(item => item.id == 0);
        
        //빈 인벤토리가 있다면
        if (index != -1)
        {
            playerData.playerEquipInven[curitem.type] = empty;
            playerData.playerGeneralInven[index] = curitem;
        }

        ResetPlayerStat();
        OnItemUpdata?.Invoke();
    }


    //플레이어의 스탯변화시(장비변화, 진화스탯변화) 호출 버그발생을 없애기 위해 호출시마다 초기화 후 데이터 대입
    public void ResetPlayerStat()
    {
        float oldMaxHp = playerData.playerMaxHp;
        DataManager.Instance.LoadPlayerStat();

        float totalMtp = 0f;

        foreach (var item in playerData.playerEquipInven.Values)
        {
            if (item == null || item.id == 0) continue;

            playerData.resultAtkPer += item.atkPercent;
            
            if (item.atkMtp > 1f)
            {
                totalMtp += (item.atkMtp - 1f);
            }

            playerData.resultHpPer += item.hpPercent;
        }

        playerData.playerMaxHp = playerData.playerMaxHp * (1f + playerData.resultHpPer);

        if (playerData.playerMaxHp > oldMaxHp)
        {
            float temp = playerData.playerMaxHp - oldMaxHp;
            playerData.playerCurrentHp += temp;
        }
        else if (playerData.playerMaxHp < oldMaxHp)
        {
            if (playerData.playerCurrentHp > playerData.playerMaxHp)
            {
                playerData.playerCurrentHp = playerData.playerMaxHp;
            }            
        }

        playerData.playerAtk = playerData.playerAtk * (1f + playerData.resultAtkPer) * (1f + totalMtp);
    }
}
