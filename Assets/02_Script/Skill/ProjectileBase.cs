using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    //스킬데이터 받을 곳
    [SerializeField] protected int dmg;
    [SerializeField] protected int speed;
    [SerializeField] protected float lifetime;

    protected Vector2 shootDirection;
    protected float spawntime;

    protected virtual void OnEnable() 
    {
        spawntime = Time.time;
    }
    protected virtual void Update()
    {
        transform.Translate(speed * Time.deltaTime * shootDirection);

        if(Time.time -  spawntime >= lifetime) ReturnPool();
    }
    public void SetDirection(Vector2 dir)
    {
        shootDirection = dir.normalized;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            DamageTextManager.Instance.ShowDamage(dmg, enemy.transform.position);
        }
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(dmg);
            ReturnPool();
        }
    }
    protected virtual void ReturnPool()
    {
        Managers.Instance.Pool.ReturnPool(this);
    }
}
