using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private HpBar hpBarPrefab;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private PlayerData playerData;
    private HpBar hpBar;
    public PlayerData PlayerStat() => playerData;

    private Vector2 input;
    private bool isMoving;

    private static readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private void Awake()
    {
        joystick = Instantiate(joystick);

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        ApplyPlayerStat();
        playerData.playerCurrentHp = playerData.playerMaxHp;
    }
    private void Start()
    {
        hpBar = Instantiate(hpBarPrefab);
        hpBar.Init(transform);
        UpdateHpBar();
    }
    private void Update()
    {
        input = joystick.GetInput();

        if(input != Vector2.zero)
        {
            if(input.x < 0) sr.flipX = true;
            else sr.flipX = false;
        }

        isMoving = Mathf.Abs(input.x) > 0 || Mathf.Abs(input.y) > 0;
        anim.SetBool(isMovingHash, isMoving);

        PullItems();
    }
    private void FixedUpdate()
    {
        rb.velocity = input * playerData.playerSpeed;
    }
    private void OnDisable()
    {
        ProjectileBase[] pjt = FindObjectsOfType<ProjectileBase>();
        if (pjt == null || pjt.Length == 0) return;
        foreach (ProjectileBase p in pjt)
        {
            Managers.Pool.ReturnPool(p);
        }
    }
    private void UpdateHpBar()
    {
        hpBar.UpdateHp(playerData.playerCurrentHp, playerData.playerMaxHp);
    }
    private void ApplyPlayerStat()
    {
        playerData = PlayerData.GetDefault();
    }
    private void PullItems()
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, playerData.magnetRadius);

        foreach (Collider2D col in items)
        {
            if (col.CompareTag("Item"))
            {
                col.transform.position = Vector3.MoveTowards(
                    col.transform.position,
                    transform.position,
                    50f * Time.deltaTime
                );
            }
        }
    }
    public void TakeDamage(int damage)
    {
        playerData.playerCurrentHp -= damage;
        UpdateHpBar();
        if(playerData.playerCurrentHp<0)
        {
            hpBar.gameObject.SetActive(false);
            joystick.gameObject.SetActive(false);
            gameObject.SetActive(false);
            //GameManager.Instance.GameOver();
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent<EnemyBase>(out EnemyBase enemy))
    //    {
    //        if(input == Vector2.zero)
    //        {
    //            rb.constraints = RigidbodyConstraints2D.FreezeAll;
    //            GetComponent<Collider2D>().isTrigger = false;
    //        }
    //        else
    //        {
    //            rb.constraints = RigidbodyConstraints2D.None;
    //            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    //            GetComponent<Collider2D>().isTrigger = true;
    //        }
    //    }
    //} 플레이어 충돌판정 일단 보류
}
