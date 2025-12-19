using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : ForTargeting, IDamageable
{
    [SerializeField] private GameObject[] itemPrefabs;

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
        Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)], transform.position, Quaternion.identity);
        ReturnPool();
    }
    private void ReturnPool()
    {
        PoolManager.Instance.ReturnPool(this);
    }
}
