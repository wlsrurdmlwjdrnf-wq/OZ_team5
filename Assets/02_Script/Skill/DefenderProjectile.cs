using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderProjectile : ProjectileBase
{
    public override int Id { get; set; } = 3002;

    protected override void OnEnable() {}
    protected override void Update() {}

    protected override void OnTriggerEnter2D(Collider2D collision)
     {
        if (collision.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            DamageTextManager.Instance.ShowDamage(damage + player.PlayerStat().playerAtk, enemy.transform.position);
        }
        if (collision.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(damage + player.PlayerStat().playerAtk);
        }
        if(collision.TryGetComponent<EnemyProjectile>(out EnemyProjectile pjt))
        {
            Managers.Instance.Pool.ReturnPool(pjt);
        }
    }
    public override void ProjectileStatUp()
    {
        damage += 1;
    }
}
