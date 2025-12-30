using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private EnemyBase zombiePrefab;
    [SerializeField] private EnemyBase bigZombiePrefab;
    [SerializeField] private EnemyBase zomPlantPrefab;
    [SerializeField] private EnemyBase zomDogPrefab;
    [SerializeField] private EnemyBase suicideBomberPrefab;
    [SerializeField] private EnemyBase wallMonsterPrefab;

    [SerializeField] private EnemyBase boss1Prefab;
    [SerializeField] private EnemyBase boss2Prefab;
    [SerializeField] private EnemyBase boss3Prefab;

    [SerializeField] private ItemBox itemBoxPrefab;

    [SerializeField] private ItemBase bombPrefab;
    [SerializeField] private ItemBase coin10Prefab;
    [SerializeField] private ItemBase coin30Prefab;
    [SerializeField] private ItemBase coin100Prefab;
    [SerializeField] private ItemBase magnetPrefab;
    [SerializeField] private ItemBase meatPrefab;

    [SerializeField] private ItemBase expStone1Prefab;
    [SerializeField] private ItemBase expStone5Prefab;
    [SerializeField] private ItemBase expStone10Prefab;
    [SerializeField] private ItemBase expStone50Prefab;

    [SerializeField] private EnemyProjectile enemySmallPjtPrefab;
    [SerializeField] private EnemyProjectile enemyBigPjtPrefab;
    [SerializeField] private EnemyProjectile enemyGlowPjtPrefab;

    [SerializeField] private GameObject WarningPanel;

    public static Spawner Instance { get; private set; }

    private float posX;
    private float posY;
    private Vector3 tmpPos;
    private Transform player;

    private void Awake()
    {
        Instance = this;
        player = GameObject.FindWithTag("Player").transform;
    }
    private void Start()
    {
        if (Managers.Instance.Pool.isCreatePool == false)
        {
            Managers.Instance.Pool.CreatePool(itemBoxPrefab, 50);

            Managers.Instance.Pool.CreatePool(zombiePrefab, 100);
            Managers.Instance.Pool.CreatePool(bigZombiePrefab, 10);
            Managers.Instance.Pool.CreatePool(zomPlantPrefab, 20);
            Managers.Instance.Pool.CreatePool(zomDogPrefab, 50);
            Managers.Instance.Pool.CreatePool(suicideBomberPrefab, 20);
            Managers.Instance.Pool.CreatePool(wallMonsterPrefab, 100);

            Managers.Instance.Pool.CreatePool(boss1Prefab, 2);
            Managers.Instance.Pool.CreatePool(boss2Prefab, 2);
            Managers.Instance.Pool.CreatePool(boss3Prefab, 2);

            Managers.Instance.Pool.CreatePool(bombPrefab, 10);
            Managers.Instance.Pool.CreatePool(coin10Prefab, 10);
            Managers.Instance.Pool.CreatePool(coin30Prefab, 10);
            Managers.Instance.Pool.CreatePool(coin100Prefab, 10);
            Managers.Instance.Pool.CreatePool(magnetPrefab, 10);
            Managers.Instance.Pool.CreatePool(meatPrefab, 10);

            Managers.Instance.Pool.CreatePool(expStone1Prefab, 100);
            Managers.Instance.Pool.CreatePool(expStone5Prefab, 100);
            Managers.Instance.Pool.CreatePool(expStone10Prefab, 100);
            Managers.Instance.Pool.CreatePool(expStone50Prefab, 50);

            Managers.Instance.Pool.CreatePool(enemySmallPjtPrefab, 150);
            Managers.Instance.Pool.CreatePool(enemyBigPjtPrefab, 100);
            Managers.Instance.Pool.CreatePool(enemyGlowPjtPrefab, 20);

            Managers.Instance.Pool.isCreatePool = true;
        }

        StartCoroutine(SpawnItemBox());
        StartCoroutine(SpawnEnemy(zombiePrefab, 1f));
        StartCoroutine(SpawnEnemy(bigZombiePrefab, 4f));
        StartCoroutine(SpawnBoss(boss1Prefab, 20f));
    }
    private void OnDisable()
    {
        ClearField(Vector2.zero, 50f);
        StopAllCoroutines();
    }
    private IEnumerator SpawnBoss(EnemyBase boss, float waitBossTime)
    {
        yield return new WaitForSeconds(waitBossTime);
        //보스경고 필요
        WarningPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        WarningPanel.SetActive(false);
        ClearField(player.position, 50f);

        WallMonsterSpawn.SpawnMonsterWall(wallMonsterPrefab, Camera.main.transform.position, 14f, 12f, 1.0f);

        EnemyBase bossObj = Managers.Instance.Pool.GetFromPool(boss);
        bossObj.transform.position = RandPos(6f, 5f, 4f) + player.position; 

        StopAllCoroutines();
        //보스 출현 경고 후 보스와 벽몬스터 스폰, 타이머 정지, 모든 적과 아이템 풀로 리턴+모든 코루틴 stop해야함.
        //보스를 잡은 후에 타이머 다시 작동
    }
    private void ClearField(Vector2 pos, float radius)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(pos, radius);

        if(cols == null || cols.Length == 0) return;

        foreach (Collider2D col in cols)
        {
            if (col.TryGetComponent<EnemyBase>(out EnemyBase target))
            {
                Managers.Instance.Pool.ReturnPool(target);
            }
            if (col.TryGetComponent<ItemBox>(out ItemBox box))
            {
                Managers.Instance.Pool.ReturnPool(box);
            }
            if (col.TryGetComponent<ItemBase>(out ItemBase item))
            {
                Managers.Instance.Pool.ReturnPool(item);
            }
            if (col.TryGetComponent<EnemyProjectile>(out EnemyProjectile projectile))
            {
                Managers.Instance.Pool.ReturnPool(projectile);
            }
        }
    }
    public void KillBoss1()
    {
        ClearField(player.position, 30f);
        StartCoroutine(SpawnItemBox());
        StartCoroutine(SpawnEnemy(zomPlantPrefab, 2f));
        StartCoroutine(SpawnEnemy(zomDogPrefab, 1f));
        StartCoroutine(SpawnBoss(boss2Prefab, 20f));
    }
    public void KillBoss2()
    {
        ClearField(player.position, 30f);
        StartCoroutine(SpawnItemBox());
        StartCoroutine(SpawnEnemy(zomPlantPrefab, 2f));
        StartCoroutine(SpawnEnemy(zomDogPrefab, 1f));
        StartCoroutine(SpawnEnemy(suicideBomberPrefab, 3f));
        StartCoroutine(SpawnBoss(boss3Prefab, 20f));
    }
    private IEnumerator SpawnItemBox()
    {
        while (true)
        {
            ItemBox itembox = Managers.Instance.Pool.GetFromPool(itemBoxPrefab);
            itembox.transform.position = RandPos(8f, 8f, 6f) + player.position;
            yield return new WaitForSeconds(10f);
        }
    }
    private IEnumerator SpawnEnemy(EnemyBase prefab,float spawnInterval)
    {
        WaitForSeconds wfs = new WaitForSeconds(spawnInterval);
        while (true)
        {
            EnemyBase enemy = Managers.Instance.Pool.GetFromPool(prefab);
            enemy.transform.position = RandPos(5f, 7f, 4f) + player.position;
            yield return wfs;
        }
    }
    private Vector3 RandPos(float xPos, float yPos, float minDistance)
    {
        while (true)
        {
            posX = Random.Range(-xPos, xPos);
            posY = Random.Range(-yPos, yPos);

            tmpPos = new Vector3(posX, posY, 0f);
            if (Vector3.Distance(tmpPos, player.position) > minDistance) break;
        }
        return tmpPos;
    }
}
