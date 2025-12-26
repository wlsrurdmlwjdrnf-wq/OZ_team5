using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    protected IngameItemData skillData;
    protected abstract int Id { get; set; }
    protected float damage;
    protected int count;
    protected WaitForSeconds interval;
    protected virtual void Awake()
    {
        skillData = DataManager.Instance.GetIngameItemData(Id);
        damage = skillData.damage;
        count = skillData.ptCount;
        interval = new WaitForSeconds(skillData.cooldown);
    }

    protected abstract void SkillLevelUp();
}
