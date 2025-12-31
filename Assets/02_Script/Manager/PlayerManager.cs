using System;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerData playerData {  get; private set; }

    public event Action OnItemUpdata;

    protected override void Init()
    {
        base.Init();
        playerData = new PlayerData();
        playerData.SetPlayerInven();
    }
    private void Start()
    {
        //GachaManager.Instance.OnDrawItem += AddItemInven;
    }

    //장비 장착
    public void EquipItem(int slot)
    {
        /* 장착할 아이템 : item
         * 빈아이템으로 채우기 위한 아이템 : empty
         * 장착중인 아이템 : curItem
         * 장착하기위한 타입 : type
         */
        ItemData item = playerData.playerGeneralInven[slot];

        //empty나 장비아이템이 아닌아이템은 장착되면 안되니까 return
        if (item.id == 0 || item.type == EnumData.EquipmentType.NONE) return;

        ItemData empty = DataManager.Instance.GetItemData(0);
        ItemData curItem = playerData.playerEquipInven[item.type];

        //장비칸이 비었다면
        if (curItem.id == 0)
        {
            playerData.playerEquipInven[item.type] = item;
            playerData.playerGeneralInven[slot] = empty;
            if (item.name == "Kunai")
            {
                playerData.playerSkillInven[0] = DataManager.Instance.GetIngameItemData(10001);
            }
            if (item.name == "Shotgun")
            {
                playerData.playerSkillInven[0] = DataManager.Instance.GetIngameItemData(10002);
            }
        }
        else
        {
            playerData.playerEquipInven[item.type] = item;
            playerData.playerGeneralInven[slot] = curItem;

        }
        //장비착용로직이 끝난 후 장비에 따른 스탯변화를 위해 resetstat 호출
        ResetPlayerStat();
        //변경된 인벤토리들의 아이템icon을 띄우라고 action 호출
        OnItemUpdata?.Invoke();
    }

    //장비 해제
    public void UnEquipItem(int slot)
    {
        //장착된 아이템 curitem
        ItemData curitem = playerData.playerEquipInven[(EnumData.EquipmentType)slot];
        ItemData empty = DataManager.Instance.GetItemData(0);

        if (curitem.id == 0) return;

        //장비 해제시 들어간 빈 인벤토리가 있는지 확인
        int index = playerData.playerGeneralInven.FindIndex(item => item.id == 0);
        
        //빈 인벤토리가 있다면
        if (index != -1)
        {
            playerData.playerEquipInven[curitem.type] = empty;
            playerData.playerGeneralInven[index] = curitem;
        }

        ResetPlayerStat();
        OnItemUpdata?.Invoke();
    }

    //상점에서 뽑은 아이템 추가
    public void AddItemInven(ItemData item)
    {
        int index = playerData.playerGeneralInven.FindIndex(_item => _item.id == 0);
        if (index != -1)
        {
            playerData.playerGeneralInven[index] = item;
        }
        // 꽉차있을때 로직 ex) 우편함에 넣어두기
    }

    //배틀씬에서 획득한 스킬,지원폼 인벤토리에 추가
    public void AddIngameItemInven(IngameItemData item)
    {
        if (item.type == EnumData.SkillType.Attack)
        {
            int index = playerData.playerSkillInven.FindIndex(_item => _item.id == 0);
            if (index != -1)
            {
                playerData.playerSkillInven[index] = item;
            }
            else
            {
                // 스킬창 꽉찼다고 알림
            }
        }
        else if (item.type == EnumData.SkillType.Support)
        {
            int index = playerData.playerSupportInven.FindIndex(_item => _item.id == 0);
            if (index != -1)
            {
                playerData.playerSupportInven[index] = item;
            }
            else
            {
                // 지원폼창 꽉찼다고 알림
            }
        }
    }

    //플레이어의 스탯변화시(장비변화, 진화스탯변화) 호출 버그발생을 없애기 위해 호출시마다 초기화 후 데이터 대입
    public void ResetPlayerStat()
    {
        float oldMaxHp = playerData.playerMaxHp;
        PlayerData baseData = DataManager.Instance.GetPlayerBaseStat();
        playerData.SetBaseData(baseData);
        float totalMtp = 0f;

        foreach (var item in playerData.playerEquipInven.Values)
        {
            if (item == null || item.id == 0) continue;

            playerData.resultAtkPer += item.atkPercent;
            
            if (item.atkMtp > 1f)
            {
                totalMtp += (item.atkMtp - 1f);
            }

            playerData.resultHpPer += item.hpPercent;
        }

        playerData.playerMaxHp = (int)MathF.Round(playerData.playerMaxHp * (1.0f + (playerData.resultHpPer / 100.0f)));

        if (playerData.playerMaxHp > oldMaxHp)
        {
            float temp = playerData.playerMaxHp - oldMaxHp;
            playerData.playerCurrentHp += temp;
        }
        else if (playerData.playerMaxHp < oldMaxHp)
        {
            if (playerData.playerCurrentHp > playerData.playerMaxHp)
            {
                playerData.playerCurrentHp = playerData.playerMaxHp;
            }            
        }

        playerData.playerAtk = (int)MathF.Round(playerData.playerAtk * (1.0f + (playerData.resultAtkPer / 100.0f))) * (1f + totalMtp);
    }
}
