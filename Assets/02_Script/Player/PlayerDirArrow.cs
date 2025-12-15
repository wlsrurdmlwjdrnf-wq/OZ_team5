using UnityEngine;

public class PlayerDirArrow : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform player;
    [SerializeField] private float radius;

    private Vector2 input;
    private void Start()
    {
        joystick = FindObjectOfType<Joystick>();
    }
    private void Update()
    {
        input = joystick.GetInput();

        if (input.sqrMagnitude > 0.01f) // 입력이 있을 때만 표시
        {
            Vector2 dir = input.normalized;

            // 플레이어 중심에서 원 궤도 위치 계산
            Vector2 offset = dir * radius;
            arrow.position = player.position + (Vector3)offset;

            // 화살표가 방향을 바라보도록 회전
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
            arrow.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
