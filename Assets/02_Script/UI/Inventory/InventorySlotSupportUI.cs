using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotSupportUI : BaseInventorySlotUI
{
    protected override void OnSlotClick()
    {
        CallOnClickSupport(slotNum);
    }
}
