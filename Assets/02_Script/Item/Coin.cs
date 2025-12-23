using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : ItemBase
{
    [SerializeField] private int amount;

    public override void Activate(Player player)
    {
        //player.PlayerStat().playerGold += amount;
        Managers.Pool.ReturnPool(this);
    }

    //게임클리어나 오버 시 모은 골드 영구적으로 반영해야 함
}
