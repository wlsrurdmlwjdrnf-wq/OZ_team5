using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 전반적인 게임흐름을 관리하는 매니저입니다

/***중요*** [UI 애니메이션 관련]
 
 게임 일시정지시 timeScale이 0으로 설정하여 관리합니다
 게임이 정지되었을때 실행되는 UI의 애니메이션은 멈추면 안되기때문에
 해당 UI의 animator컴포넌트 inspector창에서 Update Mode를 [Normal] -> [Unscaled Time]으로 변경해주세요
 UI만 해당하는 사항이며 다른 오브젝트들은 따로 설정하실 필요 없습니다

*/

public class GameManager : Singleton<GameManager>
{

    public event Action OnGameStart; // 게임시작시 호출 할 함수
    public event Action OnGameOver; // 게임오버시 호출 할 함수
    public event Action OnGameClear; // 스테이지 클리어시 호출 할 함수
    public event Action OnGamePause; // 게임일시정지시 호출 할 함수
    public event Action OnGameResume; // 게임재개시 호출 할 함수
    public event Action OnLevelUp;  // 레벨업시 호출 할 함수
    public event Action OnBossSpawn; //보스스폰시 호출 할 함수


    public bool isPlay { get; private set; } // 게임이 진행중인지 확인 변수
    public float gamePlayTime { get; private set; } // 게임 진행시간

    [SerializeField]
    public float middleBossSpawnTime = 600.0f; // 준보스 등장시간 (10분)
    [SerializeField]
    public float finalBossSpawnTime = 900.0f; // 최종보스 등장시간 (15분)

    public bool isMiddleBoss { get; private set; } = false;
    public bool isFinalBoss { get; private set; } = false;
    
    /// <summary>
    /// 승문추가
    ///(플레이어가 이동하는거에 따라 맵을 재생성할거라 플레이어 정보를 받아야함)
    public static GameManager instance;
    public Player player;
    void Awake()
    {
        instance = this;

    }
    /// </summary>

    protected override void Init()
    {
        base.Init();
        isPlay = false;
        gamePlayTime = 0f;
    }

    private void Update()
    {
        if (!isPlay) return;
        
        gamePlayTime += Time.deltaTime;
        CheckBossSpawn();                
    }


    //게임시작 함수
    //OnGameStart에 구독된 함수들이 모두 실행됩니다
    public void GameStart()
    {        
        isPlay = true;
        OnGameStart?.Invoke();
    }

    //게임오버 함수
    //OnGameOver에 구독된 함수들이 모두 실행됩니다
    public void GameOver()
    {
        Time.timeScale = 0f;
        isPlay = false;
        OnGameOver?.Invoke();
    }

    //게임클리어 함수
    //OnGameClear에 구독된 함수들이 모두 실행됩니다
    public void GameClear()
    {
        Time.timeScale = 0f;
        isPlay = false;
        OnGameClear?.Invoke();
    }

    //게임일시정지 함수
    //OnGamePause에 구독된 함수들이 모두 실행됩니다
    public void GamePause()
    {
        Time.timeScale = 0f;
        isPlay = false;
        OnGamePause?.Invoke();
    }

    //게임재개 함수
    //OnGameResume에 구독된 함수들이 모두 실행됩니다
    public void GameResume()
    {
        Time.timeScale = 1f;
        isPlay = true;
        OnGameResume?.Invoke();
    }

    //레벨업 함수
    //OnLevelUp에 구독된 함수들이 모두 실행됩니다
    public void LevelUp()
    {
        Time.timeScale = 0f;
        isPlay = false;
        OnLevelUp?.Invoke();
    }

    //보스스폰 함수
    //OnBossSpawn에 구독된 함수들이 모두 실행됩니다
    public void BossSpawn()
    {
        OnBossSpawn?.Invoke();
    }

    //몹스포너 스크립트에서 gamePlayTime으로 중간보스 최종보스 구분 해주시면 됩니다
    public void CheckBossSpawn()
    {
        if(!isMiddleBoss && gamePlayTime >= middleBossSpawnTime)
        {
            isMiddleBoss = true;
            BossSpawn();
        }

        if(!isFinalBoss && gamePlayTime >= finalBossSpawnTime)
        {
            isFinalBoss = true;
            BossSpawn();
        }
    }
}
