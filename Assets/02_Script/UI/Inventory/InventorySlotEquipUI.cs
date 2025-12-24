using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotEquipUI : BaseInventorySlotUI
{
    protected override void OnSlotClick()
    {
        CallOnClickEuip(slotNum);
    }
}
