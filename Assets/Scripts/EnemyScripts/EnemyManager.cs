using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public Transform[] m_SpawnPoints;
    public GameObject m_EnemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        SpawnNewEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        Enemy1.OnEnemyKilled += SpawnNewEnemy;
    }

    void SpawnNewEnemy()
    {
        int randInt = Mathf.RoundToInt(Random.Range(0f, m_SpawnPoints.Length - 1));

        Instantiate(m_EnemyPrefab, m_SpawnPoints[randInt].transform.position, Quaternion.identity);

    }
}
