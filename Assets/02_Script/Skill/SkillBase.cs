using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [Header("Common Skill Data")]
    [SerializeField] protected float damage;
    [SerializeField] protected int count;

    [Header("Count Limit")]
    [SerializeField] protected int minCount = 1;
    [SerializeField] protected int maxCount = 6;

    // 스킬 초기화 (처음 생성될 때 1회)
    public virtual void Init()
    {
        count = Mathf.Clamp(minCount, minCount, maxCount);
        OnChanged(); // 초기에도 배치/재계산 실행
    }

    // 스킬 레벨업
    public virtual void LevelUp(float addDamage, int addCount)
    {
        damage += addDamage;
        count = Mathf.Clamp(count + addCount, minCount, maxCount);
        OnChanged(); // 스탯이 변했으니 재배치/재계산
    }

    // 스탯이 바뀌었을 때 재배치 필요시 호출
    protected abstract void OnChanged();
}
