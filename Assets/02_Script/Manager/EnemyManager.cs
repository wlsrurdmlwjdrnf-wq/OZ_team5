using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : Singleton<EnemyManager>
{
    protected override void Init()
    {
        _IsDestroyOnLoad = false;
        base.Init();
    }
    public int enemyKillCount = 0;
    public List<ForTargeting> enemies = new List<ForTargeting>();

    public ForTargeting GetClosestEnemy(Vector2 position)
    {
        ForTargeting closest = null;
        float minDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            float dist = (enemy.transform.position - (Vector3)position).sqrMagnitude; // 제곱 거리 사용(Vector3.Distance보다 성능상 좋다고 합니다)
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }
        return closest;
    }
}