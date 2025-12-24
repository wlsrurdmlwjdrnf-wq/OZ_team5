using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostKunaiSkill : MonoBehaviour
{
    //공격속도가 매우 빨라지고 공격이 적을 관통한다
    [SerializeField] float interval;
    [SerializeField] ProjectileBase ghostKunaiPrefab;
    private void Awake()
    {
        Managers.Instance.Pool.CreatePool(ghostKunaiPrefab, 30);
    }
    private void OnEnable()
    {
        StartCoroutine(AttackCo());
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
            kunai.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }
}
