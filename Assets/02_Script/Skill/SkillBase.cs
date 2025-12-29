using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    protected IngameItemData skillData;
    protected Player player;
    public abstract int Id { get; set; }
    public int Level { get { return level; } }
    protected float damage;
    protected int count;
    protected int level;
    protected WaitForSeconds interval;
    protected virtual void Awake()
    {
        skillData = DataManager.Instance.GetIngameItemData(Id);
        damage = skillData.damage;
        count = skillData.ptCount;
        level = skillData.level;
        interval = new WaitForSeconds(skillData.cooldown);
    }
    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        damage += player.PlayerStat().playerAtk;
    }
    public abstract void SkillLevelUp();
}
