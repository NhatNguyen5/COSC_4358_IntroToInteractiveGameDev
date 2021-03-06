using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D.Animation;
using System.Linq;

public class Enemy3 : MonoBehaviour
{
    public delegate void EnemyKilled();
    public static event EnemyKilled OnEnemyKilled;

    [System.Serializable]
    public struct ItemDrops
    {
        public bool isCurrency;
        public bool isAmmo;
        public GameObject[] Drops;
        public float DropPercentage;
        public int NumOfDrop;
        public int[] convertProtein()
        {
            int numOfDigits;
            if (NumOfDrop.ToString().Length >= Drops.Length)
                numOfDigits = Drops.Length;
            else
                numOfDigits = NumOfDrop.ToString().Length;

            int[] convertedArr = new int[numOfDigits];

            float multten = 1;
            for (int i = 0; i < numOfDigits; i++)
            {
                if (i < numOfDigits - 1)
                    convertedArr[i] = (Mathf.FloorToInt(NumOfDrop / multten) % 10);
                else
                    convertedArr[i] = (Mathf.FloorToInt(NumOfDrop / multten));
                multten *= 10;
            }
            return convertedArr;
        }
        public int[] convertAmmo()
        {
            int[] convertedArr = new int[3];
            convertedArr[2] = Mathf.FloorToInt(NumOfDrop / 100f);

            if ((NumOfDrop / 100f) - convertedArr[2] > 0.5)
            {
                convertedArr[1] = 1;
                convertedArr[0] = 1;
            }
            else if ((NumOfDrop / 100f) - convertedArr[2] == 0.5)
                convertedArr[1] = 1;
            else if ((NumOfDrop / 100f) - convertedArr[2] > 0 && (NumOfDrop / 100f) - convertedArr[2] < 0.5)
                convertedArr[0] = 1;

            return convertedArr;
        }
    }

    [Header("Drops")]
    public ItemDrops[] Drops;

    public Transform target;

    public Transform EnemyTarget;

    public Transform BossObject;


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

    [Header("Movement Settings")]
    public float stoppingDistance = 0;


    [Header("Random Movement Settings")]
    public float circleRadius;
    public float timeTillNextMove;
    private float NextMoveCoolDown;
    private bool reachedDestination;
    private Vector2 randPos;



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


    [Header("Enemy3 Stats")]
    public Image HPbar;
    public float HP = 100;
    public float maxHP = 100;
    public float AR = 0;
    public float speed = 200f;
    public float contactDamage = 12;
    public int EXPWorth = 50;

    [Header("Chase Settings")]
    public float time2chase;
    private float chaseInProgress;

    [Header("Special Movement/Abilities")]
    public bool retreatToBoss = false;
    public float retreatDist;

    public bool canDash = false;
    public bool randomDash = false;
    public float dashForce;
    public float dashBackOnHit;
    public float beginningRangeToDash;
    public float endingRangeToDash;
    private float DashTimer;

    public bool canDoAirStrike = false;
    public GameObject AirStrike;
    public int amountOfAirStrikes;
    public float beginningRangeToCallAirStrike;
    public float endingRangeToCallAirStrike;
    private float AirStrikeTimer;




    private float knockbackForce = 0;
    [Header("KnockBack Settings")]
    public float knockForcePlayerContact;
    private bool knockback = false;
    public float knockbackstartrange = 0.4f;
    public float knockbackendrange = 1.0f;
    private float knockbacktime;



    private float timeBtwShots;
    [Header("Gun Settings")]
    public bool canUseGun = false;
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

    //DEATH VARIABLE
    private bool isDead = false;


    private float critRate = 0;
    private float critDMG = 0;

    //ANIMATION VARIABLES
    [Header("SkinModule")]
    [SerializeField]
    private SpriteLibrary spriteLibrary = default;
    [SerializeField]
    private SpriteResolver targetResolver = default;
    [SerializeField]
    private string targetCategory = default;

    private string[] currSprite;

    private float relaMouseAngle;

