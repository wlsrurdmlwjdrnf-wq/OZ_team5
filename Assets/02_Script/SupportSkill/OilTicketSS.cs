using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilTicketSS : SupportSkillBase
{
    public override int Id { get; set; } = 4003;

    protected override void Start()
    {
        base.Start();
        player.PlayerStat().playerGoldPt += 8;
    }
    public override void LevelUp()
    {
        level += 1;
        player.PlayerStat().playerGoldPt += 8;
    }
}
