using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArea : MonoBehaviour
{
    [SerializeField] private float damageInterval;
    [SerializeField] private int damage;
    [SerializeField] private float duration;

    private List<ForTargeting> inRange = new List<ForTargeting>();
    private WaitForSeconds interval;
    private WaitForSeconds lifetime;


    private void Awake()
    {
        interval = new WaitForSeconds(damageInterval);
        lifetime = new WaitForSeconds(duration);
    }
    private void OnEnable()
    {
        StartCoroutine(DoDamageCo());
        StartCoroutine(Duration());
        GameManager.Instance.OnGameClear += ReturnPool;
        GameManager.Instance.OnGameOver += ReturnPool;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        GameManager.Instance.OnGameClear -= ReturnPool;
        GameManager.Instance.OnGameOver -= ReturnPool;
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
    private IEnumerator Duration()
    {
        yield return lifetime;
        Managers.Instance.Pool.ReturnPool(this);
    }
    private void ReturnPool()
    {
        Managers.Instance.Pool.ReturnPool(this);
    }
}
