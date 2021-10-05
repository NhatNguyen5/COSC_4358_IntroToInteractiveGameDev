using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //public int enemiesFromColony;
    public float colonyHealth;
    public StatusIndicator StaInd;
    public Camera Mcamera;
    public CameraFollow CamFollow;
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

    bool isWaiting = false;
    bool cutSceneFlag = false;


    private void Start()
    {
        //enemiesRemainingToSpawn = enemiesFromColony;
    }

    private void Update()
    {
        if(/*colonyHealth > 0 && */timeBetweenSpawns > 0 && Time.time > nextDecrease && (timeBetweenSpawns > MinTbs || timeBetweenSpawns < MaxTbs))
        {
            nextDecrease = Time.time + DecreaseAfter;
            timeBetweenSpawns -= tbsDecreaseRate;
            if(timeBetweenSpawns < MinTbs)
            {
                timeBetweenSpawns = MinTbs;
            }
            if (timeBetweenSpawns > MaxTbs)
            {
                timeBetweenSpawns = 1000000;
            }

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
            StartCoroutine(wait(5));
        }

        if (!isWaiting && bossDeath && !cutSceneFlag)
        {
            StartCoroutine(CamFollow.MoveTo(new Vector3(-38.47f, 20.26f, -1), 1.25f, 3));
            StartCoroutine(CamFollow.ZoomTo(25, 0.6f));
            cutSceneFlag = true;
        }



        //Debug.Log(bossDeath);

    }

    private void BossDeath()
    {
        tbsDecreaseRate = -2*tbsDecreaseRate;
        StaInd.StartShake(Mcamera, 1, 0.5f);
        
    }

    private void OnEnemyDeath()
    {
        print("Enemy died");
        //enemiesRemainingToSpawn++;
        print("enemies remaing to spawn incremented");
    }

    private IEnumerator wait(float dur)
    {
        isWaiting = true;
        yield return new WaitForSeconds(dur);
        isWaiting = false;
    }
}
