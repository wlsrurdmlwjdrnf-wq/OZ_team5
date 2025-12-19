using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private int atk;

    private float spawntime;
    private Vector2 shootDirection;

    private void OnEnable()
    {
        spawntime = Time.time;
    }
    private void Update()
    {
        transform.Translate(shootDirection * speed * Time.deltaTime);

        if(Time.time - spawntime >= lifetime)
        {
            PoolManager.Instance.ReturnPool(this);
        }
    }
    public void SetDirection(Vector2 dir)
    {
        shootDirection = dir.normalized;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(atk);
            PoolManager.Instance.ReturnPool(this);
        }
    }
}
