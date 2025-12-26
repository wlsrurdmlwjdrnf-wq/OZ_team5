using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotEquipUI : BaseInventorySlotUI
{
    private void Start()
    {
        OnClickEquipSlot += PlayerManager.Instance.UnEquipItem;   
    }
    protected override void OnSlotClick()
    {
        CallOnClickEuip(slotNum);
    }
    protected override void OnDestroy()
    {
        OnClickEquipSlot -= PlayerManager.Instance.UnEquipItem;
    }
}
