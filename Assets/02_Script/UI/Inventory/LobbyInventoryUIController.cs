using System;
using UnityEngine;

//로비 인벤토리 UI 전체를 관리하는 컨트롤러
// - 플레이어 인벤 데이터(PlayerManager.playerData)를 읽어서 UI에 뿌려줌
// - 슬롯 클릭 이벤트를 받아 "아이템 팝업 요청"만 보냄
// - 팝업을 실제로 여는 책임은 LobbyUIController가 담당
// - 실제 장착/해제 로직은 PlayerManager가 담당
// - 추가:상점 뽑기처럼 "OnItemUpdata가 안 도는 경로"에서도 인벤 UI를 즉시 갱신할 수 있게 ForceRefresh 제공
public class LobbyInventoryUIController : BaseInventoryUIController
{
    [SerializeField] private InventorySlotEquipUI[] equipUI;//장비 슬롯 UI 배열
    [SerializeField] private InventorySlotGeneralUI[] generalUI;//일반 인벤 슬롯 UI 배열

    //상위 컨트롤러에게 팝업 오픈을 요청하는 이벤트
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

    //외부(상점/가챠)에서 인벤 UI를 즉시 갱신시키고 싶을 때 호출
    //-예:상점에서 뽑기 결과가 인벤에 들어간 직후 바로 UI 갱신
    public void ForceRefresh()
    {
        //비활성 오브젝트면 갱신해도 안보이니까 방어(원하면 지워도 됨)
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("//LobbyInventoryUIController ForceRefresh ignored(not active)");
            return;
        }

        //BaseInventoryUIController의 LoadInven을 직접 호출해서 즉시 다시 그린다
        Debug.Log("//LobbyInventoryUIController ForceRefresh");
        LoadInven();
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
    private void BindSlotEvents()
    {
        //장착 슬롯 클릭 이벤트
        for (int i = 0; i < equipUI.Length; i++)
        {
            int idx = i;//람다 캡처 방지
            equipUI[i].OnClickEquipSlot += _ => OnClickEquipSlot(idx);
        }

        //일반 인벤 슬롯 클릭 이벤트
        for (int i = 0; i < generalUI.Length; i++)
        {
            int idx = i;//람다 캡처 방지
            generalUI[i].OnClickGeneralSlot += _ => OnClickGeneralSlot(idx);
        }
    }

    //인벤 UI 갱신 함수(실제 그리는 함수)
    protected override void LoadInven()
    {
        //PlayerManager 방어
        if (PlayerManager.Instance == null) return;
        if (PlayerManager.Instance.playerData == null) return;

        //장착 슬롯 UI 갱신
        for (int i = 0; i < equipUI.Length; i++)
        {
            ItemData item = PlayerManager.Instance.playerData.playerEquipInven[(EnumData.EquipmentType)i];
            equipUI[i].SetSlotView(item.id);
        }

        //일반 인벤 슬롯 UI 갱신
        for (int i = 0; i < generalUI.Length; i++)
        {
            ItemData item = PlayerManager.Instance.playerData.playerGeneralInven[i];
            generalUI[i].SetSlotView(item.id);
        }
    }

    //일반 인벤 슬롯 클릭 처리(가방)
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
        else
        {
            Debug.LogWarning("//LobbyInventoryUIController OnRequestItemPopup == null");
        }
    }
}
