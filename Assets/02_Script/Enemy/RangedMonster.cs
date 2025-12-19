using System.Collections;
using UnityEngine;

public class RangedMonster : EnemyBase
{
    [SerializeField] private EnemyProjectile smallPjt;
    [SerializeField] private float shootInterval;

    private WaitForSeconds attackInterval;
    private void Start()
    {
        attackInterval = new WaitForSeconds(shootInterval);
        StartCoroutine(ShootToPlayer());
    }
    private IEnumerator ShootToPlayer()
    {
        while (true)
        {
            Vector2 dir = player.position - transform.position;

            EnemyProjectile pjt = PoolManager.Instance.GetFromPool(smallPjt);
            pjt.SetDirection(dir);
            pjt.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

            yield return attackInterval;
        }
    }
}