    [HideInInspector]
    public float facing;

    private EnemyManager enemyManager = null;


    private void Awake()
    {
        GlobalPlayerVariables.TotalEnemiesAlive += 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        //maxHP = HP;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        if (GlobalPlayerVariables.GameOver == false)
        {
            playerStash = Player.instance.transform;
            //playerStash = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Log("Player transform acquired");
            player = playerStash;
        }
        else
        {
            player = this.transform;
        }

        if (retreatToBoss == true)
        {
            BossObject = GameObject.FindGameObjectWithTag("Colony").transform;
        }

        target = player;
        playerStash = player;
        EnemyTarget = player;
        //GlobalPlayerVariables.GlobinsAndPlayerAlive += 1;
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        currSprite = spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(targetCategory).ToArray();

        //if (GameObject.Find("EnemyColony") != null)
        enemyManager = EnemyManager.instance;
        //if (GameObject.Find("EnemyColony2") != null)
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


    void FixedUpdate()
    {
        if (GlobalPlayerVariables.EnableAI)
        {
            if (player != null)
                distancefromplayer = Vector2.Distance(rb.position, player.position);

            if (retreatToBoss == true && distancefromplayer <= retreatDist)
            {
                //need logic to make it where target system doesn't conflict
                if(BossObject != null)
                    target = BossObject;
                else if (BossObject == null)
                {
                    target = playerStash;
                }
                Astar();
            }
            else if (distancefromplayer >= stoppingDistance && lineofsight == false && chaseInProgress > 0)
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

            facing = transform.Find("EnemyAim").GetComponent<Enemy3AIM>().AimDir;
            Animate(facing);
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
            if (projectile.name == "EnemyBullet")
            {

                GameObject bullet = ObjectPool.instance.GetBulletFromPool();
                if (bullet != null)
                {
                    bullet.transform.position = firePoint.position;
                    //bullet.transform.rotation = firePoint.rotation;
                    bullet.transform.rotation = newRot;
                    bullet.GetComponent<EnemyProj>().despawnTime = bullet.GetComponent<EnemyProj>().DespawnTimeHolder;
                    bullet.GetComponent<PlaySound>().replaySound();
                    //bullet.GetComponent<EnemyProj>().resetSpeed();
                }

            }
            else
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
            if (projectile.name == "EnemyBullet")
            {

                GameObject bullet = ObjectPool.instance.GetBulletFromPool();
                if (bullet != null)
                {
                    bullet.transform.position = firePoint.position;
                    //bullet.transform.rotation = firePoint.rotation;
                    bullet.transform.rotation = newRot;
                    bullet.GetComponent<EnemyProj>().despawnTime = bullet.GetComponent<EnemyProj>().DespawnTimeHolder;
                    bullet.GetComponent<PlaySound>().replaySound();
                    //bullet.GetComponent<EnemyProj>().resetSpeed();
                }

            }
            else
                Instantiate(projectile, firePoint.position, newRot);
            variation();
        }

    }

    Vector3 directionToTarget = Vector3.zero;

    private void Update()
    {
        if (GlobalPlayerVariables.EnableAI)
        {

            if (knockback == true)
            {
                knockbacktime -= Time.deltaTime;
                if (knockbacktime <= 0)
                {
                    knockback = false;
                }
            }



            if (playerStash == null)
            {
                player = this.transform;
            }


            if (target == null || EnemyTarget == null)
            {
                target = playerStash;
                EnemyTarget = playerStash;
                chaseInProgress = 0f;
            }


            if (DashTimer <= 0 && canDash == true)
            { 
                float newDashTimer = Random.Range(beginningRangeToDash, endingRangeToDash);
                Vector2 direction = Vector2.zero;
                if (lineofsight == true)
                {
                    if (target != null && randomDash == false || target != null && distancefromplayer >= shootdistance / 2)
                        direction = (target.position - transform.position).normalized;
                    else if (target != null && randomDash == true)
                    {
                        randomPos();
                        Vector3 newpos = new Vector3(randPos.x, randPos.y, 0);
                        direction = (newpos - transform.position).normalized;
                    }
                }
                else
                {
                    Vector3 newpos = new Vector3(randPos.x, randPos.y, 0);
                    direction = (newpos - transform.position).normalized;
                }
                Vector2 force = direction * dashForce;
                rb.AddForce(force, ForceMode2D.Impulse);
                

                DashTimer = newDashTimer;
                
            }
            if(DashTimer >= 0)
                DashTimer -= Time.deltaTime;


            //AIRSTRIKES
            if (canDoAirStrike == true && AirStrike != null && AirStrikeTimer <= 0 && lineofsight == true)
            {
                float newAirStrikeTimer = Random.Range(beginningRangeToCallAirStrike, endingRangeToCallAirStrike);
                AirStrikeTimer = newAirStrikeTimer;

                for (int i = 0; i < amountOfAirStrikes; i++)
                {
                    Vector2 AOE = Vector2.zero;
                    if(EnemyTarget != null)
                        AOE = EnemyTarget.position;
                    //randPos = transform.position;
                    AOE += Random.insideUnitCircle * circleRadius;
                    Instantiate(AirStrike, AOE, Quaternion.Euler(0, 0, 0));
                }

            }
            if (AirStrikeTimer >= 0)
                AirStrikeTimer -= Time.deltaTime;



            /*
            if (GlobalPlayerVariables.Defend == false)
            {
                target = GlobalPlayerVariables.newTransformCoords;
                player = GlobalPlayerVariables.newTransformCoords;

            }
            */
            /*
            if (GlobalPlayerVariables.Defend == true)
            {
                target = playerStash;
                player = playerStash;
            }
            */



            if (player != null && player != this.transform)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, Mathf.Infinity, ~IgnoreMe);
                //Debug.DrawRay(transform.position, player.position - transform.position, Color.green);
                //var rayDirection = player.position - transform.position;
                //Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);

                /*
                if (GlobalPlayerVariables.Defend == true)
                {
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
                else if (GlobalPlayerVariables.Defend == false)
                {
                    if (hit.collider.gameObject.tag == "DefendPos")
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

                */


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
                    if (collider2D.TryGetComponent<GoodGuyMarker>(out GoodGuyMarker marked))
                    {
                        if (collider2D.TryGetComponent<Transform>(out Transform enemy))
                        {
                            //CAN THEY SEE THEM

                            //can probably optimize this later
                            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, enemy.transform.position - transform.position, Mathf.Infinity, ~IgnoreMe);


                            if (hit2.collider.gameObject.CompareTag("Player") || hit2.collider.gameObject.CompareTag("Globin"))
                            {
                                canSeeEnemy = true;
                                directionToTarget = enemy.position - transform.position;
                                float dSqrToTarget = directionToTarget.sqrMagnitude;
                                if (dSqrToTarget < closestDistanceSqr)
                                {
                                    closestDistanceSqr = dSqrToTarget;
                                    EnemyTarget = enemy;
                                    if(retreatToBoss == false)
                                        target = enemy;
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
                    if (hit3.collider.gameObject.CompareTag("Globin") || hit3.collider.gameObject.CompareTag("Player"))
                    {
                        lineofsight = true;
                        canSeeEnemy = true;
                        chaseInProgress = time2chase;

                        Debug.DrawRay(transform.position, EnemyTarget.transform.position - transform.position, Color.red);
                    }
                    else
                    {
                        lineofsight = false;
                        canSeeEnemy = false;
                    }
                }
                else
                {
                    lineofsight = false;
                    canSeeEnemy = false;
                }



                /*
                if (EnemyTarget != null && canSeeEnemy == true && closest == false)
                    Debug.DrawRay(transform.position, enemy.transform.position - transform.position, Color.black);
                */
                
                if (canSeeEnemy == true && GlobalPlayerVariables.GameOver == false && canUseGun == true)
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

                    
                }
                
                if (NextMoveCoolDown <= 0 || reachedDestination == true)
                {
                    //Vector2 temp = randPos;
                    randomPos();
                    //facing = Mathf.Atan2((temp - randPos).x, (temp - randPos).y) * Mathf.Rad2Deg;
                }
                chaseInProgress -= Time.deltaTime;
                NextMoveCoolDown -= Time.deltaTime;


            }
        }
    }



    
    private void OnCollisionEnter2D(Collision2D collision)
    {

        Vector2 direction = (collision.transform.position - transform.position).normalized;
    
        Vector2 force = direction * dashBackOnHit;
        rb.AddForce(-force, ForceMode2D.Impulse);


        //reachedDestination = true;
        //NextMoveCoolDown = timeTillNextMove;

        /*

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Globin")
        {
            randomPos();
            /*
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
        */
    }

    










    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {


            Bullet collision = col.gameObject.GetComponent<Bullet>();

            float damage = collision.damage;
            float speed = collision.speed;
            critRate = collision.critRate;
            critDMG = collision.critDMG;
            knockbackForce = collision.knockbackForce;

            /*
            float damage = collision.gameObject.GetComponent<Bullet>().damage;
            float speed = collision.gameObject.GetComponent<Bullet>().speed;
            critRate = collision.gameObject.GetComponent<Bullet>().critRate;
            critDMG = collision.gameObject.GetComponent<Bullet>().critDMG;
            knockbackForce = collision.gameObject.GetComponent<Bullet>().knockbackForce;
            */

            takeDamage(damage, collision.transform, speed);

            reachedDestination = true;


            Vector2 direction = (collision.transform.position - transform.position).normalized;
            Vector2 force = direction * knockbackForce;
            rb.AddForce(force, ForceMode2D.Impulse);

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

        //Debug.Log("Globin Taking Damage");

        if (damage > AR)
        {
            HP = (HP - (damage - AR));
            showDamage(damage, impact, speed, iscrit);
            StartCoroutine(FlashRed());
        }
        else
        {
            HP = HP - 1;
            showDamage(damage, impact, speed, iscrit);
            StartCoroutine(FlashRed());
        }

        HPbar.fillAmount = HP / maxHP;
        if (HP <= 0)
        {
            Die();
        }

    }


