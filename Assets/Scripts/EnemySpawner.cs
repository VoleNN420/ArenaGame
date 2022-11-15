using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    public int EnemyNumber;


    void Awake()
    {
        spawnEnemies();
    }

    private void spawnEnemies()
    {
        for (int i=0; i < EnemyNumber; i++)
        {
            float randomOffsetX = Random.Range(-15, 15);
            float randomOffsetZ = Random.Range(-15, 15);

            Vector3 spawnPosition = new Vector3(transform.position.x + randomOffsetX, transform.position.y + 3f, transform.position.z + randomOffsetZ);

            Instantiate(Enemy, spawnPosition, transform.rotation);
        }
    }
}
