using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;
public class Globin : MonoBehaviour
{
    public Transform target;

    public Transform EnemyTarget;


    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;

    Seeker seeker;
    [HideInInspector]
    public Rigidbody2D rb;


    public GameObject DamageText;
    private SpriteRenderer sprite;
    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public Transform playerStash;

    //public event System.Action OnDeath;

    [Header("Globin Stats")]

    //public float contactDamage;
    public float HP = 100;
    public float AR = 0;
    public float speed = 200f;
    //public float speed = 0;
    //public int EXPWorth = 50;

    [Header("Movement Settings")]
    public float stoppingDistance = 0;
    //public float retreatDistance = 0;
    //public bool followPlayer;
    //public bool retreat;
    //public bool randomMovement;

    [Header("Line Of Sight")]
    public bool lineofsight;
    public LayerMask IgnoreMe;
    //private bool unstuck;
    //public float unstuckTime;
    public float shootdistance;
    public float distancefromplayer;
    //public float distancefromTarget;
    [HideInInspector]
    public bool canSeeEnemy = false;
    bool closest = false;


    //private float distToEnemy = 0;

    //private Transform UNSTUCKPOS;



    [Header("Random Movement Settings")]
    public float circleRadius;
    public float timeTillNextMove;
    private float NextMoveCoolDown;
    private bool reachedDestination;
    private Vector2 randPos;

    /*
    private float knockbackForce = 0;
    [Header("KnockBack Settings")]
    public float knockForcePlayerContact;
    private bool knockback = false;
    public float knockbackstartrange = 0.4f;
    public float knockbackendrange = 1.0f;
    private float knockbacktime;
    */

    private float timeBtwShots;
    [Header("Gun Settings")]
    public float beginningrangetoshoot;
    public float endingrangetoshoot;
    private float startTimeBtwShots;

    public int AmountOfBullets;
    public float bulletSpread = 0.0f;
    public GameObject projectile;
    public Transform firePoint;
    //private Rigidbody2D rb;


    [Header("Burst Settings")]
    public bool burstFire;
    public float timeBtwBurst;
    private float burstTime = 0;

    public int timesToShoot;
    private int TimesShot = 0;

    [Header("Follow Player After Certain Dist")]
    public bool StartToFollowAfterCertainDist = false;
    public float distToStartFollow = 0f;


    //ANIMATION VARIABLES

