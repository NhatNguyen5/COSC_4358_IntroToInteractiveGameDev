using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int enemiesFromColony;
    public float timeBetweenSpawns;
    public Enemy1 enemy;
    int enemiesRemainingToSpawn;
    float nextSpawnTime;
    public GameObject[] spawnPoints = new GameObject[3];
    public int colonyHealth;
    private System.Random rand = new System.Random();

    private void Start()
    {
        enemiesRemainingToSpawn = enemiesFromColony;
    }

    private void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime && colonyHealth > 0)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + timeBetweenSpawns;

            int randomSpawn = rand.Next(0, spawnPoints.Length);
            //print("randomSpawn = " + randomSpawn);

            Enemy1 spawnedEnemy = Instantiate(enemy, spawnPoints[randomSpawn].transform.position, Quaternion.identity) as Enemy1;
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
        
    }

    void OnEnemyDeath()
    {
        print("Enemy died");
        enemiesRemainingToSpawn++;
        print("enemies remaing to spawn incremented");
    }
}
