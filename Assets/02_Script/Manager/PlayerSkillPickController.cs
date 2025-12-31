using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkillPickController : MonoBehaviour
{
    Player player;
    List<IngameItemData> skillList;
    private bool hasSkill(IngameItemData item) => player.PlayerStat().playerSkillInven.Exists(x => x.id == item.id);
    private bool hasSup(IngameItemData item) => player.PlayerStat().playerSupportInven.Exists(x => x.id == item.id);
    private bool isMaxSkillLv(IngameItemData item) => player.PlayerStat().playerSkillInven.Exists(temp => temp.id == item.id && temp.level == 5);
    private bool isMaxSupLv(IngameItemData item) => player.PlayerStat().playerSupportInven.Exists(temp => temp.id == item.id && temp.level == 5);
    private bool isFullSkillInven() => !player.PlayerStat().playerSkillInven.Exists(temp => temp.id == 0);
    private bool isFullSupInven() => !player.PlayerStat().playerSupportInven.Exists(temp => temp.id == 0);

    private void Awake()
    {
        skillList = DataManager.Instance.GetAllIngameItemData();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

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

        for (int i = 0; i < count; i++)
        {
            if (tempList.Count == 0) break;

            int resultPer = tempList.Sum(_count => _count.per);
            int rand = UnityEngine.Random.Range(0, resultPer);
            int curPer = 0;

            TempPickSkillData pick = null;

            foreach (var _pick in tempList)
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
                tempList.Remove(pick);
            }
        }
        return result;
    }
    private bool PairCheck(int id)
    {
        return player.PlayerStat().playerSkillInven.Exists(t => t.id == id) || player.PlayerStat().playerSupportInven.Exists(t => t.id == id);
    }
}
