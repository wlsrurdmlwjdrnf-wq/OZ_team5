using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiSkill : SkillBase
{
    protected override int Id { get; set; } = 10001;
    [SerializeField] ProjectileBase kunaiPrefab;

    WaitForSeconds smallInterval = new WaitForSeconds(0.2f);

    protected override void Awake()
    {
        base.Awake();
        Managers.Instance.Pool.CreatePool(kunaiPrefab, 30);
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
                    CooldownBar.cooldownTime = 0f;
                    yield return null;
                    continue;
                }
                CooldownBar.cooldownTime = skillData.cooldown + 0.2f * (count-1);
                Vector2 dir = enemy.transform.position - transform.position;

                ProjectileBase kunai = Managers.Instance.Pool.GetFromPool(kunaiPrefab);
                kunai.SetDirection(dir);
                kunai.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                yield return smallInterval;
            }
            yield return interval;
        }
    }
    protected override void SkillLevelUp()
    {
        
    }
}

