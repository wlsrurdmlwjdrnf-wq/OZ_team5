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
        if (PoolManager.Instance.isCreatePool == false)
        {
            PoolManager.Instance.CreatePool(itemBoxPrefab, 50);

            PoolManager.Instance.CreatePool(zombiePrefab, 100);
            PoolManager.Instance.CreatePool(bigZombiePrefab, 10);
            PoolManager.Instance.CreatePool(zomPlantPrefab, 20);
            PoolManager.Instance.CreatePool(zomDogPrefab, 50);
            PoolManager.Instance.CreatePool(suicideBomberPrefab, 20);
            PoolManager.Instance.CreatePool(wallMonsterPrefab, 100);

            PoolManager.Instance.CreatePool(boss1Prefab, 2);
            PoolManager.Instance.CreatePool(boss2Prefab, 2);
            PoolManager.Instance.CreatePool(boss3Prefab, 2);

            PoolManager.Instance.CreatePool(bombPrefab, 10);
            PoolManager.Instance.CreatePool(coin10Prefab, 10);
            PoolManager.Instance.CreatePool(coin30Prefab, 10);
            PoolManager.Instance.CreatePool(coin100Prefab, 10);
            PoolManager.Instance.CreatePool(magnetPrefab, 10);
            PoolManager.Instance.CreatePool(meatPrefab, 10);

            PoolManager.Instance.CreatePool(expStone1Prefab, 100);
            PoolManager.Instance.CreatePool(expStone5Prefab, 100);
            PoolManager.Instance.CreatePool(expStone10Prefab, 100);
            PoolManager.Instance.CreatePool(expStone50Prefab, 50);

            PoolManager.Instance.CreatePool(enemySmallPjtPrefab, 150);
            PoolManager.Instance.CreatePool(enemyBigPjtPrefab, 100);
            PoolManager.Instance.CreatePool(enemyGlowPjtPrefab, 20);
            
            PoolManager.Instance.isCreatePool = true;
        }

        StartCoroutine(SpawnItemBox());
        StartCoroutine(SpawnEnemy(zombiePrefab, 1f));
        StartCoroutine(SpawnEnemy(bigZombiePrefab, 4f));
        StartCoroutine(SpawnBoss(boss1Prefab, 30f));
    }
    private IEnumerator SpawnBoss(EnemyBase boss, float waitBossTime)
    {
        yield return new WaitForSeconds(waitBossTime);
        WarningPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        ClearField();
        WarningPanel.SetActive(false);

        WallMonsterSpawn.SpawnMonsterWall(wallMonsterPrefab, Camera.main.transform.position, 12f, 10f, 1f);

        EnemyBase bossObj = PoolManager.Instance.GetFromPool(boss);
        bossObj.transform.position = RandPos(4f, 4f, 4f) + player.position; 

        StopAllCoroutines();
        //보스 출현 경고 후 보스와 벽몬스터 스폰, 타이머 정지, 모든 적과 아이템 풀로 리턴+모든 코루틴 stop해야함.
        //보스를 잡은 후에 타이머 다시 작동
    }
    private void ClearField()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(player.position, 30f);

        foreach (Collider2D col in cols)
        {
            if (col.TryGetComponent<EnemyBase>(out EnemyBase target))
            {
                PoolManager.Instance.ReturnPool(target);
            }
            if (col.TryGetComponent<ItemBox>(out ItemBox box))
            {
                PoolManager.Instance.ReturnPool(box);
            }
            if (col.TryGetComponent<ItemBase>(out ItemBase item))
            {
                PoolManager.Instance.ReturnPool(item);
            }

        }
    }
    public void KillBoss1()
    {
        ClearField();
        StartCoroutine(SpawnItemBox());
        StartCoroutine(SpawnEnemy(zomPlantPrefab, 2f));
        StartCoroutine(SpawnEnemy(zomDogPrefab, 1f));
        StartCoroutine(SpawnBoss(boss2Prefab, 20f));
    }
    public void KillBoss2()
    {
        ClearField();
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
            ItemBox itembox = PoolManager.Instance.GetFromPool(itemBoxPrefab);
            itembox.transform.position = RandPos(6f, 6f, 5f) + player.position;
            yield return new WaitForSeconds(10f);
        }
    }
    private IEnumerator SpawnEnemy(EnemyBase prefab,float spawnInterval)
    {
        WaitForSeconds wfs = new WaitForSeconds(spawnInterval);
        while (true)
        {
            EnemyBase enemy = PoolManager.Instance.GetFromPool(prefab);
            enemy.transform.position = RandPos(4f, 5f, 3f) + player.position;
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
