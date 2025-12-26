using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class ItemBox : ForTargeting, IDamageable
{
    [SerializeField] private ItemBase[] itemPrefabs;

    private float hp;
    private void OnEnable()
    {
        hp = 1;
        if (EnemyManager.Instance != null && EnemyManager.Instance.enemies != null)
        {
            EnemyManager.Instance.enemies.Add(this);
        }
    }
    private void OnDisable()
    {
        if (EnemyManager.Instance != null && EnemyManager.Instance.enemies != null)
        {
            EnemyManager.Instance.enemies.Remove(this);
        }
    }
    public override void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0) Broken();
    }
    private void Broken()
    {
        ItemBase tmpItem = Managers.Instance.Pool.GetFromPool(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
        tmpItem.transform.position = transform.position;
        ReturnPool();
    }
    private void ReturnPool()
    {
        Managers.Instance.Pool.ReturnPool(this);
    }
}
