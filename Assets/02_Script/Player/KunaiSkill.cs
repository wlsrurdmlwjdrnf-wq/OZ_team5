using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiSkill : MonoBehaviour
{
    [Header("юс╫ц")]
    [SerializeField] float interval;
    [SerializeField] int level;
    [SerializeField] int count;
    [SerializeField] KunaiProjectile kunaiPrefab;
    private void Awake()
    {
        PoolManager.Instance.CreatePool(kunaiPrefab, 30);
    }
    private void Start()
    {
        StartCoroutine(AttackCo(count));
    }
    private IEnumerator AttackCo(int count)
    {
        while (true)
        {
            for (int i = 0; i < count; i++)
            {
                ForTargeting enemy = EnemyManager.Instance.GetClosestEnemy(transform.position);
                if(enemy == null) continue;
                Vector2 dir = enemy.transform.position - transform.position;

                KunaiProjectile kunai = PoolManager.Instance.GetFromPool(kunaiPrefab);
                kunai.SetDirection(dir);
                kunai.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                yield return new WaitForSeconds(interval*0.1f);
            }
            yield return new WaitForSeconds(interval);
        }
    }
    public void LevelUp()
    {
        level++;
        count++;
    }
}

