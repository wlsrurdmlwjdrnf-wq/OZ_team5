using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballProjectile : ProjectileBase
{
    [SerializeField] private Camera cam;
    [SerializeField] private float margin = 0.02f; // 화면 가장자리 여유 (스프라이트 크기 고려)

    [SerializeField] private int maxBounceCount;
    private int curBounceCount;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        curBounceCount = 0;
    }
    protected override void Update()
    {
        base.Update();

        // 현재 위치를 Viewport 좌표로 변환 (0~1)
        Vector3 vp = cam.WorldToViewportPoint(transform.position);
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
            transform.position = cam.ViewportToWorldPoint(vp);
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
            DamageTextManager.Instance.ShowDamage(dmg, enemy.transform.position);
        }
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(dmg);
            Vector2 dir = (transform.position - collision.transform.position).normalized;
            shootDirection = Vector2.Reflect(shootDirection, dir).normalized;
            IncreaseBounceCount();
        }
    }
}
