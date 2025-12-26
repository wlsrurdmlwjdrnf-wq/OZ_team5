using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballSkill : SkillBase
{
    protected override int Id { get; set; } = 3003;
    [SerializeField] ProjectileBase footballPrefab;
    protected override void Awake()
    {
        base.Awake();
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
                yield return new WaitForSeconds(0.5f);
            }
            yield return interval;
        }
    }
    protected override void SkillLevelUp()
    {
        
    }
}
