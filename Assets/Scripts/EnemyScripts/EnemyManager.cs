using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //public int enemiesFromColony;
    public int colonyHealth;
    [Header("Spawn time setting")]
    public float timeBetweenSpawns;
    public float tbsDecreaseRate;
    [Tooltip("This is in second")]
    public float DecreaseAfter;
    public float MinTbs;
    //int enemiesRemainingToSpawn;
    private float nextSpawnTime;
    private float nextDecrease;
    public GameObject[] spawnPoints;
    public GameObject[] Enemies;
    //public int remain;
    [Header("In between is Mosaic spawnrate")]
    [Range(20, 100)]
    public int chanceToSpawnBrunt;
    [Range(0, 20)]
    public int chanceToSpawnR_Nold;
    

    private void Start()
    {
        //enemiesRemainingToSpawn = enemiesFromColony;
    }

    private void Update()
    {
        if(colonyHealth > 0 && timeBetweenSpawns > 0 && Time.time > nextDecrease && timeBetweenSpawns > MinTbs)
        {
            nextDecrease = Time.time + DecreaseAfter;
            timeBetweenSpawns -= tbsDecreaseRate;
        }
        //Debug.Log(timeBetweenSpawns);
        if (/*enemiesRemainingToSpawn > 0 &&*/ Time.time > nextSpawnTime && colonyHealth > 0)
        {
            //enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + timeBetweenSpawns;
            int randomSpawn = Random.Range(0, spawnPoints.Length);
            //print("randomSpawn = " + randomSpawn);
            int randomEnemeies = Random.Range(0, 100);
            //Debug.Log(randomEnemeies);
            if(randomEnemeies >= chanceToSpawnBrunt)
                Instantiate(Enemies[0], spawnPoints[randomSpawn].transform.position, Quaternion.identity);
            else if(randomEnemeies <= chanceToSpawnR_Nold)
                Instantiate(Enemies[2], spawnPoints[randomSpawn].transform.position, Quaternion.identity);
            else
                Instantiate(Enemies[1], spawnPoints[randomSpawn].transform.position, Quaternion.identity);


            //spawnedEnemy.OnDeath += OnEnemyDeath;
        }
        
    }

    private void OnEnemyDeath()
    {
        print("Enemy died");
        //enemiesRemainingToSpawn++;
        print("enemies remaing to spawn incremented");
    }
}
