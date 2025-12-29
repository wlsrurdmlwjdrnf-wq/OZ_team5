using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiProjectile : ProjectileBase
{
    public override int Id { get; set; } = 10001;
    private bool hasHit;

    protected override void OnEnable()
    {
        base.OnEnable();
        hasHit = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;
        if(collision.gameObject.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            DamageTextManager.Instance.ShowDamage(damage, enemy.transform.position);
        }
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(damage);
            hasHit = true;
            ReturnPool();
        }
    }
    public override void ProjectileStatUp()
    {
        damage += 1;
    }
}
