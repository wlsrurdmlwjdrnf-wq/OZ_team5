using System;
using UnityEngine;

//로비 인벤토리 UI 전체를 관리하는 컨트롤러
// - 플레이어 인벤 데이터(PlayerManager.playerData)를 읽어서 UI에 뿌려줌
// - 슬롯 클릭 이벤트를 받아 "아이템 팝업 요청"만 보냄
// - 팝업을 실제로 여는 책임은 LobbyUIController가 담당
// - 실제 장착/해제 로직은 PlayerManager가 담당
public class LobbyInventoryUIController : BaseInventoryUIController
{
    [SerializeField] private InventorySlotEquipUI[] equipUI;    //장비 슬롯 UI 배열(무기, 방어구 등)
    [SerializeField] private InventorySlotGeneralUI[] generalUI;//일반 인벤 슬롯 UI 배열(가방)

    //-변경:상위 컨트롤러에게 팝업 오픈을 요청하는 이벤트
    //item:클릭한 아이템, slot:슬롯 번호, isEquipMode:true=가방(장착), false=장착슬롯(해제)
    public event Action<ItemData, int, bool> OnRequestItemPopup;

    protected override void Start()
    {
        //BaseInventoryUIController에서 플레이어 이벤트 구독, 초기 인벤 UI 로드
        base.Start();

        //각 슬롯에 "몇번 슬롯인지" 번호를 설정
        SetSlotNum();

        //슬롯 클릭 이벤트를 컨트롤러에서 직접 받기 위해 바인딩
        BindSlotEvents();
    }

    //슬롯 번호 설정
    //UI 배열의 인덱스를 실제 인벤 데이터 인덱스로 사용
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

    //일반 인벤 슬롯 클릭 처리(가방)
    //-변경:여기서 itemPopup.Open()을 하지 않고, 이벤트로 요청만 보냄
    private void OnClickGeneralSlot(int slot)
    {
        if (PlayerManager.Instance == null) return;
        if (PlayerManager.Instance.playerData == null) return;

        ItemData item = PlayerManager.Instance.playerData.playerGeneralInven[slot];

        if (item == null) return;
        if (item.id == 0) return;
        if (item.type == EnumData.EquipmentType.NONE) return;

        if (OnRequestItemPopup != null)
        {
            Debug.Log($"//LobbyInventoryUIController Request ItemPopup General slot:{slot} id:{item.id}");
            OnRequestItemPopup.Invoke(item, slot, true);
        }
    }

    //장착 슬롯 클릭 처리(장비칸)
    //-변경:여기서 itemPopup.Open()을 하지 않고, 이벤트로 요청만 보냄
    private void OnClickEquipSlot(int slot)
    {
        if (PlayerManager.Instance == null) return;
        if (PlayerManager.Instance.playerData == null) return;

        ItemData item = PlayerManager.Instance.playerData.playerEquipInven[(EnumData.EquipmentType)slot];

        if (item == null) return;
        if (item.id == 0) return;
        if (item.type == EnumData.EquipmentType.NONE) return;

        if (OnRequestItemPopup != null)
        {
            Debug.Log($"//LobbyInventoryUIController Request ItemPopup Equip slot:{slot} id:{item.id}");
            OnRequestItemPopup.Invoke(item, slot, false);
        }
    }
}