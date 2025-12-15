using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected int maxHp;
    [SerializeField] protected int atk;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected ExpStone expStone;

    protected Transform player;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    protected bool isKilled = false;
    protected int hp;

    protected static readonly int isKilledHash = Animator.StringToHash("IsKilled");
    protected void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    protected void OnEnable()
    {
        animator.SetBool(isKilledHash, isKilled);
        hp = maxHp;
    }
    protected void Update()
    {
        MoveToPlayer();
    }
    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            isKilled = true;
            animator.SetBool(isKilledHash, isKilled);
            StartCoroutine(DieCo());
        }
    }
    protected IEnumerator DieCo()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        var tmpStone = PoolManager.Instance.GetFromPool(expStone);
        tmpStone.transform.position = transform.position;
        ReturnPool();
    }
    protected void ReturnPool()
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
}
