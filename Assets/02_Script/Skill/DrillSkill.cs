using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillSkill : SkillBase
{
    public override int Id { get; set; } = 3004;
    [SerializeField] ProjectileBase drillPrefab;

    private WaitForSeconds smallInterval = new WaitForSeconds(0.5f);
    protected override void Awake()
    {
        base.Awake();
        Managers.Instance.Pool.CreatePool(drillPrefab, 30);
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

                ProjectileBase drill = Managers.Instance.Pool.GetFromPool(drillPrefab);
                drill.SetDirection(dir);
                drill.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                yield return smallInterval;
            }
            yield return interval;
        }
    }
    public override void SkillLevelUp()
    {
        StopAllCoroutines();
        count += 1;
        level += 1;
        drillPrefab.ProjectileStatUp();
        StartCoroutine(AttackCo(count));
    }
}
