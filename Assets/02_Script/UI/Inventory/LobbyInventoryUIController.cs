using UnityEngine;

//로비 인벤토리 UI 전체를 관리하는 컨트롤러
// - 플레이어 인벤 데이터(PlayerManager.playerData)를 읽어서 UI에 뿌려줌
// - 슬롯 클릭 이벤트를 받아 "아이템 팝업"을 띄우는 역할
// - 실제 장착/해제 로직은 PlayerManager가 담당
public class LobbyInventoryUIController : BaseInventoryUIController
{
    [SerializeField] private InventorySlotEquipUI[] equipUI;    //장비 슬롯 UI 배열(무기, 방어구 등)
    [SerializeField] private InventorySlotGeneralUI[] generalUI;//일반 인벤 슬롯 UI 배열(가방)
    [SerializeField] private InventoryItemPopup itemPopup;      //아이템 상세 팝업 클릭시 팝업을 열어서 장착/해제 버튼을 제공

    protected override void Start()
    {
        //BaseInventoryUIController에서
        base.Start();

        //각 슬롯에 "몇번 슬롯인지" 번호를 설정
        SetSlotNum();

        //슬롯 클릭 이벤트를 컨트롤러에서 직접 받기 위해 바인딩
        BindSlotEvents();
    }

    //슬롯 번호 설정
    //UI 배열의 인덱스를 실제 인벤 인덱스로 사용
    protected override void SetSlotNum()
    {
        //장비 슬롯 번호 설정(0~5)
        for (int i = 0; i < equipUI.Length; i++)
        {
            equipUI[i].SetSlotNumber(i);
        }

        //일반 인벤 슬롯 번호 설정
        for (int i = 0; i < generalUI.Length; i++)
        {
            generalUI[i].SetSlotNumber(i);
        }
    }

    //슬롯 클릭 이벤트 바인딩
    //슬롯 자체에서는 "클릭했다"는 사실만 알려주고
    //어떤 동작을 할지는 이 컨트롤러가 결정
    private void BindSlotEvents()
    {
        //장착 슬롯 클릭 이벤트
        // - 장착된 아이템 클릭 → "해제 팝업"을 띄우기 위함
        for (int i = 0; i < equipUI.Length; i++)
        {
            int idx = i; //람다 캡처용 지역 변수
            equipUI[i].OnClickEquipSlot += _ => OnClickEquipSlot(idx);
        }

        //일반 인벤 슬롯 클릭 이벤트
        // - 가방 아이템 클릭 → "장착 팝업"을 띄우기 위함
        for (int i = 0; i < generalUI.Length; i++)
        {
            int idx = i; //람다 캡처용 지역 변수
            generalUI[i].OnClickGeneralSlot += _ => OnClickGeneralSlot(idx);
        }
    }

    //인벤 UI 갱신 함수
    //BaseInventoryUIController에서
    // - 게임 시작 시
    // - 아이템 장착/해제 시(PlayerManager.OnItemUpdata)
    //자동으로 호출됨
    protected override void LoadInven()
    {
        //장착 슬롯 UI 갱신
        for (int i = 0; i < equipUI.Length; i++)
        {
            ItemData item =
                PlayerManager.Instance.playerData.playerEquipInven[(EnumData.EquipmentType)i];

            //아이템 id를 전달하면
            //슬롯 UI가 DataManager를 통해 아이콘/등급을 표시
            equipUI[i].SetSlotView(item.id);
        }

        //일반 인벤 슬롯 UI 갱신
        for (int i = 0; i < generalUI.Length; i++)
        {
            ItemData item =
                PlayerManager.Instance.playerData.playerGeneralInven[i];

            generalUI[i].SetSlotView(item.id);
        }
    }

    //일반 인벤 슬롯 클릭 처리
    // - 가방 아이템 클릭 시 호출
    // - "장착" 가능한 아이템이면 아이템 팝업을 띄움
    private void OnClickGeneralSlot(int slot)
    {
        if (itemPopup == null) return;

        ItemData item =
            PlayerManager.Instance.playerData.playerGeneralInven[slot];

        //빈 슬롯이거나 NONE 아이템이면 무시
        if (item == null) return;
        if (item.id == 0) return;
        if (item.type == EnumData.EquipmentType.NONE) return;

        //가방 슬롯이므로 "장착 모드"로 팝업에 데이터 전달
        itemPopup.Bind(item, slot, true);

        //UIPopup.Open 호출
        itemPopup.Open();
    }

    //장착 슬롯 클릭 처리
    // - 이미 장착된 아이템 클릭 시 호출
    // - "해제" 가능한 아이템이면 아이템 팝업을 띄움
    private void OnClickEquipSlot(int slot)
    {
        if (itemPopup == null) return;

        ItemData item =
            PlayerManager.Instance.playerData.playerEquipInven[(EnumData.EquipmentType)slot];

        //빈 슬롯이거나 NONE 아이템이면 무시
        if (item == null) return;
        if (item.id == 0) return;
        if (item.type == EnumData.EquipmentType.NONE) return;

        //장착 슬롯이므로 "해제 모드"로 팝업에 데이터 전달
        itemPopup.Bind(item, slot, false);

        //UIPopup.Open 호출
        itemPopup.Open();
    }
}
