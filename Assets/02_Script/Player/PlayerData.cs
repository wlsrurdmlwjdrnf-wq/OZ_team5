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
    public int playerExp;
    public int playerGold;

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
            playerExp = 0,
            playerGold = 0
        };
    }
}
