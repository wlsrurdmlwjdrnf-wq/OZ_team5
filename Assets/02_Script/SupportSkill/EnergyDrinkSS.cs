using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrinkSS : SupportSkillBase
{
    public override int Id { get; set; } = 4004;
    private WaitForSeconds hpRestoreInterval = new WaitForSeconds(5f);
    private float restoreAmount = 0.01f;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(HpRestoreCo(player));
    }
    public override void LevelUp()
    {
        StopCoroutine(HpRestoreCo(player));
        level += 1;
        restoreAmount += 0.01f;
        StartCoroutine(HpRestoreCo(player));
    }
    private IEnumerator HpRestoreCo(Player player)
    {
        while (true)
        {
            yield return hpRestoreInterval;
            if(player.PlayerStat().playerCurrentHp < player.PlayerStat().playerMaxHp)
            {
                player.PlayerStat().playerCurrentHp += player.PlayerStat().playerMaxHp * restoreAmount;
                if (player.PlayerStat().playerCurrentHp >= player.PlayerStat().playerMaxHp)
                {
                    player.PlayerStat().playerCurrentHp = player.PlayerStat().playerMaxHp;
                }
            }
        }
    }
}
