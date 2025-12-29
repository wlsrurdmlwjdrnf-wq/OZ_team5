using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : Singleton<SkillSystem>
{
    //레벨업 후 스킬선택지 등장 -> 스킬선택 -> 새로운 스킬이면 스킬 or 서포트 인벤토리에 추가
    //스킬이 레벨5가 되면 더이상 선택지에 등장x
    //스킬레벨5에다가 조합에 맞는 패시브(지원품)가 있으면 돌파조합스킬 출현
    //돌파조합스킬 클릭시 스킬레벨5였던 것이 돌파조합스킬로 변경
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
    protected override void Init()
    {
        _IsDestroyOnLoad = false;
        base.Init();
    }
    
    //스킬선택
    public void SelectSkill(int id)
    {
        switch (id)
        {
            case 3000:
                if (!fireBomb.gameObject.activeSelf)
                {
                    fireBomb.gameObject.SetActive(true);
                    IngameItemData temp = DataManager.Instance.GetIngameItemData(id);
                    player.PlayerStat().playerSkillInven.Add(temp);
                }
                else
                {
                    fireBomb.SkillLevelUp();
                }
                break;
            case 3001:
                if (!barrier.gameObject.activeSelf)
                {
                    barrier.gameObject.SetActive(true);
                    IngameItemData temp = DataManager.Instance.GetIngameItemData(id);
                    player.PlayerStat().playerSkillInven.Add(temp);
                }
                else
                {
                    barrier.SkillLevelUp();
                }
                break;
            case 3002:
                if (!defender.gameObject.activeSelf)
                {
                    defender.gameObject.SetActive(true);
                    IngameItemData temp = DataManager.Instance.GetIngameItemData(id);
                    player.PlayerStat().playerSkillInven.Add(temp);
                }
                else
                {
                    defender.SkillLevelUp();
                }
                break;
            case 3003:
                if (!football.gameObject.activeSelf)
                {
                    football.gameObject.SetActive(true);
                    IngameItemData temp = DataManager.Instance.GetIngameItemData(id);
                    player.PlayerStat().playerSkillInven.Add(temp);
                }
                else
                {
                    football.SkillLevelUp();
                }
                break;
            case 3004:
                if (!drillShot.gameObject.activeSelf)
                {
                    drillShot.gameObject.SetActive(true);
                    IngameItemData temp = DataManager.Instance.GetIngameItemData(id);
                    player.PlayerStat().playerSkillInven.Add(temp);
                }
                else
                {
                    drillShot.SkillLevelUp();
                }
                break;
            case 10001:
                kunai.SkillLevelUp();
                break;
            case 10002:
                shotgun.SkillLevelUp();
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
                    IngameItemData temp = DataManager.Instance.GetIngameItemData(id);
                    player.PlayerStat().playerSupportInven.Add(temp);
                }
                else
                {
                    bulletSS.LevelUp();
                }
                break;
            case 4002:
                if (!ninjaScrollSS.gameObject.activeSelf)
                {
                    ninjaScrollSS.gameObject.SetActive(true);
                    IngameItemData temp = DataManager.Instance.GetIngameItemData(id);
                    player.PlayerStat().playerSupportInven.Add(temp);
                }
                else
                {
                    ninjaScrollSS.LevelUp();
                }
                break;
            case 4003:
                if (!oilTicketSS.gameObject.activeSelf)
                {
                    oilTicketSS.gameObject.SetActive(true);
                    IngameItemData temp = DataManager.Instance.GetIngameItemData(id);
                    player.PlayerStat().playerSupportInven.Add(temp);
                }
                else
                {
                    oilTicketSS.LevelUp();
                }
                break;
            case 4004:
                if (!energyDrinkSS.gameObject.activeSelf)
                {
                    energyDrinkSS.gameObject.SetActive(true);
                    IngameItemData temp = DataManager.Instance.GetIngameItemData(id);
                    player.PlayerStat().playerSupportInven.Add(temp);
                }
                else
                {
                    energyDrinkSS.LevelUp();
                }
                break;
            case 4005:
                if (!sneakersSS.gameObject.activeSelf)
                {
                    sneakersSS.gameObject.SetActive(true);
                    IngameItemData temp = DataManager.Instance.GetIngameItemData(id);
                    player.PlayerStat().playerSupportInven.Add(temp);
                }
                else
                {
                    sneakersSS.LevelUp();
                }
                break;
        }
    }

    //테스트
    public void OnSkillClick()
    {
        SelectSkill(3001);
    }
}
