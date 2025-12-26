using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpStone : ItemBase
{
    [SerializeField] private float amount;

    public override void Activate(Player player)
    {
        player.PlayerStat().playerCurExp += amount;
        Managers.Instance.Pool.ReturnPool(this);
    }
}
