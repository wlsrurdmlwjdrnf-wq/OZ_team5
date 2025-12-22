using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 인벤토리 데이터
public abstract class InventorySlotUI : MonoBehaviour
{
    protected Image icon;
    [SerializeField] protected EnumData.InventoryType type;
    public int slotItemID { get; protected set; } = -1;


    public event Action<InventorySlotUI> OnSlotClick;

    protected virtual void SetItem(int id)
    {
        if (DataManager.Instance.GetItemData(id) != null)
        {
            slotItemID = id;
            var item = DataManager.Instance.GetItemData(id);
            icon.sprite = DataManager.Instance.GetItemIcon(item.name);
            icon.enabled = true;
        }
        else if (DataManager.Instance.GetIngameItemData(id) != null)
        {
            slotItemID = id;
            var item = DataManager.Instance.GetIngameItemData(id);
            icon.sprite = DataManager.Instance.GetItemIcon(item.name);
            icon.enabled = true;
        }
        else
        {
            slotItemID = -1;
            icon.sprite = DataManager.Instance.GetItemIcon("empty");
            icon.enabled = true;
        }
    }                
}

