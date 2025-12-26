using UnityEngine;

public class DrillProjectile : ProjectileBase
{
    [SerializeField] private float margin = 0.02f; // 화면 가장자리 여유 (스프라이트 크기 고려)

    [SerializeField] private int maxBounceCount;
    private int curBounceCount;

    protected override int Id { get; set; } = 3004;

    protected override void OnEnable()
    {
        base.OnEnable();
        curBounceCount = 0;
    }
    protected override void Update()
    {
        base.Update();

        // 현재 위치를 Viewport 좌표로 변환 (0~1)
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        bool bounced = false;

        // X축 경계 체크
        if (vp.x < margin)
        {
            vp.x = margin;
            shootDirection.x = Mathf.Abs(shootDirection.x); // 오른쪽으로 반사
            bounced = true;
        }
        else if (vp.x > 1f - margin)
        {
            vp.x = 1f - margin;
            shootDirection.x = -Mathf.Abs(shootDirection.x); // 왼쪽으로 반사
            bounced = true;
        }

        // Y축 경계 체크
        if (vp.y < margin)
        {
            vp.y = margin;
            shootDirection.y = Mathf.Abs(shootDirection.y); // 위로 반사
            bounced = true;
        }
        else if (vp.y > 1f - margin)
        {
            vp.y = 1f - margin;
            shootDirection.y = -Mathf.Abs(shootDirection.y); // 아래로 반사
            bounced = true;
        }

        // 튕겼으면 위치를 다시 월드 좌표로 변환
        if (bounced)
        {
            transform.position = Camera.main.ViewportToWorldPoint(vp);
            IncreaseBounceCount();
        }
    }
    private void IncreaseBounceCount()
    {
        curBounceCount++;
        if (curBounceCount >= maxBounceCount)
        {
            ReturnPool();
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            DamageTextManager.Instance.ShowDamage(damage, enemy.transform.position);
        }
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(damage);
        }
    }
}