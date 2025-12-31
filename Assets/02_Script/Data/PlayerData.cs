using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class PlayerData
{
    public float playerAtk;
    public float playerDef;
    public float playerMaxHp;
    public float playerCurrentHp;
    public float playerMeatRestore;
    public float playerGold;
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
    public float resultHpPer; // (외부에 의한)최종 체력 증가량 10% 20%


    // 게임시작시 인벤토리
    public List<ItemData> playerGeneralInven = new List<ItemData>();

    // 게임시작시 장비아이템
    public Dictionary<EnumData.EquipmentType, ItemData> playerEquipInven = new Dictionary<EnumData.EquipmentType, ItemData>();

    // 인게임 장비 리스트
    public List<IngameItemData> playerSkillInven = new List<IngameItemData>();
    public List<IngameItemData> playerSupportInven = new List<IngameItemData>();
    
    
    public PlayerData()
    {
        playerAtk = 10;
        playerDef = 0;
        playerMaxHp = 200;
        playerCurrentHp = 200;
        playerMeatRestore = 20;
        playerGold = 0;
        playerSpeed = 2.0f;
        magnetRadius = 0.4f;
        playerLevel = 1;
        playerMaxExp = 100;
        playerCurExp = 0;
        playerExpPt = 0;
        playerGoldPt = 0;

        resultAtkPer = 0;
        resultAtkMtp = 0;
        resultGoldPt = 0;
        resultHpPer = 0;
        
    }
    public void SetPlayerInven()
    {
        for (int i = 0; i < 6; i++)
        {
            ItemData item = DataManager.Instance.GetItemData(0);
            playerEquipInven[(EnumData.EquipmentType)i] = item;
        }
        // 플레이어 기본아이템 지급
        for (int i = 0; i < 50; i++)
        {
            if (i == 0)
            {
                ItemData item = DataManager.Instance.GetItemData(1010);
                playerGeneralInven.Add(item);
            }
            else if (i < 6)
            {
                int tempnum = 100 * i;
                ItemData item = DataManager.Instance.GetItemData(1900 + tempnum);
                playerGeneralInven.Add(item);
            }
            else
            {
                ItemData item = DataManager.Instance.GetItemData(0);
                playerGeneralInven.Add(item);
            }
        }
        for (int i = 0; i < 6; i++)
        {
            IngameItemData temp = DataManager.Instance.GetIngameItemData(0);
            playerSkillInven.Add(temp);
        }
        for (int i = 0; i < 6; i++)
        {
            IngameItemData temp = DataManager.Instance.GetIngameItemData(0);
            playerSupportInven.Add(temp);
        }
    }
    public void SetBaseData(PlayerData other)
    {
        playerAtk = other.playerAtk;
        playerDef = other.playerDef;
        playerMaxHp = other.playerMaxHp;
        playerCurrentHp = other.playerCurrentHp;
        playerMeatRestore = other.playerMeatRestore;        
        playerSpeed = other.playerSpeed;
        magnetRadius = other.magnetRadius;
        playerLevel = other.playerLevel;
        playerMaxExp = other.playerMaxExp;
        playerCurExp = other.playerCurExp;
        playerExpPt = other.playerExpPt;
        playerGoldPt = other.playerGoldPt;

        resultAtkPer = other.resultAtkPer;
        resultAtkMtp = other.resultAtkMtp;
        resultGoldPt = other.resultGoldPt;
        resultHpPer = other.resultHpPer;
    }
    public void SetBaseBattleInventory(PlayerData other)
    {
        playerSkillInven.Clear();
        playerSupportInven.Clear();

        playerSkillInven.AddRange(other.playerSkillInven);
        playerSupportInven.AddRange(other.playerSupportInven);
    }
}
