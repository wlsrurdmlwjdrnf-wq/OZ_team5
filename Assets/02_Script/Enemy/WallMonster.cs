using UnityEngine;

public class WallMonster : EnemyBase
{
    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;

            // 벽 표면 가장 가까운 점을 구해서 밀어내기
            Vector2 closest = GetComponent<Collider2D>().ClosestPoint(other.transform.position);
            Vector2 pushDir = (rb.position - closest).normalized;

            rb.position = closest + pushDir * 0.3f; 
        }
    }
}

