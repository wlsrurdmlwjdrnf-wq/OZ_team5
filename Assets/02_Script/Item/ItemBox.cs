using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class ItemBox : ForTargeting, IDamageable
{
    [SerializeField] private ItemBase[] itemPrefabs;

    private int hp;
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
    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0) Broken();
    }
    private void Broken()
    {
        ItemBase tmpItem = PoolManager.Instance.GetFromPool(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
        tmpItem.transform.position = transform.position;
        ReturnPool();
    }
    private void ReturnPool()
    {
        PoolManager.Instance.ReturnPool(this);
    }
}
