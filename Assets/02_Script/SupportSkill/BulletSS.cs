using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSS : SupportSkillBase
{
    public override int Id { get; set; } = 4001;

    protected override void Start()
    {
        base.Start();
        player.PlayerStat().playerAtk *= 1.1f;
    }
    public override void LevelUp()
    {
        level += 1;
        player.PlayerStat().playerAtk *= 1.1f;
    }
}
