using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotEquipUI : InventorySlotUI
{
    [SerializeField] private EnumData.EquipmentType equipType;

    //아이템 장착 action 장착해제 아이템 id, 장착 후 아이템 id

    public event Action<int, int> OnEquip;

    protected override void SetItem(int id)
    {
        int beforID = slotItemID;

        base.SetItem(id);
        
        if (beforID != id)
        {
            OnEquip?.Invoke(beforID, id);
        }
    }

}
