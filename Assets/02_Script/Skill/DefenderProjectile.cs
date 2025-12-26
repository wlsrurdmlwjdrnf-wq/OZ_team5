using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//나중에 수정 필요
public class DefenderProjectile : MonoBehaviour
{
    [SerializeField] private int damage;
     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            DamageTextManager.Instance.ShowDamage(damage, enemy.transform.position);
        }
        if (collision.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(damage);
        }
        if(collision.TryGetComponent<EnemyProjectile>(out EnemyProjectile pjt))
        {
            Managers.Instance.Pool.ReturnPool(pjt);
        }
    }
}
