using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [Header("Common Skill Data")]
    [SerializeField] protected float damage;
    [SerializeField] protected int count;
    public int id;                 //스킬 종류
    public float speed;            //회전 속도

    [Header("Count Limit")]
    [SerializeField] protected int minCount = 1; //시작 개수
    [SerializeField] protected int maxCount = 6; //최대 개수

    protected virtual void Awake()
    {
        count = Mathf.Clamp(count, minCount, maxCount);
    }

    //스킬 초기화 (처음 생성될 때 1회)
    public virtual void Init()
    {
        count = minCount;
    }

    //스킬 레벨업
    public virtual void LevelUp(float addDamage, int addCount)
    {
        damage += addDamage;
        count = Mathf.Clamp(count + addCount, minCount, maxCount);
        
        //스탯이 변했으니 재배치/재계산
        OnChanged();
    }

    //스탯이 바뀌었을 때 재배치 필요시 호출(개수/데미지 등)
    protected abstract void OnChanged();
}
