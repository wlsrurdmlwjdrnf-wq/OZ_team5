using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingGunProjectile : ProjectileBase
{
    public override int Id { get; set; } = 20002;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            DamageTextManager.Instance.ShowDamage(damage + player.PlayerStat().playerAtk, enemy.transform.position);
        }
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(damage + player.PlayerStat().playerAtk);
            ReturnPool();
        }
    }

    public override void ProjectileStatUp() { }
}
