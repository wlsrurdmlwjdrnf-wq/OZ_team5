using UnityEngine;

public class GlowProjectile : MonoBehaviour
{
    [SerializeField] private EnemyProjectile smallPjt;
    [SerializeField] private EnemyProjectile bigPjt;

    private Vector2 dir;
    private bool isInit = false;
    private void OnDisable()
    {
        if(!isInit)
        {
            isInit = true;
            return;
        }

        float angleStep = 60f;
        for (int i = 0; i < 6; i++)
        {
            float angle = i * angleStep;
            dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            EnemyProjectile big = Managers.Pool.GetFromPool(bigPjt);
            big.SetDirection(dir);
            big.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        }
        angleStep = 24f;
        for (int i = 0; i < 15; i++)
        {
            float angle = i * angleStep;
            dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            EnemyProjectile small = Managers.Pool.GetFromPool(smallPjt);
            small.SetDirection(dir);
            small.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        }
    }
}
