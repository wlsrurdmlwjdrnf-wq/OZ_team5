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
        #region 필요한 풀 생성
        if (Managers.Instance.Pool.isCreatePool == false)
        {
            Managers.Instance.Pool.CreatePool(itemBoxPrefab, 50);

            Managers.Instance.Pool.CreatePool(zombiePrefab, 150);
            Managers.Instance.Pool.CreatePool(bigZombiePrefab, 30);
            Managers.Instance.Pool.CreatePool(zomPlantPrefab, 30);
            Managers.Instance.Pool.CreatePool(zomDogPrefab, 100);
            Managers.Instance.Pool.CreatePool(suicideBomberPrefab, 30);
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
        #endregion 

        StartCoroutine(SpawnItemBox());
        StartCoroutine(SpawnEnemy(zombiePrefab, 0.5f));
        StartCoroutine(SpawnEnemy(bigZombiePrefab, 2f));
        StartCoroutine(SpawnBoss(boss1Prefab, 60f));
    }
    private void OnDisable()
    {
        ClearField(Vector2.zero, 50f);
        StopAllCoroutines();
    }
    private IEnumerator SpawnBoss(EnemyBase boss, float waitBossTime)
    {
        yield return new WaitForSeconds(waitBossTime);
        AudioManager.Instance.PlaySFX(EnumData.SFX.WarningSFX);
        WarningPanel.SetActive(true);
        yield return new WaitForSeconds(4f);
        WarningPanel.SetActive(false);

        ClearField(player.position, 50f);

        WallMonsterSpawn.SpawnMonsterWall(wallMonsterPrefab, Camera.main.transform.position, 14f, 12f, 1.0f); // 벽몬스터들을 가로,세로,간격을 입력한대로 벽처럼 생성

        EnemyBase bossObj = Managers.Instance.Pool.GetFromPool(boss);
        bossObj.transform.position = RandPos(6f, 5f, 4f); 

        StopAllCoroutines();
    }

    //필드 정리
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
        StartCoroutine(SpawnEnemy(zomPlantPrefab, 3f));
        StartCoroutine(SpawnEnemy(zomDogPrefab, 0.5f));
        StartCoroutine(SpawnBoss(boss2Prefab, 60f));
    }
    public void KillBoss2()
    {
        ClearField(player.position, 30f);
        StartCoroutine(SpawnItemBox());
        StartCoroutine(SpawnEnemy(zomPlantPrefab, 3f));
        StartCoroutine(SpawnEnemy(zomDogPrefab, 0.5f));
        StartCoroutine(SpawnEnemy(suicideBomberPrefab, 3f));
        StartCoroutine(SpawnBoss(boss3Prefab, 60f));
    }
    private IEnumerator SpawnItemBox()
    {
        while (true)
        {
            ItemBox itembox = Managers.Instance.Pool.GetFromPool(itemBoxPrefab);
            itembox.transform.position = RandPos(8f, 8f, 6f);
            yield return new WaitForSeconds(10f);
        }
    }
    private IEnumerator SpawnEnemy(EnemyBase prefab,float spawnInterval)
    {
        WaitForSeconds wfs = new WaitForSeconds(spawnInterval);
        while (true)
        {
            EnemyBase enemy = Managers.Instance.Pool.GetFromPool(prefab);
            enemy.transform.position = RandPos(5f, 7f, 4f);
            yield return wfs;
        }
    }

    //플레이어와 최소 거리를 보장하는 랜덤 좌표 리턴
    private Vector3 RandPos(float xPos, float yPos, float minDistance)
    {
        while (true)
        {
            posX = Random.Range(-xPos, xPos) + player.position.x;
            posY = Random.Range(-yPos, yPos) + player.position.y;

            tmpPos = new Vector3(posX, posY, 0f);
            if (Vector3.Distance(tmpPos, player.position) > minDistance) break;
        }
        return tmpPos;
    }
}
