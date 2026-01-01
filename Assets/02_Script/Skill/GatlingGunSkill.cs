using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingGunSkill : SkillBase
{
    [SerializeField] ProjectileBase gatlingPjtPrefab;
    public override int Id { get; set; } = 20002;
    protected override void Awake()
    {
        base.Awake();
        Managers.Instance.Pool.CreatePool(gatlingPjtPrefab, 50);
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
            ProjectileBase pjt = Managers.Instance.Pool.GetFromPool(gatlingPjtPrefab);
            pjt.SetDirection(transform.position - transform.parent.position);
            pjt.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            yield return interval;
        }
    }
    public override void SkillLevelUp(){}
}
