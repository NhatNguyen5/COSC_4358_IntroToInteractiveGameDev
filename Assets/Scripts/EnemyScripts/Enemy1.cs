using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy1 : MonoBehaviour
{
    public GameObject DamageText;
    private SpriteRenderer sprite;

    public event System.Action OnDeath;

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

    [Header("Line Of Sight")]
    public bool lineofsight;
    public LayerMask IgnoreMe;
    private bool unstuck;
    public float unstuckTime;
    private Transform UNSTUCKPOS;



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
    public float beginningrangetoshoot;
    public float endingrangetoshoot;
    private float startTimeBtwShots;

    public int AmountOfBullets;
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

    [Header("Drops")]
    public GameObject[] Drops;
    public float DropPercentageTylenol;
    public int NumOfTylenolDrop;
    public float DropPercentageProtein;
    public int NumOfProteinDrop;
    public float DropPercentageAmmo;
    public int NumOfAmmoDrop;
    public float DropPercentagePhizer;
    public int NumOfPhizerDrop;
    public float DropPercentageMolly;
    public int NumOfMollyDrop;

    //ANIMATION VARIABLES

    //DEATH VARIABLE
    private bool isDead = false;

    Vector2 direction;
    float a;


    private float critRate = 0;
    private float critDMG = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(GlobalPlayerVariables.GameOver==false)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        else
            player = this.transform;
        sprite = GetComponent<SpriteRenderer>();

        variation();
    }

    void variation()
    {
        startTimeBtwShots = Random.Range(beginningrangetoshoot, endingrangetoshoot);
        timeBtwShots = startTimeBtwShots;
    }


    private void FixedUpdate()
    {
        

        if (knockback == true)
        {

            //Debug.Log("KNOCKBACK");
            if (unstuck == false)
                transform.position = Vector2.MoveTowards(transform.position, player.position, -knockbackForce * speed * Time.deltaTime);
            else
            {
                //Debug.Log("COMMENCING UNSTUCK");
                transform.position = Vector2.MoveTowards(transform.position, randPos, -speed * Time.deltaTime);

            }
            getDirection(player);

        }
        else
        {

            if (Vector2.Distance(transform.position, player.position) >= stoppingDistance && followPlayer == true && lineofsight == true) //follow player
            {
                reachedDestination = true;
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                getDirection(player);
            }
            else if ((Vector2.Distance(transform.position, player.position) >= retreatDistance) || GlobalPlayerVariables.GameOver == true) //stop /*(Vector2.Distance(transform.position, player.position) <= stoppingDistance && */ 
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
                    direction = randPos;
                    a = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                }
            }
            else if (Vector2.Distance(transform.position, player.position) <= retreatDistance && retreat == true) //retreat
            {
                reachedDestination = true;
                transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
                getDirection(player);
            }

        }
        
    }

    void getDirection(Transform objectpos)
    {
        direction = objectpos.position - transform.position;
        a = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }


    // Update is called once per frame
    void Update()
    {
        if (GlobalPlayerVariables.GameOver != false)
        {
            player = this.transform;
        }
        knockbacktime -= Time.deltaTime;
        if (knockbacktime <= 0)
        {
            knockbacktime = 0;
            knockback = false;
            unstuck = false;
        }
        if (player != null && player!=this.transform)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, Mathf.Infinity, ~IgnoreMe);
            //var rayDirection = player.position - transform.position;
            //Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            if (hit.collider.gameObject.tag == "Player")
            {
                lineofsight = true;
                //Debug.Log("Player is Visable");
                // enemy can see the player!

                //Debug.Log("Player is Visable");
            }
            else
            {
                lineofsight = false;
                //Debug.Log("Player is NOT Visable");
            }

        }

        if (lineofsight == true && GlobalPlayerVariables.GameOver==false)
        {

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
                        variation();
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
        else
        {
            if (NextMoveCoolDown <= 0)
            {

                randomPos();
            }
            NextMoveCoolDown -= Time.deltaTime;
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        reachedDestination = true;
        NextMoveCoolDown = timeTillNextMove;

        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("PLAYER CONTACT");
            knockbackForce = knockForcePlayerContact;
            knockbacktime = Random.Range(knockbackstartrange, knockbackendrange);
            knockback = true;
        }
        else if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Player")
        {
            //Debug.Log("ENEMY IS HITTING WALL");
            //UNSTUCKPOS = collision.gameObject.GetComponent<Transform>();
            //knockbacktime = unstuckTime;
            //knockback = true;
            //unstuck = true;
            randomPos();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Bullet")
        {
            float damage = collision.gameObject.GetComponent<Bullet>().damage;
            float speed = collision.gameObject.GetComponent<Bullet>().speed;
            critRate = collision.gameObject.GetComponent<Bullet>().critRate;
            critDMG = collision.gameObject.GetComponent<Bullet>().critDMG;
            knockbackForce = collision.gameObject.GetComponent<Bullet>().knockbackForce;


            takeDamage(damage, collision.transform, speed);

            reachedDestination = true;

            knockbacktime = Random.Range(knockbackstartrange, knockbackendrange);
            knockback = true;

        }



    }

    public void takeDamage(float damage, Transform impact, float speed)
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
            Die();
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

                Color colorTop = new Color(0.83529f, 0.06667f, 0.06667f);
                Color colorBottom = new Color(0.98824f, 0.33725f, .90196f);


                go.GetComponent<TextMeshPro>().text = damage.ToString();
                go.GetComponent<TextMeshPro>().colorGradient = new VertexGradient(colorTop, colorTop, colorBottom, colorBottom);
                go.GetComponent<TextMeshPro>().fontSize *= 1.2f;
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


    void burst()
    {
        for (int i = 0; i < AmountOfBullets; i++)
        {
            float WeaponSpread = Random.Range(-bulletSpread, bulletSpread);
            Quaternion newRot = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + WeaponSpread);
            Instantiate(projectile, firePoint.position, newRot);
            burstTime = timeBtwBurst;
        }
    }

    void shoot()
    {
        for (int i = 0; i < AmountOfBullets; i++)
        {
            float WeaponSpread = Random.Range(-bulletSpread, bulletSpread);
            Quaternion newRot = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + WeaponSpread);
            Instantiate(projectile, firePoint.position, newRot);
            variation();
        }

    }

    void randomPos()
    {

        NextMoveCoolDown = Random.Range(0f, timeTillNextMove);
        randPos = transform.position;
        randPos += Random.insideUnitCircle * circleRadius;
        reachedDestination = false;


    }


    void Die()
    {
        if (isDead == false)
        {
            isDead = true;
            if (OnDeath != null)
            {
                OnDeath();
            }

            if (Random.Range(0, 100) <= DropPercentageTylenol)
            {
                for (int i = 0; i < NumOfTylenolDrop; i++)
                    Instantiate(Drops[0], transform.position, Quaternion.Euler(0, 0, 0));
            }

            if (Random.Range(0, 100) <= DropPercentageProtein)
            {
                for (int i = 0; i < NumOfProteinDrop; i++)
                    Instantiate(Drops[1], transform.position, Quaternion.Euler(0, 0, 0));
            }

            if (Random.Range(0, 100) <= DropPercentageAmmo)
            {
                for (int i = 0; i < NumOfAmmoDrop; i++)
                    Instantiate(Drops[2], transform.position, Quaternion.Euler(0, 0, 0));
            }

            if (Random.Range(0, 100) <= DropPercentagePhizer)
            {
                for (int i = 0; i < NumOfPhizerDrop; i++)
                    Instantiate(Drops[3], transform.position, Quaternion.Euler(0, 0, 0));
            }

            if (Random.Range(0, 100) <= DropPercentageMolly)
            {
                for (int i = 0; i < NumOfMollyDrop; i++)
                    Instantiate(Drops[4], transform.position, Quaternion.Euler(0, 0, 0));
            }

            GameObject.Destroy(gameObject);
        }
    }


}
