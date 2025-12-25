using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotGeneralUI : BaseInventorySlotUI
{
    private void Start()
    {
        OnClickGeneralSlot += PlayerManager.Instance.EquipItem;
    }
    protected override void OnSlotClick()
    {
        CallOnClickGeneral(slotNum);
    }
    protected override void OnDestroy()
    {
        OnClickGeneralSlot -= PlayerManager.Instance.EquipItem;
    }
}
