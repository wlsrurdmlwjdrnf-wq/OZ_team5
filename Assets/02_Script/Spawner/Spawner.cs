using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;

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

    [SerializeField] private ExpStone expStone1Prefab;
    [SerializeField] private ExpStone expStone5Prefab;
    [SerializeField] private ExpStone expStone10Prefab;
    [SerializeField] private ExpStone expStone50Prefab;

    [SerializeField] private EnemyProjectile enemySmallPjtPrefab;
    [SerializeField] private EnemyProjectile enemyBigPjtPrefab;
    [SerializeField] private EnemyProjectile enemyGlowPjtPrefab;

    public static Spawner Instance { get; private set; }

    private float posX;
    private float posY;
    private Vector3 tmpPos;
    private Player player;

    private void Awake()
    {
        Instance = this;
        player = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity);  
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
        StartCoroutine(SpawnEnemy(zombiePrefab, 2f));
        StartCoroutine(SpawnEnemy(bigZombiePrefab, 9f));
        StartCoroutine(SpawnBoss(boss1Prefab, 10f));
    }
    private IEnumerator SpawnBoss(EnemyBase boss, float waitBossTime)
    {
        yield return new WaitForSeconds(waitBossTime);
        //보스 출현 경고 후 보스와 벽몬스터 스폰, 타이머 정지, 모든 적과 아이템 풀로 리턴+모든 코루틴 stop해야함.
        //보스를 잡은 후에 타이머 다시 작동
        ClearEnemy();

        WallMonsterSpawn.SpawnMonsterWall(wallMonsterPrefab, Camera.main.transform.position, 12f, 10f, 0.9f);

        EnemyBase bossObj = PoolManager.Instance.GetFromPool(boss);
        bossObj.transform.position = RandPos(3f, 3f, 2f) + player.transform.position; 

        StopAllCoroutines();
    }
    private void ClearEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.TryGetComponent<EnemyBase>(out EnemyBase em))
            {
                em.ReturnPool();
            }
        }
    }
    public void KillBoss1()
    {
        ClearEnemy();
        StartCoroutine(SpawnItemBox());
        StartCoroutine(SpawnEnemy(zomPlantPrefab, 2f));
        StartCoroutine(SpawnEnemy(zomDogPrefab, 1f));
        StartCoroutine(SpawnBoss(boss2Prefab, 10f));
    }
    public void KillBoss2()
    {
        ClearEnemy();
        StartCoroutine(SpawnItemBox());
        StartCoroutine(SpawnEnemy(zomPlantPrefab, 2f));
        StartCoroutine(SpawnEnemy(zomDogPrefab, 1f));
        StartCoroutine(SpawnEnemy(suicideBomberPrefab, 3f));
        StartCoroutine(SpawnBoss(boss3Prefab, 10f));
    }
    private IEnumerator SpawnItemBox()
    {
        while (true)
        {
            ItemBox itembox = PoolManager.Instance.GetFromPool(itemBoxPrefab);
            itembox.transform.position = RandPos(4f, 5f, 4f) + player.transform.position;
            yield return new WaitForSeconds(20f);
        }
    }
    private IEnumerator SpawnEnemy(EnemyBase prefab,float spawnInterval)
    {
        WaitForSeconds wfs = new WaitForSeconds(spawnInterval);
        while (true)
        {
            EnemyBase enemy = PoolManager.Instance.GetFromPool(prefab);
            enemy.transform.position = RandPos(3f, 4f, 2f) + player.transform.position;
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
            if (Vector3.Distance(tmpPos, playerPrefab.transform.position) > minDistance) break;
        }
        return tmpPos;
    }
}
