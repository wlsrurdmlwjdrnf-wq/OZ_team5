using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpStone : ItemBase
{
    [SerializeField] private float amount;

    public override void Activate(Player player)
    {
        player.PlayerStat().playerCurExp += amount * ((100 + player.PlayerStat().playerExpPt) * 0.01f);
        if (player.PlayerStat().playerCurExp > player.PlayerStat().playerMaxExp)
        {
            //PlayerLevelUpSystem.LevelUp(player);
        }
        Managers.Instance.Pool.ReturnPool(this);
    }
}
