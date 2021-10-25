using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyProj2 : MonoBehaviour
{
    public GameObject DamageText;
    private SpriteRenderer sprite;
    public GameObject explosion;
    //public GameObject objectThatIsShooting;

    public float HP;
    public float speed;
    public bool despawn = false;
    public float despawnTime;
    public bool isExplosiveBullet = false;
    public int damage;
    //public float explosionRadius;

    //explosion delay
    public float explodeTimeStartRange = 0.2f;
    public float explodeTimeEndRange = 0.2f;
    private float explodeTimeResult = 0;

    public bool homing;
    //private float distance = 0;
    //public bool destroyable = false;
    private float critRate = 0;
    private float critDMG = 0;

    private Vector2 followMovement;
    //private Vector2 playerPos;
    private bool reachedDestination = false;
    private Transform player;
    private Vector2 target;
    private Rigidbody2D rb;

    bool isBoom = false;
    public bool explodeOnSpawn;
    //bool lockRadius = false;
    //private float hold = 0.1f;
    //private float hold2 = 0.1f;
    private float distance;
    private float distance2; //bullet to boss
    private Vector3 spawnPos;


    private Vector2 randPos;
    public float circleRadius;


    // Start is called before the first frame update
    void Start()
    {
        //hold = transform.localScale.x;
        //hold2 = transform.localScale.y;
        spawnPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        //playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        target = new Vector2(player.position.x, player.position.y);
        rb = GetComponent<Rigidbody2D>();

        distance = Vector3.Distance(player.position, transform.position);
        if (homing == false)
        {
            rb.velocity = transform.right * speed;
        }

        explodeTimeResult = Random.Range(explodeTimeStartRange, explodeTimeEndRange);

        //Debug.Log(distance);
    }

    // Update is called once per frame
    void Update()
    {
        distance2 = Vector3.Distance(spawnPos, transform.position);


        if (GlobalPlayerVariables.GameOver == true)
        {
            DestroyEnemyProj();
        }


        if (homing == true)
        {
            if (player != null && transform != null)
            {
                Vector3 direction = player.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rb.rotation = angle;
                direction.Normalize();
                followMovement = direction;
            }
        }
        else if (reachedDestination == true)
        {
            transform.position = this.transform.position;

        }

        if (despawn == true)
            despawnTime -= Time.deltaTime;

        if (despawnTime <= 0 && despawn == true)
        {
            DestroyEnemyProj();
        }


        if (distance2 >= distance)
        {
            reachedDestination = true;
        }




        if (reachedDestination == true && isExplosiveBullet == true)
        {
            //Debug.Log("MADE IT TO POSITION");
            explode();
            //DestroyEnemyProj();
        }
        else if (reachedDestination == true)
        {
            DestroyEnemyProj();
        }

        //distance -= Time.deltaTime;
        

    }

    void explode()
    {
        //if (explodeTime < 0)
        // DestroyEnemyProj();
        
        if (isBoom == false)
        {
            //Debug.Log("boom");
            isBoom = true;
            //transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            sprite.color = Color.red;
        }

        if (isBoom == true)
        {
            //rb.velocity *= 0;
            explodeTimeResult -= Time.deltaTime;
            if (explodeTimeResult <= 0)
            {
                //SPAWN EXPLOSION OBJECT
                randPos = transform.position;
                randPos += Random.insideUnitCircle * circleRadius;
                Instantiate(explosion, randPos, Quaternion.Euler(0, 0, 0));
                DestroyEnemyProj();
            }

        }

        /*
         * 
         *
        
         * 
         * 
         */




        //CircleCollider2D newrad = GetComponent<CircleCollider2D>();
        //newrad.radius = explosionRadius;



    }


    private void FixedUpdate()
    {
        if (homing == true && isBoom == false)
            followCharacter(followMovement);
    }

    void followCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position - (direction * speed * -1 * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //if (collision.tag != "Enemy" && collision.tag != "EnemyMelee" && collision.tag != "Bullet" && collision.tag != "EnemyBullet")
        if ((collision.tag == "Player" || collision.tag == "Walls" || collision.tag == "Globin") && isExplosiveBullet == false)
            DestroyEnemyProj();
        else if (collision.tag == "Player" || collision.tag == "Walls" || collision.tag == "Globin")
        {
            reachedDestination = true;
            randPos = transform.position;
            randPos += Random.insideUnitCircle * circleRadius;
            Instantiate(explosion, randPos, Quaternion.Euler(0, 0, 0));
            DestroyEnemyProj();
            //explode(); 
        }
        


        if (collision.tag == "Bullet")
        {
            float damage = collision.gameObject.GetComponent<Bullet>().damage;
            float speed = collision.gameObject.GetComponent<Bullet>().speed;
            critRate = collision.gameObject.GetComponent<Bullet>().critRate;
            critDMG = collision.gameObject.GetComponent<Bullet>().critDMG;
            //knockbackForce = collision.gameObject.GetComponent<Bullet>().knockbackForce;
            //if(isBoom == false)
                takeDamage(damage, collision.transform, speed);

            //reachedDestination = true;

            //knockbacktime = Random.Range(knockbackstartrange, knockbackendrange);
            //knockback = true;
            //hitPlayer = false;

        }



    }






    void takeDamage(float damage, Transform impact, float speed)
    {

        //Debug.Log(damage);
        bool iscrit = false;
        float chance2crit = Random.Range(0f, 1f);
        if (chance2crit <= critRate)
        {
            iscrit = true;
            damage *= critDMG;
        }

        HP -= damage;
        showDamage(damage, impact, speed, iscrit);
        StartCoroutine(FlashRed());
        if (HP <= 0)
        {
            DestroyEnemyProj();
            //Die();
        }
    }


    void showDamage(float damage, Transform impact, float speed, bool crit)
    {
        damage = Mathf.Round(damage);
        if (damage > 1)
        {
            Vector3 direction = (transform.position - impact.transform.position).normalized;

            //might add to impact to make it go past enemy
            var go = Instantiate(DamageText, impact.position, Quaternion.identity);
            if (crit == false)
            {
                go.GetComponent<TextMeshPro>().text = damage.ToString();
            }
            else
            {
                //Debug.Log("CRIT");
                go.GetComponent<TextMeshPro>().text = damage.ToString();
                go.GetComponent<TextMeshPro>().color = Color.red;
                go.GetComponent<TextMeshPro>().fontSize *= 1.5f;
            }
            go.GetComponent<DestroyText>().spawnPos(direction.x, direction.y, speed / 5);
        }
    }


    public IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }













    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.name == "Player")
        DestroyEnemyProj();
    }


    void DestroyEnemyProj()
    {
        Destroy(gameObject);
    }
}
