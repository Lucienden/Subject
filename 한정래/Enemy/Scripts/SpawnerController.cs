using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField]
    private int numberOfEnemy = 0;
    [SerializeField]
    private int oneTimeSpawnNumber = 0;
    [SerializeField]
    private float spawnTime = 0;

    private GameObject enemyObj;

    private EnemyMemoryPool enemyMemoryPool;


    private void Awake()
    {
        enemyObj = transform.GetChild(0).gameObject;
        enemyMemoryPool = new EnemyMemoryPool(enemyObj, transform);

        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        int totalNumberOfEnemy = 0;
        while (true)
        {
            for (int i = 0; i < oneTimeSpawnNumber; i++)
            {
                GameObject item = enemyMemoryPool.ActivatePoolItem(transform);
                item.transform.position = transform.position;

                totalNumberOfEnemy++;
                if (totalNumberOfEnemy == numberOfEnemy) yield break;
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void OnDeactivateItem(GameObject removeItem)
    {
        enemyMemoryPool.DeActivatePoolItem(removeItem);
    }
}
