using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameClearPopupController : MonoBehaviour
{
    Player player;
    [SerializeField] TextMeshProUGUI killText;
    [SerializeField] TextMeshProUGUI goldText;

    private void OnEnable()
    {
        killText.text = $"{EnemyManager.Instance.enemyKillCount}";
        goldText.text = $"x{player.PlayerStat().playerGold}";
    }
}
