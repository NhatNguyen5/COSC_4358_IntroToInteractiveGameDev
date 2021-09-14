using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public GameObject DamageText;
    private SpriteRenderer sprite;

    public delegate void EnemyKilled();
    public static event EnemyKilled OnEnemyKilled;

    [Header("Enemy Stats")]

    public float contactDamage;
    public float HP = 100;
    public float speed = 0;

    [Header("Movement Settings")]
    public float stoppingDistance = 0;
    public float retreatDistance = 0;
    public bool followPlayer;
    public bool retreat;
    public bool randomMovement;

    [Header("Random Movement Settings")]
    public float circleRadius;
    public float timeTillNextMove;
    private float NextMoveCoolDown;
    private bool reachedDestination;
    private Vector2 randPos;

    private float knockbackForce = 0;
    [Header("KnockBack Settings")]
    public float knockForcePlayerContact;
    private bool knockback = false;
    public float knockbackstartrange = 0.4f;
    public float knockbackendrange = 1.0f;
    private float knockbacktime;

    private float timeBtwShots;
    [Header("Gun Settings")]
    public float startTimeBtwShots;
    public int amountOfShots;
    public float bulletSpread = 0.0f;
    public GameObject projectile;
    public Transform firePoint;
    public Transform player;
    private Rigidbody2D rb;


    [Header("Burst Settings")]
    public bool burstFire;
    public float timeBtwBurst;
    private float burstTime = 0;

    public int timesToShoot;
    private int TimesShot = 0;



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

            if (Vector2.Distance(transform.position, player.position) > stoppingDistance && followPlayer == true) //follow player
            {
                reachedDestination = true;
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance) //stop
            {
                if (randomMovement == false)
                    transform.position = this.transform.position;
                else //RANDOM MOVEMENT
                {
                    if (reachedDestination == false)
                        transform.position = Vector2.MoveTowards(transform.position, randPos, speed * Time.deltaTime);
                    else
                        transform.position = this.transform.position;

                    if (transform.position.x == randPos.x && transform.position.y == randPos.y)
                    {
                        reachedDestination = true;
                    }

                }
            }
            else if (Vector2.Distance(transform.position, player.position) < retreatDistance && retreat == true) //retreat
            {
                reachedDestination = true;
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

            if (burstFire == true)
            {
                burstTime -= Time.deltaTime;
                if (TimesShot < timesToShoot)
                {
                    if (burstTime < 0)
                    {
                        TimesShot++;
                        burst();
                    }

                }
                else
                {
                    TimesShot = 0;
                    timeBtwShots = startTimeBtwShots;
                }
            }
            else
            {
                shoot();
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }

        if (NextMoveCoolDown <= 0 && reachedDestination == true)
        {
            randomPos();
        }
        NextMoveCoolDown -= Time.deltaTime;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        reachedDestination = true;
        NextMoveCoolDown = timeTillNextMove;

        if (collision.gameObject.tag == "Player")
        {
            knockbackForce = knockForcePlayerContact;
            knockbacktime = 0.1f;
            knockback = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {




        if (collision.tag == "Bullet")
        {
            int damage = collision.gameObject.GetComponent<Bullet>().damage;
            float speed = collision.gameObject.GetComponent<Bullet>().speed;
            knockbackForce = collision.gameObject.GetComponent<Bullet>().knockbackForce;

            takeDamage(damage, collision.transform, speed);

            reachedDestination = true;

            knockbacktime = Random.Range(knockbackstartrange, knockbackendrange);
            knockback = true;

        }



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
        go.GetComponent<DestroyText>().spawnPos(direction.x, direction.y, speed / 5);
    }


    public IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }


    void burst()
    {
        for (int i = 0; i < amountOfShots; i++)
        {
            float WeaponSpread = Random.Range(-bulletSpread, bulletSpread);
            Quaternion newRot = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + WeaponSpread);
            Instantiate(projectile, firePoint.position, newRot);
            burstTime = timeBtwBurst;
        }
    }

    void shoot()
    {
        for (int i = 0; i < amountOfShots; i++)
        {
            float WeaponSpread = Random.Range(-bulletSpread, bulletSpread);
            Quaternion newRot = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + WeaponSpread);
            Instantiate(projectile, firePoint.position, newRot);
            timeBtwShots = startTimeBtwShots;
        }

    }

    void randomPos()
    {
        if (reachedDestination == true)
        {
            NextMoveCoolDown = timeTillNextMove;
            randPos = transform.position;
            randPos += Random.insideUnitCircle * circleRadius;
            reachedDestination = false;
        }

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
