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
        if (PlayerManager.Instance != null)
        {
            OnClickGeneralSlot -= PlayerManager.Instance.EquipItem;
        }
    }
}
