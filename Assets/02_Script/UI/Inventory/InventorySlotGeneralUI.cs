using UnityEngine;

//일반 인벤 슬롯(가방)
// - 클릭했을 때 "장착"을 바로 하지 않고
// - 컨트롤러에게 "몇번 슬롯 눌렀는지"만 알려준다
public class InventorySlotGeneralUI : BaseInventorySlotUI
{
    protected override void OnSlotClick()
    {
        //몇번 슬롯 클릭했는지 컨트롤러로 전달
        CallOnClickGeneral(slotNum);
    }
}
