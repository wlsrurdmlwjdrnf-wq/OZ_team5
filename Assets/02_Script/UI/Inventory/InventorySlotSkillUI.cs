using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotSkillUI : BaseInventorySlotUI
{
    protected override void OnSlotClick()
    {
        CallOnClickSkill(slotNum);
    }
}
