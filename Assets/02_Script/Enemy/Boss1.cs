using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1 : EnemyBase
{
    [SerializeField] private EnemyProjectile smallPjt;
    [SerializeField] private EnemyProjectile bigPjt;
    [SerializeField] private HpBar hpBarPrefab;

    [SerializeField] private float pattern1Interval;
    [SerializeField] private float pattern2Interval;
    [SerializeField] private float shootInterval;

    private WaitForSeconds shootPattern1;
    private WaitForSeconds shootPattern2;
    private WaitForSeconds shooting;
    private HpBar hpBar;

    private void Start()
    {
        shootPattern1 = new WaitForSeconds(pattern1Interval);
        shootPattern2 = new WaitForSeconds(pattern2Interval);
        shooting = new WaitForSeconds(shootInterval);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        hpBar = Instantiate(hpBarPrefab);
        hpBar.Init(transform);
        UpdateHpBar();
        StartCoroutine(ShootingCo());
        StartCoroutine(PatternCo());
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if(hpBar != null) Destroy(hpBar.gameObject);
    }
    private void UpdateHpBar()
    {
        hpBar.UpdateHp(hp, maxHp);
    }
    public override void TakeDamage(float amount)
    {
        hp -= amount;
        UpdateHpBar();
        if (hp <= 0)
        {
            isKilled = true;
            animator.SetBool(isKilledHash, isKilled);
            StartCoroutine(DieCo());
        }
    }
    private IEnumerator ShootingCo()
    {
        while (true)
        {
            Vector2 dir = player.position - transform.position;

            EnemyProjectile small = Managers.Instance.Pool.GetFromPool(smallPjt);
            if (small == null)
            {
                yield return null;
                continue;
            }
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

                EnemyProjectile big = Managers.Instance.Pool.GetFromPool(bigPjt);
                big.SetDirection(dir);
                big.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            }
            yield return shootPattern2;

            for (int i = 0; i < 5; i++) 
            { 
                dir = player.position - transform.position;
               
                EnemyProjectile big2 = Managers.Instance.Pool.GetFromPool(bigPjt);
                big2.SetDirection(dir);
                big2.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
    protected override IEnumerator DieCo()
    {
        //타이머 다시 작동
        //벽몬스터 사라지고 이어서 플레이
        Spawner.Instance.KillBoss1();
        yield return null;
        StartCoroutine(base.DieCo());
    }
}
