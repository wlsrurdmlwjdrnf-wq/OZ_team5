using TMPro;
using UnityEngine;
using UnityEngine.UI;

//배틀 중 항상 표시되는 HUD 숫자 관리
//레벨 / 경험치 / 골드 / 현재 생존 시간 표시
//최고 생존 시간은 기록만 하고 게임 종료 팝업에서 사용
public class BattleHUDNumbers : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI levelText; //플레이어 레벨 표시(5)
    [SerializeField] private TextMeshProUGUI expText;   //경험치 표시(30 / 100)
    [SerializeField] private TextMeshProUGUI goldText;  //골드 표시(1234)
    [SerializeField] private TextMeshProUGUI timeText;  //플레이 타임 표시(12:34)

    [Header("EXP Slider")]
    [SerializeField] private Slider expSlider; //경험치 슬라이더(0~1)

    private Player player;          //플레이어 참조
    private PlayerData playerData;  //플레이어 데이터
    private float bestSurvivalTime; //이번 플레이 최고 생존 시간

    private void Awake()
    {
        //씬에 Player는 하나이므로 1회 탐색
        player = FindObjectOfType<Player>();

        if (player != null)
            playerData = player.PlayerStat();

        //경험치 슬라이더 기본 세팅
        if (expSlider != null)
        {
            expSlider.minValue = 0f;
            expSlider.maxValue = 1f;
            expSlider.value = 0f;
        }
    }

    private void OnEnable()
    {
        //HUD 활성화 시 전체 갱신
        RefreshAll();
    }

    private void Update()
    {
        //플레이 타임은 매 프레임 갱신
        UpdateTime();
    }

    //외부에서 한 번에 전체 갱신할 때 호출
    public void RefreshAll()
    {
        UpdateLevel();
        UpdateExp();
        UpdateGold();
        UpdateTime();
    }

    //레벨 표시 갱신
    public void UpdateLevel()
    {
        if (levelText == null || playerData == null) return;
        levelText.text = $"{playerData.playerLevel}";
    }

    //경험치 텍스트 + 슬라이더 갱신
    public void UpdateExp()
    {
        if (playerData == null) return;

        float cur = playerData.playerCurExp;
        float max = playerData.playerMaxExp;

        if (expText != null)
            expText.text = $"{(int)cur} / {(int)max}";

        if (expSlider != null && max > 0f)
            expSlider.value = cur / max;
    }

    //골드 표시 갱신
    public void UpdateGold()
    {
        if (goldText == null || playerData == null) return;
        goldText.text = $"{(int)playerData.playerGold}";
    }

    //현재 생존 시간 표시 + 최고 생존 시간 기록
    private void UpdateTime()
    {
        if (timeText == null || GameManager.Instance == null) return;

        float time = GameManager.Instance.gamePlayTime;

        int min = Mathf.FloorToInt(time / 60f);
        int sec = Mathf.FloorToInt(time % 60f);

        timeText.text = $"{min:00}:{sec:00}";

        if (time > bestSurvivalTime)
            bestSurvivalTime = time;
    }

    //게임 종료 팝업에서 최고 생존 시간을 가져갈 때 사용
    public float GetBestSurvivalTime()
    {
        return bestSurvivalTime;
    }
}
