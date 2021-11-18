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
        if (destroyable == false)
        {
            //if (collision.tag != "Enemy" && collision.tag != "EnemyMelee" && collision.tag != "Bullet" && collision.tag != "EnemyBullet")
            if (collision.CompareTag("Player") || collision.CompareTag("Walls") || collision.CompareTag("Globin"))
            {

                GameObject BParticle2 = ObjectPool.instance.GetBulletEffectFromPool();
                if (BParticle2 != null)
                {
                    BParticle2.transform.localScale = new Vector3(0.1666f, 0.1666f, 1);
                    BParticle2.transform.position = gameObject.transform.position;
                    BParticle2.transform.parent = ObjectPool.instance.transform;
                }
                //BParticle.transform.parent = this.transform;
                //GameObject BParticle2 = transform.Find("BulletParticle").gameObject;




                //GameObject BParticle2 = Instantiate(BParticle.gameObject, transform.position, transform.rotation);
                //BParticle2.transform.localScale = new Vector3(0.1666f, 0.1666f, 1);
                //BParticle2.GetComponent<ParticleSystem>().Stop();


                //BParticle2.GetComponent<ParticleSystem>().Play();


                //BParticle2.transform.parent = null;



                //Destroy(BParticle2.gameObject, BParticle.GetComponent<ParticleSystem>().main.duration * 2);
                DestroyEnemyProj();
            }
        }
        else
        {
            //if (collision.tag != "Enemy" && collision.tag != "EnemyMelee" && collision.tag != "EnemyBullet")
            if (collision.CompareTag("Player") || collision.CompareTag("Bullet") || collision.CompareTag("Walls"))
            {
                
                GameObject BParticle2 = ObjectPool.instance.GetBulletEffectFromPool();
                if (BParticle2 != null)
                {
                    BParticle2.transform.localScale = new Vector3(0.1666f, 0.1666f, 1);
                    BParticle2.transform.position = gameObject.transform.position;
                    BParticle2.transform.parent = ObjectPool.instance.transform;
                }
                //BParticle.transform.parent = this.transform;
                //GameObject BParticle2 = transform.Find("BulletParticle").gameObject;




                //partti = BParticle2;
                //BParticle2.GetComponent<ParticleSystem>().Stop();
                //BParticle2.GetComponent<ParticleSystem>().Play();


                //BParticle2.transform.parent = null;



                //destroy object through pooling

                //Destroy(BParticle2.gameObject, BParticle.GetComponent<ParticleSystem>().main.duration * 2);


                DestroyEnemyProj();
                //BParticle.transform.parent = this.transform;
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.name == "Player")
        DestroyEnemyProj();
    }


    void DestroyEnemyProj()
    {
        
        if (gameObject.name.Contains("EnemyBullet"))
        {
            ObjectPool.instance.ReturnBulletToPool(gameObject);
        }
        else
            Destroy(gameObject);
    }

}
