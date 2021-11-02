using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public struct enemyType
    {
        public GameObject Enemies;
        public float StartSpawnRange;
        public float EndSpawnRange;
    }

    //public int enemiesFromColony;
    public float colonyHealth;
    public StatusIndicator StaInd;
    public Camera Mcamera;
    public CameraFollow CamFollow;
    public int SpawnCap;
    [HideInInspector]
    public float timeBetweenSpawns;
    [Header("Spawn time setting")]
    [Tooltip("This is in second")]
    public float MaxTbs;
    public float tbsDecreaseRate;
    public float DecreaseAfter;
    public float MinTbs;
    //int enemiesRemainingToSpawn;
    private float nextSpawnTime;
    private float nextDecrease;
    public List<GameObject> spawnPoints;


    public List<float> DistToSpawnFromPlayer;
    public List<float> spawnPointDistances;

    public enemyType[] EnemyTypes;

    public Animator[] SpawnPointAnim;
    private bool SPAnimReset = false;
    private int ChosenSP;
    private bool bossDeath = false;
    //public int remain;

    bool isWaiting = false;
    bool cutSceneFlag = false;
    private Player player;
    private List<GameObject> SpawnedMobs = new List<GameObject>();

    [Header("Cut Scene Settings")]
    public float xPos;
    public float yPos;




    private void Start()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            spawnPointDistances.Add(0f);
        }


        //enemiesRemainingToSpawn = enemiesFromColony;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        timeBetweenSpawns = MaxTbs;
    }

    private void Update()
    {
        if (GlobalPlayerVariables.EnableAI)
        {
            if (player != null)
            {
                //float[] distances
                //ListGameObject
                for (int i = 0; i < spawnPoints.Count; i++)
                {
                    float temp = Vector2.Distance(spawnPoints[i].transform.position, player.transform.position);
                    spawnPointDistances[i] = temp;
                }




                if (/*colonyHealth > 0 && */timeBetweenSpawns > 0 && Time.time > nextDecrease && (timeBetweenSpawns > MinTbs || timeBetweenSpawns < MaxTbs))
                {
                    nextDecrease = Time.time + DecreaseAfter;
                    timeBetweenSpawns -= tbsDecreaseRate;
                    if (timeBetweenSpawns < MinTbs)
                    {
                        timeBetweenSpawns = MinTbs;
                    }
                    if (timeBetweenSpawns > MaxTbs)
                    {
                        timeBetweenSpawns = 1000000;
                    }

                }
                if (timeBetweenSpawns > MaxTbs)
                {
                    tbsDecreaseRate = 0;
                    timeBetweenSpawns = 1000000;
                }
                int randomSpawn = Random.Range(0, spawnPoints.Count);


                if (spawnPointDistances[randomSpawn] <= DistToSpawnFromPlayer[randomSpawn])
                {

                    if (SPAnimReset)
                    {
                        ChosenSP = randomSpawn;
                        SpawnPointAnim[randomSpawn].SetBool("WasChosen", true);
                        SpawnPointAnim[randomSpawn].SetFloat("SpawnRate", 1 / timeBetweenSpawns);
                        SPAnimReset = false;
                    }
                    //Debug.Log(timeBetweenSpawns);
                    if (/*enemiesRemainingToSpawn > 0 && colonyHealth > 0 &&*/ Time.time > nextSpawnTime && SpawnedMobs.Count < SpawnCap)
                    {
                        //enemiesRemainingToSpawn--;
                        nextSpawnTime = Time.time + timeBetweenSpawns;

                        //print("randomSpawn = " + randomSpawn);
                        int randomEnemeies = Random.Range(0, 100);
                        //Debug.Log(randomEnemeies);
                        foreach (enemyType et in EnemyTypes)
                        {
                            if (randomEnemeies >= et.StartSpawnRange && randomEnemeies <= et.EndSpawnRange)
                                SpawnedMobs.Add(Instantiate(et.Enemies, spawnPoints[ChosenSP].transform.position, Quaternion.identity));
                        }
                        //Debug.Log("Spawned");

                        SpawnPointAnim[ChosenSP].SetBool("WasChosen", false);
                        SPAnimReset = true;
                        //spawnedEnemy.OnDeath += OnEnemyDeath;
                    }
                }

                StartCoroutine(RemoveDeadMobs());

                if (colonyHealth <= 0 && !bossDeath)
                {
                    BossDeath();
                    bossDeath = true;
                    StartCoroutine(wait(5));
                }

                if (!isWaiting && bossDeath && !cutSceneFlag)
                {
                    //StartCoroutine(player.Phasing(4f));
                    StartCoroutine(CutScene(4f));
                    //StartCoroutine(player.TakeOver(4f)); //in progress
                    StartCoroutine(CamFollow.MoveTo(new Vector3(xPos, yPos, -1), 2.8f, 2f));
                    StartCoroutine(CamFollow.ZoomTo(20, 1f));
                    cutSceneFlag = true;
                }






            }
            //Debug.Log(bossDeath);
        }

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

    private IEnumerator RemoveDeadMobs()
    {
        yield return 0;

        SpawnedMobs.RemoveAll(item => item == null);

    }

    private IEnumerator CutScene(float duration)
    {
        GlobalPlayerVariables.EnablePlayerControl = false;
        GlobalPlayerVariables.EnableAI = false;
        yield return new WaitForSeconds(duration);
        GlobalPlayerVariables.EnablePlayerControl = true;
        GlobalPlayerVariables.EnableAI = true;
    }
}
