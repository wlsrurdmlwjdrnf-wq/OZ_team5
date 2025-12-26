using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected int damage;
    protected int speed;
    protected float lifetime;

    protected IngameItemData skillData;
    protected abstract int Id { get; set; }

    protected Vector2 shootDirection;
    protected float spawntime;


    protected void Awake()
    {
        skillData = DataManager.Instance.GetIngameItemData(Id);
        damage = skillData.damage;
        speed = skillData.ptSpeed;
        lifetime = skillData.lifeTime;
    }
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
            DamageTextManager.Instance.ShowDamage(damage, enemy.transform.position);
        }
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(damage);
            ReturnPool();
        }
    }
    protected virtual void ReturnPool()
    {
        Managers.Instance.Pool.ReturnPool(this);
    }
}
