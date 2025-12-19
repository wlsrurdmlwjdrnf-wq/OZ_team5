using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiProjectile : MonoBehaviour
{
    [SerializeField] private int dmg;
    [SerializeField] int speed;
    [SerializeField] int lifetime;

    private float spawntime;
    private Vector2 shootDirection;
    private bool hasHit;

    private void OnEnable()
    {
        hasHit = false;
        spawntime = Time.time;
    }
    private void Update()
    {
        transform.Translate(shootDirection * speed * Time.deltaTime);

        if (Time.time - spawntime >= lifetime)
        {
            PoolManager.Instance.ReturnPool(this);
        }
    }
    public void SetDirection(Vector2 dir)
    {
        shootDirection = dir.normalized;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;
        if(collision.gameObject.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            DamageTextManager.Instance.ShowDamage(dmg, enemy.transform.position);
        }
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(dmg);
            hasHit = true;
            PoolManager.Instance.ReturnPool(this);
        }
    }
}
