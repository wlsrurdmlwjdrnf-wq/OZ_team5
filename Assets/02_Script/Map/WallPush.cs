using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WallPush : MonoBehaviour
{
    //겹침 해소 후 살짝 띄워두는 여유 값 (재겹침 방지)
    [SerializeField] private float padding = 0.02f;

    //벽 방향 속도 감쇠 비율 (0 = 완전 제거, 0.2 ~ 0.4 = 미끄러짐)
    [Range(0f, 1f)]
    [SerializeField] private float velocityDamping = 0.0f;

    //이 벽에 막힐 대상 태그들
    [SerializeField] private string[] targetTags = { "Player", "Enemy" };

    //벽 콜라이더
    private Collider2D wallCol;

    private void Awake()
    {
        //벽의 Collider2D 캐싱
        wallCol = GetComponent<Collider2D>();

        //트리거로 감지해서 위치만 보정
        wallCol.isTrigger = true;
    }

    //이 오브젝트가 막아야 할 대상인지 검사
    private bool IsTarget(Collider2D other)
    {
        //태그 배열이 비어 있으면 모두 허용
        if (targetTags == null || targetTags.Length == 0)
            return true;

        // 등록된 태그 목록을 하나씩 확인
        foreach (var tag in targetTags)
        {
            if (other.CompareTag(tag))
                return true;
        }
        return false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Player / Enemy 외에는 무시
        if (!IsTarget(other))
            return;

        //Rigidbody2D가 없으면 보정 불가
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        //대상 콜라이더
        Collider2D otherCol = other;

        //벽과 대상의 실제 겹침 정보 계산
        ColliderDistance2D cd = wallCol.Distance(otherCol);

        //겹치지 않았으면 처리하지 않음
        if (!cd.isOverlapped) return;

        //겹친 깊이만큼 + padding 만큼 바깥으로 밀어냄
        Vector2 push = cd.normal * (-cd.distance + padding);

        //물리 위치 이동 (끼임 방지 핵심)
        rb.MovePosition(rb.position + push);

        //벽으로 파고드는 속도 성분만 제거/감쇠
        Vector2 v = rb.velocity;

        // 현재 속도가 "벽을 향해 얼마나 들어가고 있는지" 계산
        float intoWall = Vector2.Dot(v, cd.normal);

        // 벽 쪽으로 파고드는 속도가 있을 때만 처리
        if (intoWall > 0f)
        {
            v -= cd.normal * (intoWall * (1f - velocityDamping));
            rb.velocity = v;
        }
    }
}
