using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Joystick joystick;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    public PlayerData PlayerStat { get; private set; }

    private Vector2 input;
    private bool isMoving;

    private static readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        ApplyPlayerStat();
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
    }
    private void FixedUpdate()
    {
        rb.velocity = input * PlayerStat.playerSpeed;
    }
    
    private void ApplyPlayerStat()
    {
        PlayerStat = PlayerData.GetDefault();
    }
}
