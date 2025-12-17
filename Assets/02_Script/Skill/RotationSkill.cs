using UnityEngine;

public class RotationSkill : SkillBase
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotateSpeed = -150f;
    [SerializeField] private float radius = 1.5f;

    [Header("Prefab Spawn")]
    [Tooltip("회전 링에 배치될 프리펩 (콜라이더/피격 스크립트 포함 추천)")]
    [SerializeField] private GameObject orbiterPrefab;

    [Tooltip("생성된 오브젝트를 이 Transform 아래에 모읍니다. 비워두면 이 오브젝트(transform) 아래로 생성됩니다.")]
    [SerializeField] private Transform orbiterRoot;

    [Tooltip("생성 시 콜라이더/트리거 체크를 자동으로 보정할지")]
    [SerializeField] private bool autoFixPhysics = true;

    private void Awake()
    {
        if (orbiterRoot == null) orbiterRoot = transform;
    }

    private void Start()
    {
        // 프리팹으로 maxCount까지 미리 생성해두기
        EnsureOrbiters();

        // 시작 개수(minCount)로 초기화 + 배치
        Init();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    protected override void OnChanged()
    {
        EnsureOrbiters(); // maxCount가 바뀌거나 프리팹 늦게 넣었을 때 대비
        Deploy();
    }

    // orbiterRoot 아래 자식이 maxCount가 될 때까지 프리팹으로 생성
    private void EnsureOrbiters()
    {
        if (orbiterPrefab == null)
        {
            Debug.LogError("[RotationSkill] orbiterPrefab이 비어 있습니다. 인스펙터에 프리팹을 넣어주세요.");
            return;
        }

        while (orbiterRoot.childCount < maxCount)
        {
            GameObject go = Instantiate(orbiterPrefab, orbiterRoot);

            // 정리용 이름(선택)
            go.name = $"{orbiterPrefab.name}_{orbiterRoot.childCount - 1}";

            // 처음엔 꺼두고, count만큼만 켬
            go.SetActive(false);

            // 배치 초기화
            Transform t = go.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;

            if (autoFixPhysics)
                FixPhysics(go);
        }
    }

    // count 개수만큼 활성화하고 원형 배치
    private void Deploy()
    {
        if (orbiterRoot == null) return;

        int useCount = Mathf.Clamp(count, minCount, maxCount);

        // 전체 비활성화
        for (int i = 0; i < orbiterRoot.childCount; i++)
            orbiterRoot.GetChild(i).gameObject.SetActive(false);

        // 필요한 개수만 활성화 + 원형 배치
        for (int i = 0; i < useCount; i++)
        {
            Transform orb = orbiterRoot.GetChild(i);
            orb.gameObject.SetActive(true);

            orb.localPosition = Vector3.zero;
            orb.localRotation = Quaternion.identity;

            float angle = 360f * i / useCount;
            orb.localRotation = Quaternion.Euler(0f, 0f, angle);
            orb.Translate(Vector3.up * radius, Space.Self);
        }
    }

    // "인식이 안 됨"을 줄이기 위한 최소 물리 설정 점검/보정
    // - 2D 트리거 데미지 방식이면: Collider2D.isTrigger = true 권장
    // - Rigidbody2D가 없으면 트리거 이벤트가 안 뜨는 경우가 많아서 하나 붙여줌(kinematic)
    private void FixPhysics(GameObject go)
    {
        // 2D 콜라이더가 없으면 경고(프리팹에 넣는 게 정석)
        Collider2D col = go.GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogWarning($"[RotationSkill] {go.name} 프리팹에 Collider2D가 없습니다. 피격/충돌 인식이 안 될 수 있어요.");
        }
        else
        {
            // 트리거 데미지 방식이면 트리거로 (원하지 않으면 여기 false로 바꿔)
            col.isTrigger = true;
        }

        // 트리거 콜백 안정성 위해 Rigidbody2D를 하나 둠(권장)
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = go.AddComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        rb.gravityScale = 0f;
    }
}
