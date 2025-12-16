using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Awake()
    {
        if (target == null)
        {
            var p = FindObjectOfType<Player>();
            if (p != null) target = p.transform;
            else Debug.LogError("AreaFollow : Player를 찾지 못했습니다.");
        }
    }
    private void Start()
    {
        target = FindObjectOfType<Player>()?.transform;
    }
    private void FixedUpdate()
    {
        if (target == null) return;

        // 경계 판정용 Area는 "부드러움"보다 "정확함"이 중요
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
