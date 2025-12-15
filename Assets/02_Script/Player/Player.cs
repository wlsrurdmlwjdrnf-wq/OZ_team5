using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Joystick joystick;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private PlayerData playerData;
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

    //임시적으로 데미지주는 코드
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(playerData.playerAtk);
        }
    }
}
