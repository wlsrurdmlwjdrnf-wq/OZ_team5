using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventoryController : MonoBehaviour
{
    protected virtual void Start() { LoadInven(); }
    protected virtual void OnDestroy() { }       
    protected abstract void LoadInven();
}
