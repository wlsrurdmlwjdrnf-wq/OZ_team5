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
    [SerializeField] private ItemBox itemBoxPrefab;
    [SerializeField] private ExpStone expStone1Prefab;
    [SerializeField] private ExpStone expStone5Prefab;
    [SerializeField] private ExpStone expStone10Prefab;
    [SerializeField] private ExpStone expStone50Prefab;

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
        PoolManager.Instance.CreatePool(expStone1Prefab, 100);
        PoolManager.Instance.CreatePool(expStone5Prefab, 100);
        PoolManager.Instance.CreatePool(expStone10Prefab, 100);
        PoolManager.Instance.CreatePool(expStone50Prefab, 50);

        StartCoroutine(SpawnItemBox());
        StartCoroutine(SpawnEnemy(zombiePrefab, 3f));
        StartCoroutine(SpawnEnemy(bigZombiePrefab, 10f));
        StartCoroutine(SpawnEnemy(zomPlantPrefab, 5f));
        StartCoroutine(SpawnEnemy(zomDogPrefab, 3f));
        StartCoroutine(SpawnEnemy(suicideBomberPrefab, 7f));
    }
    private IEnumerator SpawnItemBox()
    {
        while (true)
        {
            ItemBox itembox = PoolManager.Instance.GetFromPool(itemBoxPrefab);
            itembox.transform.position = RandPos(4f, 5f, 3f) + player.transform.position;
            yield return new WaitForSeconds(20f);
        }
    }
    private IEnumerator SpawnEnemy(EnemyBase prefab,float time)
    {
        WaitForSeconds wfs = new WaitForSeconds(time);
        while (true)
        {
            EnemyBase enemy = PoolManager.Instance.GetFromPool(prefab);
            enemy.transform.position = RandPos(2f, 3f, 1.5f) + player.transform.position;
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
