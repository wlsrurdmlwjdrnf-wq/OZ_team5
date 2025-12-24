using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotGeneralUI : BaseInventorySlotUI
{
    protected override void OnSlotClick()
    {
        CallOnClickGeneral(slotNum);
    }
}
