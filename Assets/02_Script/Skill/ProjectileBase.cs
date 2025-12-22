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

    protected virtual void OnEnable() 
    {
        StartCoroutine(Lifetime());
    }
    protected virtual void Update()
    {
        transform.Translate(speed * Time.deltaTime * shootDirection);
    }
    public void SetDirection(Vector2 dir)
    {
        shootDirection = dir.normalized;
    }
    protected IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);
        ReturnPool();
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
    protected void ReturnPool()
    {
        PoolManager.Instance.ReturnPool(this);
    }
}
