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
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(ShootToPlayer());
    }
    private IEnumerator ShootToPlayer()
    {
        while (true)
        {
            Vector2 dir = player.position - transform.position;

            EnemyProjectile pjt = Managers.Instance.Pool.GetFromPool(smallPjt);
            if (pjt == null)
            {
                yield return null;
                continue;
            }
            pjt.SetDirection(dir);
            pjt.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

            yield return attackInterval;
        }
    }
}
