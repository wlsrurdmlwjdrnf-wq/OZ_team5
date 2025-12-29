using System.Collections;
using UnityEngine;

public class SuicideBomber : EnemyBase
{
    protected override void FixedUpdate()
    {
        if (Vector2.Distance(player.position, transform.position) <= 0.3f && isKilled == false)
        { 
            StartCoroutine(Boom()); 
        }
        base.FixedUpdate();
    }
    private IEnumerator Boom()
    {
        isKilled = true;
        animator.SetBool(isKilledHash, isKilled);
        atk *= 2;
        yield return new WaitForSeconds(0.4f);
        var tmpStone = Managers.Instance.Pool.GetFromPool(expStone);
        tmpStone.transform.position = transform.position;
        ReturnPool();
    }
}
