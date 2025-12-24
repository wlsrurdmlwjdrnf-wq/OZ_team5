using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingGunSkill : MonoBehaviour
{
    [SerializeField] float interval;
    [SerializeField] ProjectileBase gatlingPjtPrefab;
    private void Awake()
    {
        Managers.Instance.Pool.CreatePool(gatlingPjtPrefab, 30);
    }
    private void OnEnable()
    {
        StartCoroutine(AttackCo());
    }
    private IEnumerator AttackCo()
    {
        while (true)
        {
            ProjectileBase pjt = Managers.Instance.Pool.GetFromPool(gatlingPjtPrefab);
            pjt.SetDirection(Vector2.right);
            pjt.transform.SetPositionAndRotation(transform.position, transform.rotation);
            yield return new WaitForSeconds(interval);
        }
    }
}
