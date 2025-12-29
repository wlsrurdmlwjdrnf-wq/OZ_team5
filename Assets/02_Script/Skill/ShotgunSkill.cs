using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunSkill : SkillBase
{
    public override int Id { get; set; } = 10002;

    private List<ForTargeting> inRange = new List<ForTargeting>();

    private void OnEnable()
    {
        StartCoroutine(ShotCo());
        CooldownBar.cooldownTime = skillData.cooldown;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<ForTargeting>(out ForTargeting target))
        {
            if (target != null && !inRange.Contains(target))
            {
                inRange.Add(target);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ForTargeting>(out ForTargeting target))
        {
            if (target != null && inRange.Contains(target))
            {
                inRange.Remove(target);
            }
        }
    }
    private IEnumerator ShotCo()
    {
        while (true)
        {
            inRange.RemoveAll(t =>
                t == null || !((MonoBehaviour)t).gameObject.activeInHierarchy);

            for (int i = 0; i < inRange.Count; i++)
            {
                DamageTextManager.Instance.ShowDamage(damage, inRange[i].transform.position);
                inRange[i].TakeDamage(damage);                
            }
            yield return interval;
        }
    }
    public override void SkillLevelUp()
    {
        StopAllCoroutines();
        level += 1;
        damage += 5;
        StartCoroutine(ShotCo());
    }
}
