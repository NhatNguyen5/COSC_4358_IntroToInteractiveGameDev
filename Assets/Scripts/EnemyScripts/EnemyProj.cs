using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProj : MonoBehaviour
{
    public float speed;
    public float DespawnTimeHolder = 3f;
    public float despawnTime;
    public int damage;
    public bool homing;
    public bool destroyable = false;
    public bool isDeflected = false;

    private Vector2 followMovement;

    private Transform player;
    private Vector2 target;
    //[HideInInspector]
    private Rigidbody2D rb;

    private Transform BParticle;
    private Transform partti;


    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //target = new Vector2(player.position.x, player.position.y);
        rb = GetComponent<Rigidbody2D>();

       
        //rb.velocity = transform.right * speed;

        //BParticle = transform.Find("BulletParticle");
        //BParticle.GetComponent<ParticleSystem>().Stop();

    }

    // Update is called once per frame
    void Update()
    {


        rb.velocity = transform.right * speed;
        /*
        if (homing == true)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            direction.Normalize();
            followMovement = direction;
        }
        */

        despawnTime -= Time.deltaTime;
        if (despawnTime <= 0)
        {
            DestroyEnemyProj();
        }

    }

    private void FixedUpdate()
    {
        if (homing == true)
            followCharacter(followMovement);
    }

    void followCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position - (direction * speed * -1 * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //BParticle.transform.parent = this.transform;
        if (collision.CompareTag("MeleeWeapon"))
            isDeflected = true;
        if (destroyable == false)
        {
            //if (collision.tag != "Enemy" && collision.tag != "EnemyMelee" && collision.tag != "Bullet" && collision.tag != "EnemyBullet")
            if ((collision.CompareTag("Player") || collision.CompareTag("Walls") || collision.CompareTag("Globin")) && isDeflected == false)
            {

                
                DestroyEnemyProj();
            }
            else if ((collision.CompareTag("Enemy") || collision.CompareTag("EnemyMelee") || collision.CompareTag("Colony") || collision.CompareTag("Walls")) && isDeflected == true)
            {
                
                hurtEnemy(collision);
                DestroyEnemyProj();
            }
        }
        else
        {
            //if (collision.tag != "Enemy" && collision.tag != "EnemyMelee" && collision.tag != "EnemyBullet")
            if ((collision.CompareTag("Player") || collision.CompareTag("Bullet") || collision.CompareTag("Walls")) && isDeflected == false)
            {
                
                DestroyEnemyProj();
            }
            else if (isDeflected == true)
            {
                if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyMelee") || collision.CompareTag("Colony") || collision.CompareTag("Walls"))
                {
                    Debug.Log("deflected and found tag");
                    hurtEnemy(collision);
                    DestroyEnemyProj();
                }
            }
        }
    }

    void hurtEnemy(Collider2D collision)
    {
        Debug.Log("hurt da enemy");
        if (collision.CompareTag("EnemyMelee"))
        {
            collision.GetComponent<Enemy2>().takeDamage(damage, collision.transform, 10);
        }
        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<Enemy1>() != null)
                collision.GetComponent<Enemy1>().takeDamage(damage, collision.transform, 10);
            else
                collision.GetComponent<Enemy3>().takeDamage(damage, collision.transform, 10);
        }
        if (collision.CompareTag("Colony")) { collision.GetComponent<EnemyColony>().takeDamage(damage, collision.transform, 10); }
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.name == "Player")
        DestroyEnemyProj();
    }


    public void DestroyEnemyProj()
    {
        
        if (gameObject.name.Contains("EnemyBullet"))
        {
            GameObject BParticle2 = ObjectPool.instance.GetBulletEffectFromPool();
            if (BParticle2 != null)
            {
                BParticle2.transform.localScale = new Vector3(0.1666f, 0.1666f, 1);
                BParticle2.transform.position = gameObject.transform.position;
                BParticle2.transform.parent = ObjectPool.instance.transform;
            }
            ObjectPool.instance.ReturnBulletToPool(gameObject);
        }
        else
            Destroy(gameObject);
    }

}
