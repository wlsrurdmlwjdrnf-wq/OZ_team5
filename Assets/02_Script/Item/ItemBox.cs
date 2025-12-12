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
        //풀매니저를 통한 리턴풀
        hp = 1;
    }
}