    //DEATH VARIABLE
    private bool isDead = false;



    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        if (GlobalPlayerVariables.GameOver == false)
        {
            playerStash = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Log("Player transform acquired");
            player = playerStash;
        }
        else
        {
            player = this.transform;
        }
        target = player;
        GlobalPlayerVariables.GlobinsAndPlayerAlive += 1;
        InvokeRepeating("UpdatePath", 0f, 0.5f);

    }

    private void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }



    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    void Astar()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
            return;
        }
        else
        {
            reachedEndofPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);


        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        //return;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GlobalPlayerVariables.EnableAI)
        {
            if (player != null)
                distancefromplayer = Vector2.Distance(rb.position, player.position);

            if (StartToFollowAfterCertainDist == true)
            {
                if (distancefromplayer <= distToStartFollow)
                {
                    StartToFollowAfterCertainDist = false;
                }
            }


            if (StartToFollowAfterCertainDist == false)
            {

                if (distancefromplayer >= stoppingDistance || lineofsight == false)
                {
                    Astar();
                }
                else //RANDOM MOVEMENT
                {
                    if (reachedDestination == false)
                    {
                        Vector2 direction = (randPos - rb.position).normalized;
                        Vector2 force = direction * speed * Time.deltaTime;
                        rb.AddForce(force);
                        //transform.position = Vector2.MoveTowards(transform.position, randPos, speed * Time.deltaTime); //need to make it where it walks in that direction
                    }
                    if (transform.position.x == randPos.x && transform.position.y == randPos.y)
                    {
                        reachedDestination = true;
                    }
                }
            }
        }
    }


    void randomPos()
    {

        NextMoveCoolDown = Random.Range(0f, timeTillNextMove);
        randPos = transform.position;
        randPos += Random.insideUnitCircle * circleRadius;
        reachedDestination = false;
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

    void variation()
    {
        startTimeBtwShots = Random.Range(beginningrangetoshoot, endingrangetoshoot);
        timeBtwShots = startTimeBtwShots;
    }

    void shoot()
    {
        for (int i = 0; i < AmountOfBullets; i++)
        {
            //EnemyWeapon.WeaponAnim.SetBool("IsShooting", true);
            float WeaponSpread = Random.Range(-bulletSpread, bulletSpread);
            Quaternion newRot = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + WeaponSpread);
            Instantiate(projectile, firePoint.position, newRot);
            variation();
        }

    }






    Vector3 directionToTarget = Vector3.zero;
    private void Update()
    {
        if (GlobalPlayerVariables.EnableAI)
        {

            if (GlobalPlayerVariables.Defend == false)
            {
                target = GlobalPlayerVariables.newTransformCoords;
                player = GlobalPlayerVariables.newTransformCoords;
                
            }
            if (GlobalPlayerVariables.Defend == true)
            {
                target = playerStash;
                player = playerStash;
            }



            if (player != null && player != this.transform)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, Mathf.Infinity, ~IgnoreMe);
                Debug.DrawRay(transform.position, player.position - transform.position, Color.green);
                //var rayDirection = player.position - transform.position;
                //Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);

                if (GlobalPlayerVariables.Defend == true)
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
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
                else if (GlobalPlayerVariables.Defend == false)
                {
                    if (hit.collider.gameObject.CompareTag("DefendPos"))
                    {
                        //Debug.Log("Player is Visable");
                        lineofsight = true;
                    }
                    else
                    {
                        //Debug.Log("Player is Not Visable");
                        lineofsight = false;
                    }
                }




                if (NextMoveCoolDown <= 0)
                {
                    //Vector2 temp = randPos;
                    randomPos();
                    //facing = Mathf.Atan2((temp - randPos).x, (temp - randPos).y) * Mathf.Rad2Deg;
                }
                NextMoveCoolDown -= Time.deltaTime;




                //working on clearing up globin vision
                float closestDistanceSqr = Mathf.Infinity;
                Collider2D[] ColliderArray = Physics2D.OverlapCircleAll(transform.position, shootdistance);
                foreach (Collider2D collider2D in ColliderArray)
                {
                    if (collider2D.TryGetComponent<EnemyMarker>(out EnemyMarker marked))
                    {
                        if (collider2D.TryGetComponent<Transform>(out Transform enemy))
                        {
                            //CAN THEY SEE THEM

                            //can probably optimize this later
                            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, enemy.transform.position - transform.position, Mathf.Infinity, ~IgnoreMe);


                            if (hit2.collider.gameObject.CompareTag("EnemyMelee") || hit2.collider.gameObject.CompareTag("Enemy") || hit2.collider.gameObject.CompareTag("Colony"))
                            {
                                canSeeEnemy = true;
                                directionToTarget = enemy.position - transform.position;
                                float dSqrToTarget = directionToTarget.sqrMagnitude;
                                if (dSqrToTarget < closestDistanceSqr)
                                {
                                    closestDistanceSqr = dSqrToTarget;
                                    EnemyTarget = enemy;
                                    closest = true;
                                    //Debug.Log("Found target");

                                    //if (EnemyTarget != null && canSeeEnemy == true && closest == true && EnemyTarget == enemy)
                                }
                            }


                            //target = enemy;
                        }
                    }
                }
                /*
                if (canSeeEnemy == false)
                    Debug.DrawRay(transform.position, enemy.transform.position - transform.position, Color.white);
                */



                if (EnemyTarget != null)
                {
                    RaycastHit2D hit3 = Physics2D.Raycast(transform.position, EnemyTarget.transform.position - transform.position, Mathf.Infinity, ~IgnoreMe);
                    if (hit3.collider.gameObject.CompareTag("EnemyMelee") || hit3.collider.gameObject.CompareTag("Enemy") || hit3.collider.gameObject.CompareTag("Colony"))
                    {
                        canSeeEnemy = true;
                        //Debug.DrawRay(transform.position, EnemyTarget.transform.position - transform.position, Color.red);
                    }
                    else
                    {
                        canSeeEnemy = false;
                    }
                }
                else
                {
                    canSeeEnemy = false;
                }



                /*
                if (EnemyTarget != null && canSeeEnemy == true && closest == false)
                    Debug.DrawRay(transform.position, enemy.transform.position - transform.position, Color.black);
                */
                if (canSeeEnemy == true && GlobalPlayerVariables.GameOver == false)
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

                    if (NextMoveCoolDown <= 0 || reachedDestination == true)
                    {
                        //Vector2 temp = randPos;
                        randomPos();
                        //facing = Mathf.Atan2((temp - randPos).x, (temp - randPos).y) * Mathf.Rad2Deg;
                    }
                    NextMoveCoolDown -= Time.deltaTime;
                }
            }
        }
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            float contactDamage = 0;
            float speed = 0;
            if (collision.gameObject.GetComponent<Enemy1>() != null)
            {
                contactDamage = collision.gameObject.GetComponent<Enemy1>().contactDamage;
                speed = collision.gameObject.GetComponent<Enemy1>().speed;
            }
            else
            {
                contactDamage = collision.gameObject.GetComponent<Enemy3>().contactDamage;
                speed = collision.gameObject.GetComponent<Enemy3>().speed;
            }


            //float contactDamage = collision.gameObject.GetComponent<Enemy1>().contactDamage;
            //float speed = collision.gameObject.GetComponent<Enemy1>().speed;
            takeDamage(contactDamage, collision.transform, speed);
        }
        if (collision.gameObject.CompareTag("EnemyMelee"))
        {
            float contactDamage = collision.gameObject.GetComponent<Enemy2>().contactDamage;
            float speed = collision.gameObject.GetComponent<Enemy2>().speed;
            takeDamage(contactDamage, collision.transform, speed);
        }
        if (collision.gameObject.CompareTag("Colony"))
        {
            float contactDamage = collision.gameObject.GetComponent<EnemyColony>().contactDamage;
            float speed = collision.gameObject.GetComponent<EnemyColony>().speed;
            takeDamage(contactDamage, collision.transform, speed);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("EnemyBullet"))
        {
            if (collision.gameObject.GetComponent<EnemyProj>().isDeflected == false)
            {
                float damage = collision.gameObject.GetComponent<EnemyProj>().damage;
                float speed = collision.gameObject.GetComponent<EnemyProj>().speed;

                //collision.gameObject.GetComponent<EnemyProj>().hit = true;
                takeDamage(damage, collision.transform, speed);
            }
        }
        if (collision.CompareTag("EnemyBullet2"))
        {
            if (collision.gameObject.GetComponent<EnemyProj2>().isDeflected == false)
            {
                float damage = collision.gameObject.GetComponent<EnemyProj2>().damage;
                float speed = collision.gameObject.GetComponent<EnemyProj2>().speed;

                //collision.gameObject.GetComponent<EnemyProj>().hit = true;
                takeDamage(damage, collision.transform, speed);
            }
        }

    }

    public void takeDamage(float damage, Transform impact, float speed)
    {
        //Debug.Log(damage);
        //bool iscrit = false;
        //float chance2crit = Random.Range(0f, 1f);
        /*
        if (chance2crit <= critRate)
        {
            iscrit = true;
            damage *= critDMG;
        }*/

        //Debug.Log("Globin Taking Damage");

        if (damage > AR)
        {
            HP = (HP - (damage - AR));
            showDamage(damage, impact, speed);
            StartCoroutine(FlashRed());
        }
        else {
            HP = HP - 1;
            showDamage(damage, impact, speed);
            StartCoroutine(FlashRed());
        }
        
        
        if (HP <= 0)
        {
            Die();
        }
            
    }


    void showDamage(float damage, Transform impact, float speed)
    {
        //damage = Mathf.Round(damage - AR);


        if (damage > AR)
        {
            damage = Mathf.Round(damage - AR);
        }
        else
        {
            damage = Mathf.Round(1);
        }


        if (damage >= 1)
        {
            Vector3 direction = (transform.position - impact.transform.position).normalized;

            //might add to impact to make it go past enemy
            var go = Instantiate(DamageText, impact.position, Quaternion.identity);

            //Debug.Log("CRIT");

            //Color colorTop = new Color(0.83529f, 0.06667f, 0.06667f);
            //Color colorBottom = new Color(0.98824f, 0.33725f, .90196f);


            go.GetComponent<TextMeshPro>().text = damage.ToString();
            //go.GetComponent<TextMeshPro>().colorGradient = new VertexGradient(colorTop, colorTop, colorBottom, colorBottom);
            go.GetComponent<TextMeshPro>().fontSize *= 0.8f;
            
            go.GetComponent<DestroyText>().spawnPos(direction.x, direction.y, speed / 5);
        }
    }


    public IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }






    void Die()
    {
        if (isDead == false)
        {
            isDead = true;
            //lobalPlayerVariables.expToDistribute += EXPWorth;
           

            if (transform.Find("StickyGrenade(Clone)") != null)
            {
                transform.Find("StickyGrenade(Clone)").GetComponent<StickyGrenade>().stuck = false;
                transform.Find("StickyGrenade(Clone)").parent = null;
            }
            GlobalPlayerVariables.GlobinsAndPlayerAlive -= 1;
            GameObject.Destroy(gameObject);
        }
    }











}
