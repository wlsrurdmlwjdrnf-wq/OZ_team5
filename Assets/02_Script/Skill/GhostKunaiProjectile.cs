using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostKunaiProjectile : ProjectileBase
{
    protected override int Id { get; set; } //데이터 추가 필요
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
}
