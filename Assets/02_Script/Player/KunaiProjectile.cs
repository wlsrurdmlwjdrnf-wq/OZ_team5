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

    private void OnEnable()
    {
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
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(dmg);
            PoolManager.Instance.ReturnPool(this);
        }
    }
}
