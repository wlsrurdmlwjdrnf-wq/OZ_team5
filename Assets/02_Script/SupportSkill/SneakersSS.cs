using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneakersSS : SupportSkillBase
{
    public override int Id { get; set; } = 4005;

    protected override void Start()
    {
        base.Start();
        player.PlayerStat().playerSpeed *= 1.1f;
    }
    public override void LevelUp()
    {
        level += 1;
        player.PlayerStat().playerSpeed *= 1.1f;
    }
}
