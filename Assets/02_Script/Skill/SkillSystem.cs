using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : Singleton<SkillSystem>
{
    [SerializeField] private Player player;

    [SerializeField] private SkillBase kunai;
    [SerializeField] private SkillBase shotgun;
    [SerializeField] private SkillBase fireBomb;
    [SerializeField] private SkillBase barrier;
    [SerializeField] private SkillBase defender;
    [SerializeField] private SkillBase football;
    [SerializeField] private SkillBase drillShot;
    [SerializeField] private SkillBase ghostKunai;
    [SerializeField] private SkillBase gatlingGun;

    [SerializeField] private SupportSkillBase bulletSS;
    [SerializeField] private SupportSkillBase ninjaScrollSS;
    [SerializeField] private SupportSkillBase oilTicketSS;
    [SerializeField] private SupportSkillBase energyDrinkSS;
    [SerializeField] private SupportSkillBase sneakersSS;

    [SerializeField] int testNumber;

    private int invenItemCount = 1;
    protected override void Init()
    {
        _IsDestroyOnLoad = false;
        base.Init();
    }
    
    //스킬선택(고유ID)
    public void SelectSkill(int id)
    {
        switch (id)
        {
            case 3000:
                if (!fireBomb.gameObject.activeSelf)
                {
                    fireBomb.gameObject.SetActive(true);
                    AddIngameItem(id);
                }
                else
                {
                    fireBomb.SkillLevelUp();
                    LevelUpIngameItem(id);            
                }
                break;
            case 3001:
                if (!barrier.gameObject.activeSelf)
                {
                    barrier.gameObject.SetActive(true);
                    AddIngameItem(id);
                }
                else
                {
                    barrier.SkillLevelUp();
                    LevelUpIngameItem(id);
                }
                break;
            case 3002:
                if (!defender.gameObject.activeSelf)
                {
                    defender.gameObject.SetActive(true);
                    AddIngameItem(id);
                }
                else
                {
                    defender.SkillLevelUp();
                    LevelUpIngameItem(id);
                }
                break;
            case 3003:
                if (!football.gameObject.activeSelf)
                {
                    football.gameObject.SetActive(true);
                    AddIngameItem(id);
                }
                else
                {
                    football.SkillLevelUp();
                    LevelUpIngameItem(id);
                }
                break;
            case 3004:
                if (!drillShot.gameObject.activeSelf)
                {
                    drillShot.gameObject.SetActive(true);
                    AddIngameItem(id);
                }
                else
                {
                    drillShot.SkillLevelUp();
                    LevelUpIngameItem(id);
                }
                break;
            case 10001:
                kunai.SkillLevelUp();
                LevelUpIngameItem(id);
                break;
            case 10002:
                shotgun.SkillLevelUp();
                LevelUpIngameItem(id);
                break;
            case 20001:
                kunai.gameObject.SetActive(false);
                ghostKunai.gameObject.SetActive(true);
                player.PlayerStat().playerSkillInven[0] = DataManager.Instance.GetIngameItemData(id);
                break;
            case 20002:
                shotgun.gameObject.SetActive(false);
                gatlingGun.gameObject.SetActive(true);
                player.PlayerStat().playerSkillInven[0] = DataManager.Instance.GetIngameItemData(id);
                break;

            case 4001:
                if (!bulletSS.gameObject.activeSelf)
                {
                    bulletSS.gameObject.SetActive(true);
                    AddIngameItem(id);
                }
                else
                {
                    bulletSS.LevelUp();
                    LevelUpIngameItem(id);
                }
                break;
            case 4002:
                if (!ninjaScrollSS.gameObject.activeSelf)
                {
                    ninjaScrollSS.gameObject.SetActive(true);
                    AddIngameItem(id);
                }
                else
                {
                    ninjaScrollSS.LevelUp();
                    LevelUpIngameItem(id);
                }
                break;
            case 4003:
                if (!oilTicketSS.gameObject.activeSelf)
                {
                    oilTicketSS.gameObject.SetActive(true);
                    AddIngameItem(id);
                }
                else
                {
                    oilTicketSS.LevelUp();
                    LevelUpIngameItem(id);
                }
                break;
            case 4004:
                if (!energyDrinkSS.gameObject.activeSelf)
                {
                    energyDrinkSS.gameObject.SetActive(true);
                    AddIngameItem(id);
                }
                else
                {
                    energyDrinkSS.LevelUp();
                    LevelUpIngameItem(id);
                }
                break;
            case 4005:
                if (!sneakersSS.gameObject.activeSelf)
                {
                    sneakersSS.gameObject.SetActive(true);
                    AddIngameItem(id);
                }
                else
                {
                    sneakersSS.LevelUp();
                    LevelUpIngameItem(id);
                }
                break;
        }
    }
    private void AddIngameItem(int id)
    {
        IngameItemData temp = DataManager.Instance.GetIngameItemData(id);

        if (id > 4000 && id < 4006) player.PlayerStat().playerSupportInven[invenItemCount] = temp;
        else player.PlayerStat().playerSkillInven[invenItemCount] = temp;

        invenItemCount++;
    }
    private void LevelUpIngameItem(int id)
    {
        if (id > 4000 && id < 4006)
        {
            int index = player.PlayerStat().playerSupportInven.FindIndex(x => x.id == id);
            player.PlayerStat().playerSupportInven[index].level++;
        }
        else
        {
            int index = player.PlayerStat().playerSkillInven.FindIndex(x => x.id == id);
            player.PlayerStat().playerSkillInven[index].level++;
        }
    }

    //테스트
    public void OnSkillClick()
    {
        SelectSkill(testNumber);
        Debug.Log($"{player.PlayerStat().playerSkillInven[1].name}");
    }
}
