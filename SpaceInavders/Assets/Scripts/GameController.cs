using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MapLimits limits;
    public GameObject enemyOne;
    public GameObject enemyTwo;
    public GameObject enemyThree;
    public float spawnTimer;
    float maxSpawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        maxSpawnTimer = spawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        // decrement spawn timer by time, once - reassign it with max spawn timer so we can repeat it
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            RespawnEnemy();
            spawnTimer = maxSpawnTimer;
        }
    }

    void RespawnEnemy()
    {
        int randomNumber = Random.Range(0, 3);

        switch (randomNumber)
        {
            case 0:
                {
                    Instantiate(enemyOne,
                        new Vector3(
                            Random.Range(limits.minX, limits.maxX),
                            Random.Range(limits.minY, limits.maxY),
                            0),
                enemyOne.transform.rotation);
                } break;
            case 1:
                {
                    Instantiate(enemyTwo,
                        new Vector3(
                            Random.Range(limits.minX, limits.maxX),
                            Random.Range(limits.minY, limits.maxY),
                            0),
                        enemyTwo.transform.rotation);
                } break;
            case 2:
                {
                    Instantiate(enemyThree,
                        new Vector3(
                            Random.Range(limits.minX, limits.maxX),
                            Random.Range(limits.minY, limits.maxY),
                            0),
                        enemyThree.transform.rotation);
                } break;
            default:
                {
                    Instantiate(enemyOne,
                new Vector3(
                        Random.Range(limits.minX, limits.maxX),
                        Random.Range(limits.minY, limits.maxY),
                        0),
                            enemyOne.transform.rotation);
                }
                break;
        }
    }
}
