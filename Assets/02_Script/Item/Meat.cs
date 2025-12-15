using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : ItemBase
{
    public override void Activate(Player player)
    {
        player.PlayerStat().playerCurrentHp += player.PlayerStat().playerMeatRestore;

        if (player.PlayerStat().playerCurrentHp > player.PlayerStat().playerMaxHp)
            player.PlayerStat().playerCurrentHp = player.PlayerStat().playerMaxHp;

        Destroy(gameObject);
    }
}
