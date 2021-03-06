using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyColony : MonoBehaviour
{
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

    public GameObject DamageText;
    private SpriteRenderer sprite;
    public Text BossName;
    public Image HealthBar;
    public string NameOfEnemy;
    public GameObject EnemyUI;
    public float DetectRange;
    //DEATH VARIABLE
    [HideInInspector]
    public bool isDead = false;

    [Header("Enemy Stats")]

    public float contactDamage;
    private EnemyManager enemyColony;
    private float MaxHP = 0;
    public float speed = 0;
    public int EXPWorth = 50;

    [Header("Movement Settings")]
    public float stoppingDistance = 0;
    public float retreatDistance = 0;
    public bool followPlayer;
    public bool retreat;
    public bool randomMovement;

    [Header("Line Of Sight")]
    public bool lineofsight;
    public LayerMask IgnoreMe;
    public float shootdistance;
    private float distancefromplayer;

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
    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public Transform playerStash;
    private Rigidbody2D rb;

    [Header("Burst Settings")]
    public bool burstFire;
    public float timeBtwBurst;
    private float burstTime = 0;

    public int timesToShoot;
    private int TimesShot = 0;



    [Header("Drops")]
    public ItemDrops[] Drops;


    Vector2 direction;
    float a;


    private float critRate = 0;
    private float critDMG = 0;

    private bool hideing = false;


    [Header("Reset Check Setting")]
    public float recalcshortestDist = 3f;
    private float timer2reset;



    // Start is called before the first frame update
    void Start()
    {
        BossName.text = NameOfEnemy;
        enemyColony = transform.parent.GetComponent<EnemyManager>();
        //Debug.Log(enemyColony.colonyHealth);
        MaxHP = enemyColony.colonyHealth;
        //HealthBar = GameObject.Find("EnemyHP").GetComponent<Image>();
        //BossName = GameObject.Find("BossName").GetComponent<Text>();
        sprite = transform.Find("BossSprite").GetComponent<SpriteRenderer>();
        HealthBar.fillAmount = enemyColony.colonyHealth / MaxHP;

        rb = GetComponent<Rigidbody2D>();
        if (GlobalPlayerVariables.GameOver == false)
        {
            //Debug.Log("SETTING PLAYER");
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
            player = this.transform;


        playerStash = player;
        GlobalPlayerVariables.TotalEnemiesAlive += 1;
        variation();

    }

    void variation()
    {
        startTimeBtwShots = Random.Range(beginningrangetoshoot, endingrangetoshoot);
        timeBtwShots = startTimeBtwShots;
    }


    private void FixedUpdate()
    {
        if (GlobalPlayerVariables.EnableAI)
        {
            if(player!=null)
                distancefromplayer = Vector2.Distance(transform.position, player.position);
            if (knockback == true)
            {
                transform.position = Vector2.MoveTowards(transform.position, randPos, -speed * Time.deltaTime);
                getDirection(player);

            }
            else
            {

                if (distancefromplayer >= stoppingDistance && followPlayer == true && lineofsight == true) //follow player
                {
                    reachedDestination = true;
                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                    getDirection(player);
                }
                else if ((distancefromplayer >= retreatDistance) || GlobalPlayerVariables.GameOver == true) //stop /*(Vector2.Distance(transform.position, player.position) <= stoppingDistance && */ 
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
                else if (distancefromplayer <= retreatDistance && retreat == true) //retreat
                {
                    reachedDestination = true;
                    transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
                    getDirection(player);
                }

            }
        }
    }

    void getDirection(Transform objectpos)
    {
        if(player!=null)
            direction = objectpos.position - transform.position;
        a = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }



    // Update is called once per frame
    void Update()
    {
        if (GlobalPlayerVariables.EnableAI)
        {
            if (player == null)
                player = playerStash;


            //working on clearing up globin vision
            if (timer2reset <= 0)
            {
                timer2reset = recalcshortestDist;
                float closestDistanceSqr = Mathf.Infinity;
                Collider2D[] ColliderArray = Physics2D.OverlapCircleAll(transform.position, shootdistance);
                foreach (Collider2D collider2D in ColliderArray)
                {
                    if (collider2D.TryGetComponent<GoodGuyMarker>(out GoodGuyMarker marked))
                    {
                        if (collider2D.TryGetComponent<Transform>(out Transform enemy))
                        {
                            //Debug.Log("good guy detected");
                            //CAN THEY SEE THEM

                            //can probably optimize this later
                            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, enemy.transform.position - transform.position, Mathf.Infinity, ~IgnoreMe);


                            if (hit2.collider.gameObject.CompareTag("Player") || hit2.collider.gameObject.CompareTag("Globin"))
                            {
                                lineofsight = true;
                                Vector3 directionToTarget = enemy.position - transform.position;
                                float dSqrToTarget = directionToTarget.sqrMagnitude;
                                if (dSqrToTarget < closestDistanceSqr)
                                {
                                    closestDistanceSqr = dSqrToTarget;
                                    player = enemy;
                                    //closest = true;
                                    //Debug.Log("Found target");

                                    //if (EnemyTarget != null && canSeeEnemy == true && closest == true && EnemyTarget == enemy)
                                }
                            }


                            //target = enemy;
                        }
                    }
                }
            }




            if (GlobalPlayerVariables.GameOver != false)
            {
                if (hideing == false)
                {
                    player = this.transform;
                    hideing = true;
                }
            }

            if (isDead == true)
            {
                knockbacktime = 0;
                //knockback = false;
            }



            if (distancefromplayer <= DetectRange && GlobalPlayerVariables.GameOver == false)
            {
                if (hideing == false)
                    EnemyUI.SetActive(true);
            }
            else if (distancefromplayer >= DetectRange)
            {
                EnemyUI.SetActive(false);
            }


            knockbacktime -= Time.deltaTime;
            if (knockbacktime <= 0)
            {
                knockbacktime = 0;
                knockback = false;
            }
            if (player != null && player != this.transform)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, Mathf.Infinity, ~IgnoreMe);
                //var rayDirection = player.position - transform.position;
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
                if (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Globin"))
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

            if (lineofsight == true && GlobalPlayerVariables.GameOver == false && isDead == false && distancefromplayer <= shootdistance)
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
                        //Debug.Log("SHOOT");
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

        timer2reset -= Time.deltaTime;

    }

    





    private void OnCollisionEnter2D(Collision2D collision)
    {
        reachedDestination = true;
        NextMoveCoolDown = timeTillNextMove;

        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("PLAYER CONTACT");
            knockbackForce = knockForcePlayerContact;
            knockbacktime = Random.Range(knockbackstartrange, knockbackendrange);
            knockback = true;
        }
        else if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("ENEMY IS HITTING WALL");
            //UNSTUCKPOS = collision.gameObject.GetComponent<Transform>();
            //knockbacktime = unstuckTime;
            //knockback = true;
            //unstuck = true;
            randomPos();
        }
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

        enemyColony.colonyHealth -= damage;
        HealthBar.fillAmount = enemyColony.colonyHealth / MaxHP;
        showDamage(damage, impact, speed, iscrit);
        StartCoroutine(FlashRed());
        if (enemyColony.colonyHealth <= 0)
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

    WaitForSeconds shortWait = new WaitForSeconds(0.1f);
    public IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return shortWait;
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
        if (isDead == false)
        {
            NextMoveCoolDown = Random.Range(0f, timeTillNextMove);

            randPos = transform.position;

            randPos += Random.insideUnitCircle * circleRadius;
            //Vector3 zfix = new Vector3(randPos.x, randPos.y, 0);
            //randPos = zfix;
            reachedDestination = false;
        }


    }



    void Die()
    {
        if (isDead == false)
        {
            isDead = true;
            reachedDestination = true;
            NextMoveCoolDown = 15f;
            GlobalPlayerVariables.expToDistribute += EXPWorth;
            RememberLoadout.totalExperienceEarned += EXPWorth;
            GlobalPlayerVariables.TotalEnemiesAlive -= 1;
            GlobalPlayerVariables.enemiesKilled += 1;
            transform.position = this.transform.position;
            transform.Find("BossSprite").GetComponent<Animator>().SetBool("IsDead", isDead);
            GetComponent<PolygonCollider2D>().enabled = false;
            StartCoroutine(Dying());
            //GameObject.Destroy(gameObject);
        }
        
    }

    IEnumerator Dying()
    {
        yield return new WaitForSecondsRealtime(2.75f);
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
        EnemyUI.SetActive(false);
        Destroy(transform.gameObject);
    }




}
