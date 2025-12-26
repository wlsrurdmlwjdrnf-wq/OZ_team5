using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSkill : SkillBase
{
    protected override int Id { get; set; } = 3001;

    private List<ForTargeting> inRange = new List<ForTargeting>();

    private void OnEnable()
    {
        StartCoroutine(DoDamageCo());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ForTargeting>(out ForTargeting target))
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
    private IEnumerator DoDamageCo()
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
    protected override void SkillLevelUp()
    {
        
    }
}
