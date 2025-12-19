using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : ForTargeting, IDamageable
{
    [SerializeField] protected int maxHp;
    [SerializeField] protected int atk;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected ItemBase expStone;

    protected Transform player;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    protected bool isKilled = false;
    protected bool isOverlapped = false;
    protected int hp;
    protected int initAtk;
    protected float initSpeed;
    protected Vector3 initScale;
    protected WaitForSeconds damageInterval;

    protected static readonly int isKilledHash = Animator.StringToHash("IsKilled");
    protected void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        initAtk = atk;
        initSpeed = moveSpeed;
        initScale = transform.localScale;
        damageInterval = new WaitForSeconds(0.5f);
    }
    protected void OnEnable()
    {
        animator.SetBool(isKilledHash, isKilled);
        hp = maxHp;
        atk = initAtk;
        moveSpeed = initSpeed;
        transform.localScale = initScale;
        if (EnemyManager.Instance != null && EnemyManager.Instance.enemies != null)
        {
            EnemyManager.Instance.enemies.Add(this);
        }
    }
    protected void OnDisable()
    {
        if (EnemyManager.Instance != null && EnemyManager.Instance.enemies != null)
        {
            EnemyManager.Instance.enemies.Remove(this);
        }
    }
    protected virtual void Update()
    {
        MoveToPlayer();
    }
    public virtual void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            isKilled = true;
            animator.SetBool(isKilledHash, isKilled);
            StartCoroutine(DieCo());
        }
    }
    protected virtual IEnumerator DieCo()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        ItemBase tmpStone = PoolManager.Instance.GetFromPool(expStone);
        tmpStone.transform.position = transform.position;
        ReturnPool();
    }
    public void ReturnPool()
    {
        isKilled = false;
        GetComponent<Collider2D>().enabled = true;
        PoolManager.Instance.ReturnPool(this);
    }
    protected void MoveToPlayer()
    {
        if (player == null || isKilled) return;

        transform.position = Vector3.MoveTowards(
                    transform.position,
                    player.position,
                    moveSpeed * Time.deltaTime
                    );

        if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            isOverlapped = true;
            StartCoroutine(DamageToPlayerCo(player));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            isOverlapped = false;
            StopCoroutine(DamageToPlayerCo(player));
        }
    }
    private IEnumerator DamageToPlayerCo(Player player)
    {
        while (isOverlapped)
        {
            player.TakeDamage(atk);
            yield return damageInterval;
        }
    }
}
