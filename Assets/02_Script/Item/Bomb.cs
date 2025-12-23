using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ItemBase
{
    public override void Activate(Player player)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if(enemy.TryGetComponent<EnemyBase>(out EnemyBase em))
            {
                em.TakeDamage(999999);
                Managers.Pool.ReturnPool(this);
            }
        }
    }
}
