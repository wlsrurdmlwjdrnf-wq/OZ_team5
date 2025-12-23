using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotEquipUI : BaseInventorySlotUI
{
    [SerializeField] private EnumData.EquipmentType equipType;

    //아이템 장착 action 장착해제 아이템 id, 장착 후 아이템 id

    public event Action<int, int> OnEquip;


    //일반 인벤토리에서 클릭했을때 이 함수 호출
    protected override void SetItem(int id)
    {
        int beforID = slotItemID;
        var item = DataManager.Instance.GetItemData(id);
        if (item.type == equipType)
        {
            base.SetItem(id); // 해당 타입의 UI에 icon 띄움            
        }
        else return;

        if (beforID != id)
        {
            OnEquip?.Invoke(beforID, id);
        }
    }

}
