using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBombProjectile : ProjectileBase
{
    [SerializeField] private FireArea fireAreaPrefab;
    private bool hasHit;

    protected override void OnEnable()
    {
        base.OnEnable();
        hasHit = false;
    }
    protected override void ReturnPool()
    {
        FireArea fa = Managers.Instance.Pool.GetFromPool(fireAreaPrefab);
        fa.transform.position = transform.position;
        base.ReturnPool();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            FireArea fa = Managers.Instance.Pool.GetFromPool(fireAreaPrefab);
            fa.transform.position = transform.position;
            hasHit = true;
            ReturnPool();
        }
    }
}

