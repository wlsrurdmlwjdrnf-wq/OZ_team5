using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    protected IngameItemData skillData;
    protected abstract int Id { get; set; }

    protected void Awake()
    {
        skillData = DataManager.Instance.GetIngameItemData(Id);
    }
}
