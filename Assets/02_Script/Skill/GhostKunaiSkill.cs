using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostKunaiSkill : SkillBase
{
    [SerializeField] ProjectileBase ghostKunaiPrefab;

    public override int Id { get; set; } = 20001;
    protected override void Awake()
    {
        base.Awake();
        Managers.Instance.Pool.CreatePool(ghostKunaiPrefab, 30);
    }
    private void OnEnable()
    {
        StartCoroutine(AttackCo());
        CooldownBar.timer = 0f;
        CooldownBar.cooldownTime = 0f;
    }
    private IEnumerator AttackCo()
    {
        while (true)
        {
            ForTargeting enemy = EnemyManager.Instance.GetClosestEnemy(transform.position);
            if (enemy == null)
            {
                yield return null;
                continue;
            }
            Vector2 dir = enemy.transform.position - transform.position;

            ProjectileBase kunai = Managers.Instance.Pool.GetFromPool(ghostKunaiPrefab);
            kunai.SetDirection(dir);
            kunai.transform.SetPositionAndRotation(transform.position, kunai.transform.rotation);
            yield return interval;
        }
    }
    public override void SkillLevelUp(){}
}
