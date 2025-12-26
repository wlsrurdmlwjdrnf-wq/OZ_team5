using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUpSystem : MonoBehaviour
{ 
    public static void LevelUp(Player player)
    {
        player.PlayerStat().playerLevel++;
        player.PlayerStat().playerCurExp -= player.PlayerStat().playerMaxExp;
        player.PlayerStat().playerMaxExp = 100 + player.PlayerStat().playerLevel * 50;

        GameManager.Instance.LevelUp();

        // 스킬 선택 UI 띄우기
    }
}
