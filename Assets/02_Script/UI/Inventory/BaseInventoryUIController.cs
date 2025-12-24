using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventoryUIController : MonoBehaviour
{

    protected virtual void Start() 
    {
        LoadInven();
        PlayerManager.Instance.OnItemUpdata += LoadInven;
    }

    protected abstract void LoadInven();
    protected abstract void SetSlotNum();
    protected virtual void OnDestroy() { }       

}
