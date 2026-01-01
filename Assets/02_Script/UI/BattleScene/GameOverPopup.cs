using TMPro;
using UnityEngine;

//게임오버 팝업
//- 활성화될 때(OnEnable) 결과 텍스트 갱신
//- 버튼 클릭(로비 이동)은 인스펙터에서 다른 스크립트로 처리
public class GameOverPopup : UIPopup
{
    [Header("Result Texts")]
    [SerializeField] private TextMeshProUGUI chapterText;   //현재 챕터(예: Chapter 3)
    [SerializeField] private TextMeshProUGUI endTimeText;   //게임 종료 시간(이번 판 생존 시간, 00:00)
    [SerializeField] private TextMeshProUGUI bestTimeText;  //최장 시간(00:00)
    [SerializeField] private TextMeshProUGUI killText;      //처치 수
    [SerializeField] private TextMeshProUGUI goldText;      //코인

    private BattleHUDNumbers hud;
    private PlayerData playerData;

    private int cachedChapter = -1;     //외부에서 SetChapter로 넣어준 값(없으면 -1)
    private float cachedEndTime = -1f;  //외부에서 SetEndTime로 넣어준 값(없으면 -1)

    //외부(게임 흐름 스크립트)에서 챕터를 넘겨줄 때 사용
    public void SetChapter(int chapter)
    {
        cachedChapter = chapter;
    }

    //외부(게임오버 순간)에서 종료 시간을 넘겨줄 때 사용
    public void SetEndTime(float timeSec)
    {
        cachedEndTime = timeSec;
    }

    private void OnEnable()
    {
        RefreshResult();
    }

    private void RefreshResult()
    {
        if (hud == null)
        {
            hud = FindObjectOfType<BattleHUDNumbers>();
        }

        if (playerData == null)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                playerData = player.PlayerStat();
            }
        }
        //챕터
        int chapter = GetChapterValue();
        if (chapterText != null)
        {
            chapterText.text = chapter > 0 ? $"{chapter}" : "-";
        }
        //생존시간
        float endTime = GetEndTimeValue();
        if (endTimeText != null)
        {
            endTimeText.text = FormatTime(endTime);
        }
        //최장 생존시간
        float bestTime = 0f;
        if (hud != null)
        {
            bestTime = hud.GetBestSurvivalTime();
        }
        if (bestTimeText != null)
        {
            bestTimeText.text = FormatTime(bestTime);
        }
        //골드
        int gold = 0;
        if (playerData != null)
        {
            gold = (int)playerData.playerGold;
        }
        if (goldText != null)
        {
            goldText.text = $"{gold}";
        }
        //킬수
        int killCount = 0;
        if (hud != null)
        {
            killCount = hud.GetKillCount();
        }
        if (killText != null)
        {
            killText.text = $"{killCount}";
        }
    }

    //챕터 값은 외부 주입이 있으면 우선 사용
    private int GetChapterValue()
    {
        if (cachedChapter > 0)
        {
            return cachedChapter;
        }

        //프로젝트에 챕터를 들고 있는 매니저가 있다면 여기서 가져오도록 확장 가능
        //예: if (StageManager.Instance != null) { return StageManager.Instance.CurrentChapter; }

        return -1;
    }

    //종료 시간은 외부 주입이 있으면 우선 사용, 없으면 GameManager 시간 사용
    private float GetEndTimeValue()
    {
        if (cachedEndTime >= 0f)
        {
            return cachedEndTime;
        }

        if (GameManager.Instance != null)
        {
            return GameManager.Instance.gamePlayTime;
        }

        if (hud != null)
        {
            return hud.GetBestSurvivalTime();
        }

        return 0f;
    }

    private string FormatTime(float timeSec)
    {
        if (timeSec < 0f)
        {
            timeSec = 0f;
        }

        int min = Mathf.FloorToInt(timeSec / 60f);
        int sec = Mathf.FloorToInt(timeSec % 60f);

        return $"{min:00}:{sec:00}";
    }
}
