using System.Collections;
using UnityEngine;

public class Boss1 : EnemyBase
{
    [SerializeField] private EnemyProjectile smallPjt;
    [SerializeField] private EnemyProjectile bigPjt;

    [SerializeField] private float pattern1Interval;
    [SerializeField] private float pattern2Interval;
    [SerializeField] private float shootInterval;

    private WaitForSeconds shootPattern1;
    private WaitForSeconds shootPattern2;
    private WaitForSeconds shooting;

    private void Start()
    {
        shootPattern1 = new WaitForSeconds(pattern1Interval);
        shootPattern2 = new WaitForSeconds(pattern2Interval);
        shooting = new WaitForSeconds(shootInterval);
        StartCoroutine(ShootingCo());
        StartCoroutine(PatternCo());
    }
    private IEnumerator ShootingCo()
    {
        while (true)
        {
            Vector2 dir = player.position - transform.position;

            EnemyProjectile small = PoolManager.Instance.GetFromPool(smallPjt);
            small.SetDirection(dir);
            small.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

            yield return shooting;
        }
    }
    private IEnumerator PatternCo()
    {
        while (true)
        {
            Vector2 dir;
            yield return shootPattern1;

            float angleStep = 18f;
            for (int i = 0; i < 20; i++)
            {
                float angle = i * angleStep;
                dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

                EnemyProjectile big = PoolManager.Instance.GetFromPool(bigPjt);
                big.SetDirection(dir);
                big.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            }
            yield return shootPattern2;

            for (int i = 0; i < 5; i++) 
            { 
                dir = player.position - transform.position;
               
                EnemyProjectile big2 = PoolManager.Instance.GetFromPool(bigPjt);
                big2.SetDirection(dir);
                big2.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
