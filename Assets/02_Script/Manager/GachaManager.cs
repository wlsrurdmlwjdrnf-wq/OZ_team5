using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/*
 * 뽑기 버튼을 누르면 룰렛이 돌아가고 선택된 아이템의 이미지가 화면에 나옴
 * 나온 아이템의 정보를 플레이어 인벤토리에 저장해야됨 (inventorymanager additem호출)
 * 뽑기에 필요한 돈의 보유여부는? ( playermanager에서 가진골드값 받아오기)
 */

public class GachaManager : Singleton<GachaManager>
{
    Player player;

    // 등급에 따른 확률 조정칸
    [SerializeField] public int niceRate = 45;
    [SerializeField] public int rareRate = 25;
    [SerializeField] public int eliteRate = 20;
    [SerializeField] public int epicRate = 9;
    [SerializeField] public int legendaryRate = 1;


    //인게임 레벨업시 뜨는 스킬들의 확률 시스템을 위한 변수들
    List<IngameItemData> skillList = DataManager.Instance.GetAllIngameItemData();
    private bool hasSkill(IngameItemData item) => player.PlayerStat().playerSkillInven.Contains(item);
    private bool hasSup(IngameItemData item) => player.PlayerStat().playerSupportInven.Contains(item);
    private bool isMaxSkillLv(IngameItemData item) => player.PlayerStat().playerSkillInven.Exists(temp => temp.id == item.id && temp.level == 5);
    private bool isMaxSupLv(IngameItemData item) => player.PlayerStat().playerSupportInven.Exists(temp => temp.id == item.id && temp.level == 5);
    private bool isFullSkillInven() => !player.PlayerStat().playerSkillInven.Exists(temp => temp.id == 0);
    private bool isFullSupInven() => !player.PlayerStat().playerSupportInven.Exists(temp => temp.id == 0);

    public event Action<ItemData> OnDrawItem;

    

    protected override void Init()
    {
        base.Init();
    }
    //뽑기 버튼을 누르면 이 함수를 호출하세요
    public void DrawItem()
    {
        // 돈이 부족한지 체크 부족하면 리턴


        //등급을 먼저 뽑고 뽑은 등급의 컨테이너에서 장비 한개 뽑아서 item 변수에 저장
        EnumData.EquipmentTier tier = GetRarity();
        ItemData item = GetItemByRarity(tier);


        //아이템 뽑았다고 외부에 알리기
        OnDrawItem?.Invoke(item);

    }


    // 무슨 등급인지 반환
    public EnumData.EquipmentTier GetRarity()
    {
        int value = UnityEngine.Random.Range(1, 101);
        int total = 0;


        total += legendaryRate;
        if (value <= total)
        {
            return EnumData.EquipmentTier.Legendary;
        }

        total += epicRate;
        if (value <= total)
        {
            return EnumData.EquipmentTier.Epic;
        }

        total += eliteRate;
        if (value <= total)
        {
            return EnumData.EquipmentTier.Elite;
        }

        total += rareRate;
        if (value <= total)
        {
            return EnumData.EquipmentTier.Rare;
        }

        return EnumData.EquipmentTier.Nice;
    }

    //등급별로 분류된 컨테이너에서 랜덤아이템 뽑아서 반환
    public ItemData GetItemByRarity(EnumData.EquipmentTier tier)
    {       
        List<ItemData> item = DataManager.Instance.GetItemRarityList(tier);
        if (item == null || item.Count == 0) return null;
        int value = UnityEngine.Random.Range(0, item.Count);
        return item[value];
    }
    

    /*
     * 레벨업시 호출 함수
     * 플레이어 스킬인벤토리의 정보를 기반으로 로직이 작동
     * 보유한 아이템은 좀더 잘뜨고 만렙일경우 제외(진화 가능한경우 예외) / 해당타입의 인벤토리가 꽉차면 제외
     * 진화가 가능하다면 다음레벨업시 나올확률이 매우높음
     */
    public List<IngameItemData> SelectSkill()
    {
        List<TempPickSkillData> result = new List<TempPickSkillData>();

        foreach (var item in skillList)
        {
            if (item.id == 0 || item.type == EnumData.SkillType.NONE) continue;

            bool hasItem = false;
            bool maxLv = false;
            bool isFull = false;

            if (item.type == EnumData.SkillType.Attack)
            {
                hasItem = hasSkill(item);
                maxLv = isMaxSkillLv(item);
                isFull = isFullSkillInven();
            }
            else
            {
                hasItem = hasSup(item);
                maxLv = isMaxSupLv(item);
                isFull = isFullSupInven();
            }
            // 아이템을 가지고 있는가
            if (hasItem)
            {
                // 만렙이 아닌가
                if (!maxLv)
                {
                    // 50의 값으로 저장
                    result.Add(new TempPickSkillData(item, 50));
                }
                else // 만렙이라면
                {
                    if (item.EvID != -1) // 진화 아이디가 있다면
                    {
                        bool hasPair = item.pairID.Any(id => PairCheck(id)); // 인벤토리에 짝꿍아이템 체크

                        if (hasPair)
                        {                            
                            if (DataManager.Instance.TryGetIngameItem(item.EvID, out IngameItemData temp)) // 만렙이고 짝꿍도 있다면 무조건 나와야함
                            {
                                result.Add(new TempPickSkillData(temp, 1000));
                            }
                        }
                    }
                }
            }
            else // 새로운 아이템
            {
                if (!isFull && item.id < 20000) // 스킬칸이 남아있으면, 20000은 최종진화 ID 최종진화 아이템을 안뜨도록
                {
                    result.Add(new TempPickSkillData(item, 10));
                }
            }
        }
        // 임시로 뽑힌 스킬데이터중에 확률을 비교해서 3개 뽑음
        return ResultPickSkillData(result, 3);
    }
    
    // 가중치 계산전 임시로 픽된 스킬데이터 모음
    public class TempPickSkillData
    {
        public IngameItemData item;
        public int per;

        public TempPickSkillData(IngameItemData _item, int _per)
        {
            item = _item;
            per = _per;
        }
    }
    
    // 최종 선택된 스킬데이터 리턴함수
    private List<IngameItemData> ResultPickSkillData(List<TempPickSkillData> tempList, int count)
    {
        List<IngameItemData> result = new List<IngameItemData>();
        var pool = new List<TempPickSkillData>(tempList);

        for (int i = 0; i < count; i++)
        {
            if (pool.Count == 0) break;

            int resultPer = pool.Sum(_count => _count.per);
            int rand = UnityEngine.Random.Range(0, resultPer);
            int curPer = 0;

            TempPickSkillData pick = null;

            foreach (var _pick in pool)
            {
                curPer += _pick.per;
                if (rand < curPer)
                {
                    pick = _pick;
                    break;
                }
            }
            if (pick != null)
            {
                result.Add(pick.item);
                pool.Remove(pick);
            }
        }
        return result;
    }
    private bool PairCheck(int id)
    {
        return player.PlayerStat().playerSkillInven.Exists(t => t.id == id) || player.PlayerStat().playerSupportInven.Exists(t => t.id == id);
    }
}
