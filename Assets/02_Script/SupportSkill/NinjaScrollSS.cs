using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaScrollSS : SupportSkillBase
{
    public override int Id { get; set; } = 4002;

    protected override void Start()
    {
        base.Start();
        player.PlayerStat().playerExpPt += 8;
    }
    public override void LevelUp()
    {
        level += 1;
        player.PlayerStat().playerExpPt += 8;
    }
}
