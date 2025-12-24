using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boss3 : EnemyBase
{
    [SerializeField] private EnemyProjectile glowPjt;
    [SerializeField] private float shootInterval;
    [SerializeField] private HpBar hpBarPrefab;

    private WaitForSeconds shooting;
    private WaitForSeconds angryShooting;
    private HpBar hpBar;

    private bool isAngry = false;

    private static readonly int isAngryHash = Animator.StringToHash("IsAngry");
    private void Start()
    {
        shooting = new WaitForSeconds(shootInterval);
        angryShooting = new WaitForSeconds(shootInterval * 0.5f);
        hpBar = Instantiate(hpBarPrefab);
        hpBar.Init(transform);
        UpdateHpBar();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        hpBar = Instantiate(hpBarPrefab);
        hpBar.Init(transform);
        UpdateHpBar();
        StartCoroutine(NormalPatternCo());
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (hpBar != null) Destroy(hpBar.gameObject);
    }
    private void UpdateHpBar()
    {
        hpBar.UpdateHp(hp, maxHp);
    }
    public override void TakeDamage(int amount)
    {
        hp -= amount;
        UpdateHpBar();
        if(hp <= maxHp / 2 && hp > 0 &&!isAngry)
        {
            isAngry = true;
            animator.SetBool(isAngryHash, isAngry);

            transform.localScale *= 1.3f;
            moveSpeed *= 2f;
            atk *= 2;
            StartCoroutine(AngryPatternCo());
        }
        if (hp <= 0)
        {
            isAngry = false;
            isKilled = true;
            animator.SetBool(isAngryHash, isAngry);
            animator.SetBool(isKilledHash, isKilled);
            StopCoroutine(AngryPatternCo());
            StartCoroutine(DieCo());
        }
    }
    private IEnumerator NormalPatternCo()
    {
        while (true)
        {
            Vector2 dir = player.position - transform.position;

            EnemyProjectile small = Managers.Instance.Pool.GetFromPool(glowPjt);
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
    private IEnumerator AngryPatternCo()
    {
        Vector2 dir;
        while (true)
        {
            float angleStep = 120f;
            for (int i = 0; i < 3; i++)
            {
                float angle = i * angleStep;
                dir = Quaternion.Euler(0, 0, angle) * Vector2.left;

                EnemyProjectile big = Managers.Instance.Pool.GetFromPool(glowPjt);
                big.SetDirection(dir);
                big.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            }
            yield return angryShooting;
        }
    }
    protected override IEnumerator DieCo()
    {
        GameManager.Instance.GameClear();
        yield return null;
        StartCoroutine(base.DieCo());
    }
}
