using UnityEngine;

public class RotationSkill : MonoBehaviour
{
    [Header("Common Skill Data")]
    [SerializeField] protected int damage;
    [SerializeField] protected int count;

    [Header("Count Limit")]
    [SerializeField] protected int minCount = 1;
    [SerializeField] protected int maxCount = 6;

    // 스킬 초기화 (처음 생성될 때 1회)
    public virtual void Init()
    {
        count = Mathf.Clamp(minCount, minCount, maxCount);
        OnChanged(); // 초기에도 배치/재계산 실행
    }

    // 스킬 레벨업
    public virtual void LevelUp(int addDamage, int addCount)
    {
        damage += addDamage;
        count = Mathf.Clamp(count + addCount, minCount, maxCount);
        OnChanged(); // 스탯이 변했으니 재배치/재계산
    }
    [Header("Rotation Settings")]
    // 부모 오브젝트의 회전 속도 (음수면 시계/반시계 반전)
    [SerializeField] private float rotateSpeed = -150f;

    // 회전 오브젝트들이 플레이어로부터 떨어진 거리
    [SerializeField] private float radius = 1.5f;

    [Header("Prefab Spawn")]
    // 회전 링에 배치될 프리팹
    // 콜라이더/피격 스크립트가 포함된 프리팹 권장
    [SerializeField] private GameObject orbiterPrefab;

    // 생성된 오브젝트들을 모아둘 부모
    // 비워두면 이 스크립트가 붙은 오브젝트(transform)를 사용
    [SerializeField] private Transform orbiterRoot;

    [Header("Physics Auto Fix (optional)")]
    // 충돌 인식 문제를 줄이기 위한 자동 물리 설정 여부
    [SerializeField] private bool autoFixPhysics = true;

    // 콜라이더를 트리거 방식으로 강제할지 여부
    [SerializeField] private bool forceTriggerCollider = true;

    private void Awake()
    {
        // orbiterRoot를 지정하지 않았다면 자기 자신을 루트로 사용
        // 인스펙터 설정 실수로 인한 NullReference 방지
        if (orbiterRoot == null)
            orbiterRoot = transform;
    }

    private void Start()
    {
        // 최대 개수(maxCount)만큼 미리 프리팹을 생성해둠
        // (레벨업 시 새로 Instantiate 하지 않기 위함)
        EnsureOrbiters();

        // 시작 개수(minCount)로 초기화
        // SkillBase.Init() 내부에서 OnChanged() → Deploy()까지 호출됨
        Init();
    }

    private void Update()
    {
        // 부모 오브젝트를 회전
        // 자식으로 배치된 모든 오브젝트가 함께 회전함
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    // count나 스탯이 변경되었을 때 호출됨
    protected void OnChanged()
    {
        // maxCount 변경이나 프리팹 교체 상황 대비
        EnsureOrbiters();

        // 현재 count 기준으로 활성화 + 재배치
        Deploy();
    }

    // orbiterRoot 아래에 프리팹을 maxCount까지 미리 생성
    private void EnsureOrbiters()
    {
        if (orbiterPrefab == null)
        {
            Debug.LogError("[RotationSkill] orbiterPrefab이 비어 있습니다.");
            return;
        }

        // 자식 수가 maxCount보다 적으면 계속 생성
        while (orbiterRoot.childCount < maxCount)
        {
            GameObject go = Instantiate(orbiterPrefab, orbiterRoot);

            // 구분용 이름 (Hierarchy 정리용)
            go.name = $"{orbiterPrefab.name}_{orbiterRoot.childCount - 1}";

            // 실제 사용 개수(count) 전까지는 비활성화
            go.SetActive(false);

            // 로컬 트랜스폼 초기화
            Transform t = go.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;

            // 충돌 인식 안정성을 위한 물리 설정 보정
            if (autoFixPhysics)
                FixPhysics(go);
        }
    }

    // count 개수만큼 활성화하고 원형으로 배치
    private void Deploy()
    {
        int useCount = Mathf.Clamp(count, minCount, maxCount);

        // 먼저 전부 비활성화
        for (int i = 0; i < orbiterRoot.childCount; i++)
            orbiterRoot.GetChild(i).gameObject.SetActive(false);

        // 필요한 개수만 활성화 + 각도 분배
        for (int i = 0; i < useCount; i++)
        {
            Transform orb = orbiterRoot.GetChild(i);
            orb.gameObject.SetActive(true);

            // 중심 기준으로 회전 배치
            orb.localPosition = Vector3.zero;
            orb.localRotation = Quaternion.identity;

            // 360도를 개수만큼 균등 분할
            float angle = 360f * i / useCount;
            orb.localRotation = Quaternion.Euler(0f, 0f, angle);

            // 로컬 Up 방향으로 반경만큼 이동
            orb.Translate(Vector3.up * radius, Space.Self);
        }
    }

    // 회전 오브젝트가 충돌/트리거 이벤트를 안정적으로 받도록 보정
    private void FixPhysics(GameObject go)
    {
        Collider2D col = go.GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogWarning($"[RotationSkill] {go.name}에 Collider2D가 없습니다.");
        }
        else if (forceTriggerCollider)
        {
            // 트리거 방식 데미지 처리용
            col.isTrigger = true;
        }

        // 트리거 콜백 안정성을 위해 Kinematic Rigidbody2D를 추가/보정
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = go.AddComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        rb.gravityScale = 0f;
    }
}