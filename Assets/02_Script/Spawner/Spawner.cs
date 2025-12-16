using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private Boss1 boss1Prefab;
    [SerializeField] private Boss2 boss2Prefab;
    [SerializeField] private Boss3 boss3Prefab;

    [SerializeField] private ItemBox itemBoxPrefab;

    [SerializeField] private ExpStone expStone1Prefab;
    [SerializeField] private ExpStone expStone5Prefab;
    [SerializeField] private ExpStone expStone10Prefab;
    [SerializeField] private ExpStone expStone50Prefab;

    [SerializeField] private EnemyProjectile enemySmallPjtPrefab;
    [SerializeField] private EnemyProjectile enemyBigPjtPrefab;

    private float posX;
    private float posY;
    private Vector3 tmpPos;
    private Player player;

    private void Awake()
    {
        player = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity);  
    }
    private void Start()
    {
        PoolManager.Instance.CreatePool(itemBoxPrefab, 50);

        PoolManager.Instance.CreatePool(zombiePrefab, 100);
        PoolManager.Instance.CreatePool(bigZombiePrefab, 10);
        PoolManager.Instance.CreatePool(zomPlantPrefab, 20);
        PoolManager.Instance.CreatePool(zomDogPrefab, 50);
        PoolManager.Instance.CreatePool(suicideBomberPrefab, 20);
        PoolManager.Instance.CreatePool(wallMonsterPrefab, 100);

        PoolManager.Instance.CreatePool(expStone1Prefab, 100);
        PoolManager.Instance.CreatePool(expStone5Prefab, 100);
        PoolManager.Instance.CreatePool(expStone10Prefab, 100);
        PoolManager.Instance.CreatePool(expStone50Prefab, 50);

        PoolManager.Instance.CreatePool(enemySmallPjtPrefab, 50);
        PoolManager.Instance.CreatePool(enemyBigPjtPrefab, 30);

        StartCoroutine(SpawnItemBox());
        StartCoroutine(SpawnEnemy(zombiePrefab, 3f));
        StartCoroutine(SpawnEnemy(bigZombiePrefab, 10f));
        StartCoroutine(SpawnEnemy(zomPlantPrefab, 5f));
        StartCoroutine(SpawnEnemy(zomDogPrefab, 3f));
        StartCoroutine(SpawnEnemy(suicideBomberPrefab, 7f));
        StartCoroutine(SpawnBoss());
    }
    private IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(10f);
        //보스 출현 경고 후 보스와 벽몬스터 스폰, 모든 적과 아이템 풀로 리턴+모든 코루틴 stop해야함
        WallMonsterSpawn.SpawnMonsterWall(wallMonsterPrefab, Camera.main.transform.position, 10f, 7f, 0.9f);
        Instantiate(boss1Prefab, player.transform.position + Vector3.right, Quaternion.identity);
        Instantiate(boss2Prefab, player.transform.position + Vector3.right, Quaternion.identity);
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
