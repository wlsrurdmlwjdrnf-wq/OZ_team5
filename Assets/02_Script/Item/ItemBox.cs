using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemBox : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject[] itemPrefabs;

    private int hp = 1;
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
        hp = 1;
    }
}
