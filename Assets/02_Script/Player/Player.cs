using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private HpBar hpBarPrefab;
    [SerializeField] private GameObject kunai;
    [SerializeField] private GameObject shotgun;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private PlayerData playerData;
    private HpBar hpBar;

    private Vector2 input;
    private bool isMoving;

    public PlayerData PlayerStat() => playerData;

    private static readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private void Awake()
    {
        joystick = Instantiate(joystick);

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        ApplyPlayerStat();
        SetBaseWeapon();
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
            Managers.Instance.Pool.ReturnPool(p);
        }
    }
    private void UpdateHpBar()
    {
        hpBar.UpdateHp(playerData.playerCurrentHp, playerData.playerMaxHp);
    }
    private void ApplyPlayerStat()
    {
        playerData = PlayerManager.Instance.playerData;
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
    public void TakeDamage(float damage)
    {
        playerData.playerCurrentHp -= damage;
        UpdateHpBar();
        if(playerData.playerCurrentHp<0)
        {
            hpBar.gameObject.SetActive(false);
            joystick.gameObject.SetActive(false);
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }
    private void SetBaseWeapon()
    {
        if (playerData.playerSkillInven[0].id == 0 || playerData.playerSkillInven[0].id == 10001)
        {
            IngameItemData temp = DataManager.Instance.GetIngameItemData(10001);
            playerData.playerSkillInven[0] = temp;
            kunai.gameObject.SetActive(true);
        }
        if (playerData.playerSkillInven[0].id == 10002)
        {
            IngameItemData temp = DataManager.Instance.GetIngameItemData(10002);
            playerData.playerSkillInven[0] = temp;
            shotgun.gameObject.SetActive(true);
        }
    }
}
