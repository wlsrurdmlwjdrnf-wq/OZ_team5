using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyInventoryUIController : BaseInventoryUIController
{
    [SerializeField] private InventorySlotEquipUI[] equipUI;
    [SerializeField] private InventorySlotGeneralUI[] generalUI;

    protected override void Start()
    {
        base.Start();
        SetSlotNum();
    }
    protected override void SetSlotNum()
    {
        for (int i = 0; i < equipUI.Length; i++)
        {
            equipUI[i].SetSlotNumber(i);
        }
        for (int i = 0; i < generalUI.Length; i++)
        {
            generalUI[i].SetSlotNumber(i);
        }
    }
    protected override void LoadInven()
    {
        //플레이어의 장착아이템 리스트에서 값을 받아와서 View에 띄우도록 호출
        for (int i = 0;  i < equipUI.Length; i++)
        {

            ItemData item = PlayerManager.Instance.playerData.playerEquipInven[(EnumData.EquipmentType)i];

            //아이템이 정상적으로 들어왔는지 확인용 디버그
            //Debug.Log($"{item.name}");
            equipUI[i].SetSlotView(item.id);
        }
        //플레이어의 기본인벤토리 리스트에서 값을 받아와서 View에 띄우도록 호출
        for (int i = 0;i < generalUI.Length; i++)
        {
            ItemData item = PlayerManager.Instance.playerData.playerGeneralInven[i];

            //아이템이 정상적으로 들어왔는지 확인용 디버그
            //Debug.Log($"{item.name}");
            generalUI[i].SetSlotView(item.id);
        }
    }

}
