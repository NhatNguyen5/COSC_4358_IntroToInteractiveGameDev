using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public GameObject DamageText;
    private SpriteRenderer sprite;

    public delegate void EnemyKilled();
    public static event EnemyKilled OnEnemyKilled;

    public float HP = 100;
    public float speed = 0;
    public float stoppingDistance = 0;
    public float retreatDistance = 0;
    public bool retreat;


    private float knockbackForce = 0;
    private bool knockback = false;
    public float knockbackstartrange = 0.4f;
    public float knockbackendrange = 1.0f;
    private float knockbacktime;


    private bool hitTarget = false;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject projectile;
    public Transform firePoint;
    public Transform player;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();

        timeBtwShots = startTimeBtwShots;
    }


    private void FixedUpdate()
    {

        if (knockback == true)
        {
            
            //Debug.Log("KNOCKBACK");
            transform.position = Vector2.MoveTowards(transform.position, player.position, -knockbackForce * speed * Time.deltaTime);

        }
        else
        {

            if (Vector2.Distance(transform.position, player.position) > stoppingDistance) //follow player
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance) //stop
            {
                transform.position = this.transform.position;
            }
            else if (Vector2.Distance(transform.position, player.position) < retreatDistance && retreat == true) //retreat
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            }

        }
    }


    // Update is called once per frame
    void Update()
    {
        knockbacktime -= Time.deltaTime;
        if (knockbacktime <= 0)
        {
            knockbacktime = 0;
            knockback = false;
        }
        
        if (timeBtwShots <= 0)
        {
            Instantiate(projectile, firePoint.position, firePoint.rotation);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }

        hitTarget = false;

    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

       

        if (collision.tag == "Bullet")
        {
            int damage = collision.gameObject.GetComponent<Bullet>().damage;
            float speed = collision.gameObject.GetComponent<Bullet>().speed;
            knockbackForce = collision.gameObject.GetComponent<Bullet>().knockbackForce;
            //Transform bulletpos = collision.gameObject.transform;
            if (hitTarget == false)
            {
                takeDamage(damage, collision.transform, speed);
            }
            //knockbacktime = knockbacktimer;
            knockbacktime = Random.Range(knockbackstartrange, knockbackendrange);
            knockback = true;

            //Vector2 difference = (transform.position - collision.transform.position).normalized;
            //Vector2 force = difference * knockbackForce;
            //rb.AddForce(force); //if you don't want to take into consideration enemy's mass then use ForceMode.VelocityChange
        }



        hitTarget = true;
    }

    void takeDamage(float damage, Transform impact, float speed)
    {
        //Debug.Log(damage);
        bool iscrit = false;
        float chance2crit = Random.Range(0f, 1f);
        if (chance2crit <= GlobalPlayerVariables.critRate)
        {
            iscrit = true;
            damage *= GlobalPlayerVariables.critDmg;
        }

        HP -= damage;
        showDamage(damage, impact, speed, iscrit);
        StartCoroutine(FlashRed());
        if (HP <= 0)
        {
            Die();
        }
    }


    void showDamage(float damage, Transform impact, float speed, bool crit)
    {

        Vector3 direction = (transform.position - impact.transform.position).normalized;

        //might add to impact to make it go past enemy
        var go = Instantiate(DamageText, impact.position, Quaternion.identity);
        if (crit == false)
        {
            go.GetComponent<TextMesh>().text = damage.ToString();
        }
        else
        {
            //Debug.Log("CRIT");
            go.GetComponent<TextMesh>().text = damage.ToString();
            go.GetComponent<TextMesh>().color = Color.red;
            go.GetComponent<TextMesh>().fontSize *= 3;
        }
        go.GetComponent<DestroyText>().spawnPos(direction.x, direction.y, speed/5);
    }


    public IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    void Die()
    {
        Destroy(gameObject);
        if (OnEnemyKilled != null)
        {
            OnEnemyKilled();
        }
    }
}
