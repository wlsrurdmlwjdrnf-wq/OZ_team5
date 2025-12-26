using System;
using UnityEngine;
using UnityEngine.UI;

// 인벤토리 데이터
public abstract class BaseInventorySlotUI : MonoBehaviour
{    
    //슬롯번호만 지정해주면됨
    [SerializeField] protected Image icon; // 아이템의 sprite가 표시될 이미지
    [SerializeField] protected Image grade; // 아이템 등급 sprite
    [SerializeField] protected Button btn;
    [SerializeField] protected int slotNum; // Action용 슬롯 넘버

    //몇번째 슬롯이 눌렸는지 알림
    public event Action<int> OnClickEquipSlot;
    public event Action<int> OnClickGeneralSlot;
    public event Action<int> OnClickSkillSlot;
    public event Action<int> OnClickSupportSlot;

    protected virtual void Awake()
    {
        btn.onClick.AddListener(OnSlotClick);
    }

    public void SetSlotView(int id)
    {
        //처음 6번은 장비칸에서 부른 slotview

        if (DataManager.Instance.GetItemData(id) != null)
        {
            ItemData temp = DataManager.Instance.GetItemData(id);
            icon.sprite = DataManager.Instance.GetItemIcon(temp);            
            grade.sprite = DataManager.Instance.GetItemGrade(temp);
            icon.enabled = true;
            grade.enabled = true;
        }
        else if (DataManager.Instance.GetIngameItemData(id) != null)
        {
            IngameItemData temp = DataManager.Instance.GetIngameItemData(id);
            icon.sprite = DataManager.Instance.GetIngameItemIcon(temp);
            icon.enabled = true;
            grade.enabled = true;
        }
        else
        {
            icon.enabled = false;
            grade.enabled = false;
        }

    }
    public void SetSlotNumber(int num)
    {
        slotNum = num;
    }

    protected void CallOnClickEuip(int slotNum)
    {
        OnClickEquipSlot?.Invoke(slotNum);
    }
    protected void CallOnClickGeneral(int slotNum)
    {
        OnClickGeneralSlot?.Invoke(slotNum);
    }
    protected void CallOnClickSkill(int slotNum)
    {
        OnClickSkillSlot?.Invoke(slotNum);
    }
    protected void CallOnClickSupport(int slotNum)
    {
        OnClickSupportSlot?.Invoke(slotNum);
    }

    protected virtual void OnDestroy() { }

    protected abstract void OnSlotClick();
}

