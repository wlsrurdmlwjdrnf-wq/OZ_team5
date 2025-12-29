using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBombSkill : SkillBase
{
    public override int Id { get; set; } = 3000;
    [SerializeField] ProjectileBase fireBombPrefab;
    [SerializeField] FireArea fireAreaPrefab;

    private WaitForSeconds smallInterval = new WaitForSeconds(0.2f);
    protected override void Awake()
    {
        base.Awake();
        Managers.Instance.Pool.CreatePool(fireBombPrefab, 30);
        Managers.Instance.Pool.CreatePool(fireAreaPrefab, 30);
    }
    private void OnEnable()
    {
        StartCoroutine(AttackCo(count));
    }
    private IEnumerator AttackCo(int count)
    {
        while (true)
        {
            float angleStep = 360f / count;
            for (int i = 0; i < count; i++)
            {
                float angle = i * angleStep;
                Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

                ProjectileBase fb = Managers.Instance.Pool.GetFromPool(fireBombPrefab);
                fb.SetDirection(dir);
                fb.transform.SetPositionAndRotation(transform.position, fb.transform.rotation);
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
        fireBombPrefab.ProjectileStatUp();
        StartCoroutine(AttackCo(count));
    }
}
