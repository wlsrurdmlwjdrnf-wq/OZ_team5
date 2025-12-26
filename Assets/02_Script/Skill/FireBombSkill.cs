using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBombSkill : SkillBase
{
    protected override int Id { get; set; } = 3000;
    [SerializeField] ProjectileBase fireBombPrefab;
    [SerializeField] FireArea fireAreaPrefab;
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
                fb.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
            }
            yield return interval;
        }
    }
    protected override void SkillLevelUp()
    {
        
    }
}
