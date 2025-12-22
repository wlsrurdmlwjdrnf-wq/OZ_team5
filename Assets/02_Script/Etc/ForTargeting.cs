using UnityEngine;

public class ForTargeting : MonoBehaviour, IDamageable
{
    //플레이어가 타겟팅할 MonoBehavior 클래스들은 이것을 상속받음
    public virtual void TakeDamage(int amout) { }
}
