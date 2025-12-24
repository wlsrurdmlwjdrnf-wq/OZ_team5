using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballSkill : MonoBehaviour
{
    [SerializeField] float interval;
    [SerializeField] int count;
    [SerializeField] ProjectileBase footballPrefab;
    private void Awake()
    {
        Managers.Instance.Pool.CreatePool(footballPrefab, 30);
    }
    private void OnEnable()
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
                if (enemy == null)
                {
                    yield return null;
                    continue;
                }
                Vector2 dir = enemy.transform.position - transform.position;

                ProjectileBase drill = Managers.Instance.Pool.GetFromPool(footballPrefab);
                drill.SetDirection(dir);
                drill.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                yield return new WaitForSeconds(interval * 0.2f);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
