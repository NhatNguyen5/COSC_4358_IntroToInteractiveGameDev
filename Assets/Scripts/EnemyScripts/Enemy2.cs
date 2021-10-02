using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
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
    //public float retreatDistance = 0;
    public bool followPlayer;
    //public bool retreat;
    public bool randomMovement;
    public float timeRunningTwdPlayer;
    private float runningTimeOut;

    [Header("Random Movement Settings")]
    public float circleRadius;
    public float timeTillNextMove;

    //public bool DodgeWhileCharging;



    private float NextMoveCoolDown;
    private bool reachedDestination;
    private Vector2 randPos;

    private float knockbackForce = 0;
    [Header("KnockBack Settings")]
    //public float knockForcePlayerContact;
    private bool knockback = false;
    public float knockbackstartrange = 0.4f;
    public float knockbackendrange = 1.0f;
    private float knockbacktime;




    public Transform player;
    private Rigidbody2D rb;


    [Header("Melee Settings")]
    //public float DistanceToPlayer;
    

    public float beginningrangetomove = 0;
    public float endingrangetomove = 0;
    private float chaseCoolDown;

    public float RetreatSpeed;
    public float retreatTime = 0.1f;
    private float chaseCoolDownTimer;
    private bool hitPlayer;

    [Header("Line Of Sight")]
    public bool LineOfSight;


    //DEATH VARIABLE
    private bool isDead = false;

    //ANIMATION VARIABLES
    public LayerMask IgnoreMe;
    Vector2 direction;
    float a;

    [Header("Drops")]
    public GameObject Tylenol;
    public float DropPercentageTylenol;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        attack();


    }

    void variation()
    {
        chaseCoolDown = Random.Range(beginningrangetomove, endingrangetomove);
        chaseCoolDownTimer = chaseCoolDown;
    }


    private void FixedUpdate()
    {



        if (knockback == true)
        {

            //Debug.Log("KNOCKBACK");
            if (hitPlayer == false)
                transform.position = Vector2.MoveTowards(transform.position, player.position, -knockbackForce * speed * Time.deltaTime);
            else
            {
                //Vector2 Offset = player.position;

                transform.position = Vector2.MoveTowards(transform.position, randPos, RetreatSpeed * speed * Time.deltaTime);
            }
            getDirection(player);

        }
        else
        {

            if (Vector2.Distance(transform.position, player.position) > stoppingDistance && followPlayer == true) //follow player
            {
                reachedDestination = true;
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                //go to position 
                getDirection(player);

                //Debug.Log(transform);
            }
            else
            {
                if (randomMovement == false)
                    transform.position = this.transform.position;
                else //RANDOM MOVEMENT
                {
                    //Debug.Log("RANDOMPOS");
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
            /*
            else if (Vector2.Distance(transform.position, player.position) < retreatDistance && retreat == true) //retreat
            {
                reachedDestination = true;
                transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            }
            */

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

        knockbacktime -= Time.deltaTime;
        if (knockbacktime <= 0)
        {
            knockbacktime = 0;
            knockback = false;
        }

        if (player != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, Mathf.Infinity, ~IgnoreMe);
            //var rayDirection = player.position - transform.position;
            //Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            if (hit.collider.gameObject.tag == "Player")
            {
                LineOfSight = true;
                //Debug.Log("Player is Visable");
                // enemy can see the player!

                //Debug.Log("Player is Visable");
            }
            else
            {
                LineOfSight = false;
                //Debug.Log("Player is NOT Visable");
            }
        }


        if (LineOfSight == true)
        {
            if (followPlayer == true)
            {
                runningTimeOut += Time.deltaTime;
                if (runningTimeOut >= timeRunningTwdPlayer)
                {
                    randomRetreatPos();
                    reachedDestination = true;
                    NextMoveCoolDown = timeTillNextMove;
                    knockbacktime = retreatTime;
                    knockback = true;
                    attack();
                    runningTimeOut = 0;
                }
            }
            //Debug.Log(a); //ANGLE




            if (chaseCoolDownTimer <= 0)
            {
                resumeFollow();
            }
            if (NextMoveCoolDown <= 0 && reachedDestination == true && hitPlayer == true)
            {

                randomPos();
            }
            NextMoveCoolDown -= Time.deltaTime;
            chaseCoolDownTimer -= Time.deltaTime;
        }
        else
        {
            //reachedDestination = true;
            //NextMoveCoolDown = timeTillNextMove;
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

        if (collision.gameObject.tag != "Enemy")
        {
            //knockbackForce = knockForcePlayerContact;
            knockbacktime = retreatTime;
            knockback = true;
            //reachedDestination = true;
            attack();
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

            //reachedDestination = true;

            knockbacktime = Random.Range(knockbackstartrange, knockbackendrange);
            knockback = true;
            hitPlayer = false;

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


    void attack()
    {

        hitPlayer = true;
        followPlayer = false;
        NextMoveCoolDown = 0;
        variation();
        //reachedDestination = true;
        //randomPos();
        //NextMoveCoolDown = timeTillNextMove;
    }

    void resumeFollow()
    {
        reachedDestination = true;
        hitPlayer = false;
        followPlayer = true;
    }

    void randomRetreatPos()
    {
        randPos = transform.position;
        randPos += Random.insideUnitCircle * circleRadius;
    }

    void randomPos()
    {

        NextMoveCoolDown = timeTillNextMove;
        randPos = transform.position;
        randPos += Random.insideUnitCircle * circleRadius;
        reachedDestination = false;


    }


    void Die()
    {
        if (isDead == false)
        {
            isDead = true;
            if (OnEnemyKilled != null)
            {
                OnEnemyKilled();
            }
            if (Random.Range(0, 100) <= DropPercentageTylenol)
                Instantiate(Tylenol, transform.position, Quaternion.Euler(0, 0, 0));
            GameObject.Destroy(gameObject);
        }
    }


}
