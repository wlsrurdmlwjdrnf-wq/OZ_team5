using UnityEngine;

//장착 슬롯(무기/갑옷/목걸이/벨트/장갑/신발)
// - 클릭했을 때 "해제"를 바로 하지 않고
// - 컨트롤러에게 "몇번 슬롯 눌렀는지"만 알려준다
public class InventorySlotEquipUI : BaseInventorySlotUI
{
    protected override void OnSlotClick()
    {
        //몇번 장착칸 클릭했는지 컨트롤러로 전달
        CallOnClickEuip(slotNum);
    }
}
