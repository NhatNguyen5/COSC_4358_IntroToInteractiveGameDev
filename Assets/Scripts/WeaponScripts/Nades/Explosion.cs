using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{


    public int explosionDamage;
    public float explodeTime = 0.2f;
    //private float explosionSize;
    //private bool isBoom = false;
    public float explosionRangeStart = 0;
    public float explosionRangeEnd = 0;

    private float explosionRangeResult;


    public bool hurtPlayer = false;
    public bool hurtEnemies = false;


    // Start is called before the first frame update
    void Start()
    {
        explosionRangeResult = Random.Range(explosionRangeStart, explosionRangeEnd);
        //Debug.Log(explosionRangeResult);
        transform.localScale = new Vector3(explosionRangeResult, explosionRangeResult, 1);
        //explosionSize = transform.localScale.x;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hurtPlayer == true)
            if (collision.tag == "Player") { collision.GetComponent<TakeDamage>().takeDamage(explosionDamage, collision.transform, 10); }
        if (hurtEnemies == true)
        {
            if (collision.tag == "EnemyMelee") { collision.GetComponent<Enemy2>().takeDamage(explosionDamage, collision.transform, 10); }
            if (collision.tag == "Enemy") { collision.GetComponent<Enemy1>().takeDamage(explosionDamage, collision.transform, 10); }
            if (collision.tag == "Colony") { collision.GetComponent<EnemyColony>().takeDamage(explosionDamage, collision.transform, 10); }
        }
    }


    // Update is called once per frame
    void Update()
    {



        /*
        if (isBoom == false)
        {
            isBoom = true;
            Collider2D[] objectsToHurt = Physics2D.OverlapCircleAll(transform.position, explosionSize);
            foreach (var objectToHurt in objectsToHurt)
            {

                if (objectToHurt.tag == "EnemyMelee") { objectToHurt.GetComponent<Enemy2>().takeDamage(explosionDamage, objectToHurt.transform, 10); }
                if (objectToHurt.tag == "Enemy") { objectToHurt.GetComponent<Enemy1>().takeDamage(explosionDamage, objectToHurt.transform, 10); }
                if (objectToHurt.tag == "Colony") { objectToHurt.GetComponent<EnemyColony>().takeDamage(explosionDamage, objectToHurt.transform, 10); }

                if (objectToHurt.tag == "Player") { objectToHurt.GetComponent<TakeDamage>().takeDamage(explosionDamage, objectToHurt.transform, 10); }
            }


        }
        */

        explodeTime -= Time.deltaTime;
        if (explodeTime < 0)
            DestroyEnemyProj();
    }

    void DestroyEnemyProj()
    {
        Destroy(gameObject);
    }


}
