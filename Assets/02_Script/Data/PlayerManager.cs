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
        //해당슬롯 번호와 일치하는 플레이어 인벤토리에서 아이템 데이터 불러오기
        ItemData temp = playerData.playerGeneralInven[slot];

        //장비칸을 돌면서 비어있는지, 장착하려는 아이템이 같은 type인지 확인
        for (int i = 0; i < playerData.playerEquipInven.Count; i++)
        {
            if (playerData.playerEquipInven[(EnumData.EquipmentType)i].id == 0 && (EnumData.EquipmentType)i == temp.type)
            {
                
            }
        }
        //일반칸에서 장비칸으로 데이터를 옮겨야됨
        
        //아이템 데이터들이 플레이어 인벤토리에 다 옮겨지고나서 스탯리셋함수호출
        ResetPlayerStat();
        //아이템icon을 띄우라고 action 호출
        OnItemUpdata?.Invoke();
    }

    public void UnEquipItem(int id)
    {


        //ResetPlayerStat();
    }

    //플레이어의 스탯변화시(장비변화, 진화스탯변화) 호출 버그발생을 없애기 위해 호출시마다 초기화 후 데이터 대입
    public void ResetPlayerStat()
    {
        DataManager.Instance.LoadPlayerStat();

        foreach (var itemId in playerData.playerEquipInven.Values)
        {
            if (itemId == null) continue;

            ItemData item = DataManager.Instance.GetItemData(itemId.id);

            playerData.resultAtkPer += item.atkPercent;
            playerData.resultAtkMtp += item.atkMtp;
            playerData.resultHpPer += item.hpPercent;
        }
        float oldMaxHp = playerData.playerMaxHp;
        
        playerData.playerMaxHp = playerData.playerCurrentHp * (1f + playerData.resultHpPer);
        if (playerData.playerMaxHp > oldMaxHp )
        {
            float temp = playerData.playerMaxHp - oldMaxHp;
            playerData.playerCurrentHp += temp;
        }
        playerData.playerAtk = (playerData.playerAtk * (1f + playerData.resultAtkPer)) * playerData.resultAtkPer;        
    }

}
