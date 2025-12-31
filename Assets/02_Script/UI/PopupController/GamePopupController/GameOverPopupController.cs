using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopupController : UIPopup
{
    [SerializeField] TextMeshProUGUI chapterText;
    [SerializeField] TextMeshProUGUI playTime;
    [SerializeField] TextMeshProUGUI bestTime;
    [SerializeField] TextMeshProUGUI killText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] Player player;
    [SerializeField] Button okBtn;
    private void Start()
    {
        okBtn.onClick.AddListener(GotoLooby);
    }
    private void OnEnable()
    {
        //bestTime.text = $"{GameManager.Instance.}";
        playTime.text = $"{FormatTimeText(GameManager.Instance.gamePlayTime)}";
        killText.text = $"{EnemyManager.Instance.enemyKillCount}";
        goldText.text = $"{player.PlayerStat().playerGold}";

    }
    private void GotoLooby()
    {

    }
    
    private string FormatTimeText(float time)
    {
        int min = Mathf.FloorToInt(time / 60f);
        int sec = Mathf.FloorToInt(time % 60f);

        return $"{min:00}:{sec:00}";
    }


}
