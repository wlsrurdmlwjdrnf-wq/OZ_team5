using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : ItemBase
{
    private bool superPulling = false;
   public override void Activate(Player player)
    {
        if(superPulling == false) StartCoroutine(MagnetForceSoar(player));
    }

    private IEnumerator MagnetForceSoar(Player player)
    {
        superPulling = true;
        player.PlayerStat().magnetRadius += 50f;

        yield return new WaitForSeconds(1.0f);

        player.PlayerStat().magnetRadius -= 50f;
        superPulling = false;
        Managers.Instance.Pool.ReturnPool(this);
    }
}
