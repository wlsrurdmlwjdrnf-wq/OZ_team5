using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    public static Managers Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("@Managers");
                instance = obj.AddComponent<Managers>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    private PoolManager _pool;
    public PoolManager Pool
    {
        get
        {
            if (_pool == null)
            {
                GameObject obj = new GameObject("PoolManager");
                _pool = obj.AddComponent<PoolManager>();
                obj.transform.SetParent(transform);
            }
            return _pool;
        }
    }
}