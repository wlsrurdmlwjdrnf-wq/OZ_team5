using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//º¸·ù
public class GravityFieldSkill : MonoBehaviour
{
    [SerializeField] private float damageInterval;
    [SerializeField] private int damage;

    private List<ForTargeting> inRange = new List<ForTargeting>();
    private float tmpSpeed;
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
        if(collision.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            tmpSpeed = enemy.Speed;
            enemy.Speed *= 0.1f;
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
            if (collision.TryGetComponent<EnemyBase>(out EnemyBase enemy))
            {
                enemy.Speed = tmpSpeed;
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
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
