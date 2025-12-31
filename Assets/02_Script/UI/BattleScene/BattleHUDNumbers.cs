using TMPro;
using UnityEngine;
using UnityEngine.UI;

//배틀 중 항상 표시되는 HUD 숫자 관리
//레벨 / 경험치(슬라이더) / 코인 / 처치 수 / 현재 생존 시간 표시
//최고 생존 시간은 내부에서 기록만 하고 게임 종료 팝업에서 사용
public class BattleHUDNumbers : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI levelText; //플레이어 레벨 표시(5)
    [SerializeField] private TextMeshProUGUI goldText;  //코인 표시(1234)
    [SerializeField] private TextMeshProUGUI killText;  //처치 수 표시(25)
    [SerializeField] private TextMeshProUGUI timeText;  //플레이 타임 표시(12:34)

    [Header("EXP")]
    [SerializeField] private Slider expSlider; //경험치 슬라이더(0~1)

    private Player player;          //씬에 존재하는 플레이어 참조
    private float bestSurvivalTime; //이번 플레이 최고 생존 시간 기록용
    private int killCount;          //이번 플레이 동안의 적 처치 수

    private void Awake()
    {
        //씬에 Player는 하나이므로 1회 탐색
        player = FindObjectOfType<Player>();
        //경험치 슬라이더 기본값 설정
        if (expSlider != null)
        {
            expSlider.minValue = 0f;
            expSlider.maxValue = 1f;
            expSlider.value = 0f;
        }
    }

    private void OnEnable()
    {
        //HUD가 활성화될 때 전체 UI를 한 번 갱신
        RefreshAll();
    }
    private void OnDestroy()
    {
        //게임매니저 플레이 타임 리셋
    }
    private void Update()
    {
        //생존 시간은 매 프레임 갱신
        //UpdateTime();
        RefreshAll();
    }

    //외부에서 한 번에 모든 HUD 값을 갱신하고 싶을 때 호출
    public void RefreshAll()
    {
        UpdateLevel();
        UpdateExp();
        UpdateGoldAndKill();
        UpdateTime();
    }

    //플레이어 레벨 표시 갱신
    public void UpdateLevel()
    {
        if (levelText == null)
        {
            return;
        }
        levelText.text = $"{player.PlayerStat().playerLevel}";
    }

    //경험치 슬라이더 갱신
    public void UpdateExp()
    {
        if (expSlider == null)
        {
            return;
        }

        float curExp = player.PlayerStat().playerCurExp;
        float maxExp = player.PlayerStat().playerMaxExp;

        if (maxExp <= 0f)
        {
            return;
        }

        //0~1 범위로 정규화하여 슬라이더에 반영
        expSlider.value = curExp / maxExp;
    }

    //코인과 처치 수는 항상 같이 표시되므로 한 함수에서 갱신
    public void UpdateGoldAndKill()
    {
        goldText.text = $"{player.PlayerStat().playerGold}";

        if (killText != null)
        {
            killText.text = $"{EnemyManager.Instance.enemyKillCount}";
        }
    }

    //현재 생존 시간 표시 + 최고 생존 시간 기록
    private void UpdateTime()
    {
        if (timeText == null)
        {
            return;
        }

        if (GameManager.Instance == null)
        {
            return;
        }

        float time = GameManager.Instance.gamePlayTime;

        int min = Mathf.FloorToInt(time / 60f);
        int sec = Mathf.FloorToInt(time % 60f);

        timeText.text = $"{min:00}:{sec:00}";

        //이번 플레이 중 가장 오래 생존한 시간 기록
        if (time > bestSurvivalTime)
        {
            bestSurvivalTime = time;
        }
    }

    //적 처치 시 호출
    public void AddKill(int add = 1)
    {
        killCount += add;
        UpdateGoldAndKill();
    }

    //코인 획득/변경 시 호출
    public void OnGoldChanged()
    {
        UpdateGoldAndKill();
    }

    //게임 종료 팝업에서 최고 생존 시간을 가져갈 때 사용
    public float GetBestSurvivalTime()
    {
        return bestSurvivalTime;
    }

    //게임 종료 팝업에서 처치 수를 가져갈 때 사용
    public int GetKillCount()
    {
        return killCount;
    }
}