    void showDamage(float damage, Transform impact, float speed, bool crit)
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
            //var go = Instantiate(DamageText, impact.position, Quaternion.identity);
            GameObject go = ObjectPool.instance.GetDamagePopUpFromPool();
            go.GetComponent<Animator>().Play("DamagePopUp", -1, 0f);
            go.transform.SetParent(null);
            go.transform.position = impact.position;
            go.transform.rotation = Quaternion.identity;

            if (crit == false)
            {
                go.GetComponent<TextMeshPro>().text = damage.ToString();
                go.GetComponent<TextMeshPro>().fontSize = 9f;
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


            //Debug.Log("CRIT");

            //Color colorTop = new Color(0.83529f, 0.06667f, 0.06667f);
            //Color colorBottom = new Color(0.98824f, 0.33725f, .90196f);


            //go.GetComponent<TextMeshPro>().text = damage.ToString();
            //go.GetComponent<TextMeshPro>().colorGradient = new VertexGradient(colorTop, colorTop, colorBottom, colorBottom);
            //go.GetComponent<TextMeshPro>().fontSize *= 0.8f;

            go.GetComponent<DestroyText>().spawnPos(direction.x, direction.y, speed / 5);
        }
    }

    WaitForSeconds shortWait = new WaitForSeconds(0.1f);
    public IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return shortWait;
        sprite.color = Color.white;
    }






    void Die()
    {
        if (isDead == false)
        {
            isDead = true;
            if (enemyManager != null)
            {
                enemyManager.OnEnemyDeath(3);
            }
            GlobalPlayerVariables.expToDistribute += EXPWorth;
            RememberLoadout.totalExperienceEarned += EXPWorth;
            GlobalPlayerVariables.TotalEnemiesAlive -= 1;
            GlobalPlayerVariables.enemiesKilled += 1;
            if (OnEnemyKilled != null)
            {
                OnEnemyKilled();
            }
            foreach (ItemDrops id in Drops)
            {
                if (Random.Range(0, 100) <= id.DropPercentage)
                {
                    if (id.isAmmo == false)
                    {
                        if (id.isCurrency == false)
                        {
                            for (int i = 0; i < id.NumOfDrop; i++)
                                Instantiate(id.Drops[0], transform.position, Quaternion.identity);
                        }
                        else if (id.isCurrency == true)
                        {
                            int[] NumArr = id.convertProtein();
                            for (int i = 0; i < NumArr.Length; i++)
                            {
                                for (int j = 0; j < NumArr[i]; j++)
                                    Instantiate(id.Drops[i], transform.position, Quaternion.identity);
                            }
                        }
                    }
                    else
                    {
                        int[] NumArr = id.convertAmmo();
                        for (int i = 0; i < NumArr.Length; i++)
                        {
                            for (int j = 0; j < NumArr[i]; j++)
                                Instantiate(id.Drops[i], transform.position, Quaternion.identity);
                        }
                    }
                }
            }
            if (transform.Find("StickyGrenade(Clone)") != null)
            {
                transform.Find("StickyGrenade(Clone)").GetComponent<StickyGrenade>().stuck = false;
                transform.Find("StickyGrenade(Clone)").GetComponent<StickyGrenade>().landed = true;
                transform.Find("StickyGrenade(Clone)").GetComponent<CircleCollider2D>().isTrigger = false;
                transform.Find("StickyGrenade(Clone)").parent = null;
            }

            GameObject.Destroy(gameObject);
        }
    }

    public void Animate(float angle)
    {
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(player.Stats.Direction.y, player.Stats.Direction.x) * Mathf.Rad2Deg - 180));
        //relaMouseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        relaMouseAngle = angle;
        if (relaMouseAngle < 0)
            relaMouseAngle = relaMouseAngle + 360;
        //Debug.Log(relaMouseAngle);
        //Debug.Log(relaMouseAngle);
        //New 8 directions system
        /*[0]Down
         *[1]Up
         *[2]Left
         *[3]Right
         *[4]TopLeft
         *[5]TopRight
         *[6]BotLeft
         *[7]BotRight
         */
        if (relaMouseAngle <= 22.5 || relaMouseAngle > 337.5) //Right
        {
            targetResolver.SetCategoryAndLabel(targetCategory, currSprite[3]);
        }
        else if (relaMouseAngle > 22.5 && relaMouseAngle <= 67.5) //TopRight
        {
            targetResolver.SetCategoryAndLabel(targetCategory, currSprite[5]);
        }
        else if (relaMouseAngle > 67.5 && relaMouseAngle <= 112.5) //Up
        {
            targetResolver.SetCategoryAndLabel(targetCategory, currSprite[1]);
        }
        else if (relaMouseAngle > 112.5 && relaMouseAngle <= 157.5) //TopLeft
        {
            targetResolver.SetCategoryAndLabel(targetCategory, currSprite[4]);
        }
        else if (relaMouseAngle > 157.5 && relaMouseAngle <= 202.5) //Left
        {
            targetResolver.SetCategoryAndLabel(targetCategory, currSprite[2]);
        }
        else if (relaMouseAngle > 202.5 && relaMouseAngle <= 247.5) //BotLeft
        {
            targetResolver.SetCategoryAndLabel(targetCategory, currSprite[6]);
        }
        else if (relaMouseAngle > 247.5 && relaMouseAngle <= 292.5) //Down
        {
            targetResolver.SetCategoryAndLabel(targetCategory, currSprite[0]);
        }
        else if (relaMouseAngle > 292.5 && relaMouseAngle <= 337.5) //BotRight
        {
            targetResolver.SetCategoryAndLabel(targetCategory, currSprite[7]);
        }
    }
}
