using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpStone : ItemBase
{
    [SerializeField] private int amount;

    public override void Activate(Player player)
    {
        player.PlayerStat().playerExp += amount;
        PoolManager.Instance.ReturnPool(this);
    }
}
