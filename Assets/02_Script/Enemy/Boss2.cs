using System.Collections;
using UnityEngine;

public class Boss2 : EnemyBase
{
    private bool isCharging = false;
    private WaitForSeconds chargetime;
    private WaitForSeconds jumptime;

    private void Start()
    {
        chargetime = new WaitForSeconds(1.5f);
        jumptime = new WaitForSeconds(0.5f);
        StartCoroutine(ChargeCo());
    }
    protected override void Update()
    {
        if(isCharging) base.Update();
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
