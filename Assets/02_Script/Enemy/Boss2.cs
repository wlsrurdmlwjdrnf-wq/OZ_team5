using System.Collections;
using UnityEngine;

public class Boss2 : EnemyBase
{
    [SerializeField] private HpBar hpBarPrefab;
    private bool isCharging = false;
    private WaitForSeconds chargetime;
    private WaitForSeconds jumptime;
    private HpBar hpBar;

    private void Start()
    {
        chargetime = new WaitForSeconds(1.5f);
        jumptime = new WaitForSeconds(0.5f);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        hpBar = Instantiate(hpBarPrefab);
        hpBar.Init(transform);
        UpdateHpBar();
        StartCoroutine(ChargeCo());
    }
    protected override void Update()
    {
        if(isCharging) base.Update();
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
            Destroy(hpBar.gameObject);
            isKilled = true;
            animator.SetBool(isKilledHash, isKilled);
            StartCoroutine(DieCo());
        }
    }
    private IEnumerator ChargeCo()
    {
        while (true)
        {
            yield return chargetime;
            isCharging = true;
            yield return jumptime;
            isCharging = false;
            transform.localScale *= 1.1f;
            atk += 2;
        }
    }
    protected override IEnumerator DieCo()
    {
        //타이머 다시 작동
        //벽몬스터 사라지고 이어서 플레이
        Spawner.Instance.KillBoss2();
        yield return null;
        StartCoroutine(base.DieCo());
    }
}
