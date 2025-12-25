using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInventoryUIController : BaseInventoryUIController
{
    [SerializeField] private InventorySlotSkillUI[] skillUI;
    [SerializeField] private InventorySlotSupportUI[] supportUI;

    protected override void Start()
    {
        base.Start();
        SetSlotNum();
    }
    protected override void SetSlotNum()
    {
        for (int i = 0; i < skillUI.Length; i++)
        {
            skillUI[i].SetSlotNumber(i);
        }
        for (int i = 0; i < supportUI.Length; i++)
        {
            supportUI[i].SetSlotNumber(i);
        }
    }
    protected override void LoadInven()
    {
        //플레이어의 skillinven에서 데이터를 불러와 view에 띄움
        for (int i = 0; i < skillUI.Length; i++)
        {
            IngameItemData item = PlayerManager.Instance.playerData.playerSkillInven[i];
            skillUI[i].SetSlotView(item.id);
        }
        //플레이어의 supportinven에서 데이터를 불러와 view에 띄움
        for (int i = 0;i < supportUI.Length; i++)
        {
            IngameItemData item = PlayerManager.Instance.playerData.playerSupportInven[i];
            supportUI[i].SetSlotView(item.id);
        }
    }
}
