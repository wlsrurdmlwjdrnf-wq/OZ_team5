using UnityEngine;

public static class WallMonsterSpawn
{
    public static void SpawnMonsterWall(EnemyBase enemyPrefab, Vector2 center, float width, float height, float interval)
    {
        EnemyBase enemy;
        // 위쪽 변
        for (float x = center.x - width / 2; x <= center.x + width / 2; x += interval)
        {
            enemy = Managers.Pool.GetFromPool(enemyPrefab);
            enemy.transform.position = new Vector2(x, center.y + height / 2);
        }

        // 아래쪽 변
        for (float x = center.x - width / 2; x <= center.x + width / 2; x += interval)
        {
            enemy = Managers.Pool.GetFromPool(enemyPrefab);
            enemy.transform.position = new Vector2(x, center.y - height / 2);
        }

        // 왼쪽 변
        for (float y = center.y - height / 2; y <= center.y + height / 2; y += interval)
        {
            enemy = Managers.Pool.GetFromPool(enemyPrefab);
            enemy.transform.position = new Vector2(center.x - width / 2, y);
        }

        // 오른쪽 변
        for (float y = center.y - height / 2; y <= center.y + height / 2; y += interval)
        {
            enemy = Managers.Pool.GetFromPool(enemyPrefab);
            enemy.transform.position = new Vector2(center.x + width / 2, y);
        }
    }
}