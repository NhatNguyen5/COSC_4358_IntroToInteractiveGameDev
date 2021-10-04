using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //public int enemiesFromColony;
    public float colonyHealth;
    [Header("Spawn time setting")]
    public float timeBetweenSpawns;
    public float tbsDecreaseRate;
    [Tooltip("This is in second")]
    public float DecreaseAfter;
    public float MinTbs;
    public float MaxTbs;
    //int enemiesRemainingToSpawn;
    private float nextSpawnTime;
    private float nextDecrease;
    public GameObject[] spawnPoints;
    public GameObject[] Enemies;
    public Animator[] SpawnPointAnim;
    private bool SPAnimReset = false;
    private int ChosenSP;
    private bool bossDeath = false;
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
        if(/*colonyHealth > 0 && */timeBetweenSpawns > 0 && Time.time > nextDecrease && timeBetweenSpawns > MinTbs && timeBetweenSpawns <= MaxTbs)
        {
            nextDecrease = Time.time + DecreaseAfter;
            timeBetweenSpawns -= tbsDecreaseRate;
        }
        if(timeBetweenSpawns > MaxTbs )
        {
            tbsDecreaseRate = 0;
            timeBetweenSpawns = 1000000;
        }
        int randomSpawn = Random.Range(0, spawnPoints.Length);
        if (SPAnimReset)
        {
            ChosenSP = randomSpawn;
            SpawnPointAnim[randomSpawn].SetBool("WasChosen", true);
            SpawnPointAnim[randomSpawn].SetFloat("SpawnRate", 1 / timeBetweenSpawns);
            SPAnimReset = false;
        }
        //Debug.Log(timeBetweenSpawns);
        if (/*enemiesRemainingToSpawn > 0 && colonyHealth > 0 &&*/ Time.time > nextSpawnTime)
        {
            //enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + timeBetweenSpawns;
            
            //print("randomSpawn = " + randomSpawn);
            int randomEnemeies = Random.Range(0, 100);
            //Debug.Log(randomEnemeies);
            if(randomEnemeies >= chanceToSpawnBrunt)
                Instantiate(Enemies[0], spawnPoints[ChosenSP].transform.position, Quaternion.identity);
            else if(randomEnemeies <= chanceToSpawnR_Nold)
                Instantiate(Enemies[2], spawnPoints[ChosenSP].transform.position, Quaternion.identity);
            else
                Instantiate(Enemies[1], spawnPoints[ChosenSP].transform.position, Quaternion.identity);
            //Debug.Log("Spawned");
            SpawnPointAnim[ChosenSP].SetBool("WasChosen", false);
            SPAnimReset = true;
            //spawnedEnemy.OnDeath += OnEnemyDeath;
        }

        if (colonyHealth <= 0 && !bossDeath)
        {
            BossDeath();
            bossDeath = true;
        }
        //Debug.Log(bossDeath);
        
    }

    private void BossDeath()
    {
        tbsDecreaseRate = -2*tbsDecreaseRate;
    }

    private void OnEnemyDeath()
    {
        print("Enemy died");
        //enemiesRemainingToSpawn++;
        print("enemies remaing to spawn incremented");
    }
}
