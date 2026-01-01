using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class ProjectileBase : MonoBehaviour
{
    protected float damage;
    protected float speed;
    protected float lifetime;

    protected Player player;
    protected IngameItemData skillData;
    protected Rigidbody2D rb;
    public abstract int Id { get; set; }

    protected Vector2 shootDirection;
    protected float spawntime;


    protected void Awake()
    {
        skillData = DataManager.Instance.GetIngameItemData(Id);
        damage = skillData.damage;
        speed = skillData.ptSpeed;
        lifetime = skillData.lifeTime;

        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    protected virtual void OnEnable() 
    {
        SceneController.Instance.OnLoadLobbyScene += ReturnPool;
        GameManager.Instance.OnGameClear += ReturnPool;
        GameManager.Instance.OnGameOver += ReturnPool;
        spawntime = Time.time;
        if (rb != null)
        {
            rb.velocity = shootDirection * speed;
        }
    }
    protected virtual void OnDisable()
    {
        SceneController.Instance.OnLoadLobbyScene -= ReturnPool;
        GameManager.Instance.OnGameClear -= ReturnPool;
        GameManager.Instance.OnGameOver -= ReturnPool;
    }
    protected virtual void Update()
    {
        if (Time.time - spawntime >= lifetime) ReturnPool();
    }
    public void SetDirection(Vector2 dir)
    {
        shootDirection = dir.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (rb != null)
        {
            rb.velocity = shootDirection * speed;
        }

    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            DamageTextManager.Instance.ShowDamage(damage + player.PlayerStat().playerAtk, enemy.transform.position);
        }
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(damage + player.PlayerStat().playerAtk);
            ReturnPool();
        }
    }
    protected virtual void ReturnPool()
    {
        Managers.Instance.Pool.ReturnPool(this);
    }
    public abstract void ProjectileStatUp();
}
