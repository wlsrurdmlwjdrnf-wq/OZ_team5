using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected bool _IsDestroyOnLoad = false;
    protected static T _instance;

    public static T Instance
    {
        get { return _instance; }
    }

    protected void Awake()
    {
        Init();
    }

    // 초기화 함수
    protected virtual void Init()
    {
        if( _instance == null )
        {
            _instance = (T)this;

            if(_IsDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        Dispose();
    }

    // 삭제 작업시 추가 처리 작업 오버라이딩
    protected virtual void Dispose()
    {
        _instance = null;
    }
}
