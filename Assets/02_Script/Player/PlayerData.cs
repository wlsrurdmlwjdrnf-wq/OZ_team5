using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int playerAtk;
    public int playerDef;
    public int playerMaxHp;
    public int playerCurrentHp;
    public int playerMeatRestore;
    public float playerSpeed;
    public float magnetRadius;
    public int playerLevel;
    public float playerMaxExp; // 최대 경험치
    public float playerCurExp; // 현재 경험치
    public float playerExpPt; // 경험치 획득량
    public float playerGoldPt; // 골드 획득량

    public float resultAtkPer; // (외부에 의한)최종 공격력 증가량 10% 20%
    public float resultAtkMtp; // (외부에 의한)최종 공격력 증가 배수 2배 3배
    public float resultGoldPt; // (외부에 의한)최종 골드 획득량 10% 20%

    // 게임시작시 장비아이템
    public List<int> playerEquipInven;
    // 게임시작시 인벤토리
    public List<int> playerGeneralInven;

    public PlayerData()
    {
        playerAtk = 20;
        playerDef = 0;
        playerMaxHp = 200;
        playerCurrentHp = 200;
        playerMeatRestore = 20;
        playerSpeed = 2.0f;
        magnetRadius = 0.4f;
        playerLevel = 1;
        playerMaxExp = 100;
        playerCurExp = 0;
        playerExpPt = 0;
        playerGoldPt = 0;


        playerGeneralInven = new List<int>();

        //장비칸 -1값 할당 (빈칸)
        playerEquipInven = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            playerEquipInven.Add(-1);
        }
    }


    //제거 해야함 datamanager에서 초기화 완료
    //기존에 getdefault는 playerData로 불러와야함
    public static PlayerData GetDefault()
    {
        return new PlayerData
        {
            playerAtk = 20,
            playerDef = 0,
            playerMaxHp = 200,
            playerMeatRestore = 20,
            playerSpeed = 2.0f,
            magnetRadius = 0.4f,
            playerLevel = 1,
            playerExpPt = 0,
            playerGoldPt = 0
        };
    }
}
