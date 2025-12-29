using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostKunaiProjectile : ProjectileBase
{
    public override int Id { get; set; } = 20001;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            DamageTextManager.Instance.ShowDamage(damage, enemy.transform.position);
        }
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(damage);
        }
    }
    public override void ProjectileStatUp(){}
}
