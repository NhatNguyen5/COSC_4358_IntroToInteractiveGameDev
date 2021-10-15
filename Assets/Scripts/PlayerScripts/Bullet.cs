using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    //private Transform player;
    public float speed = 20f;
    public Rigidbody2D rb;
    public float damage;
    public float knockbackForce;
    public bool pierce;
    public int targetToPierce;
    public float dropOffPerTarget;
    public bool isLeftWeapon = false;
    private int count;

    public float bulletDamageDropOff;
    public float timeToDropDmg;

    private float timebeforedrop = 0;

    public float critRate = 0;
    public float critDMG = 0;

    //explosion settings
    public GameObject explosion;
    private bool isBoom = false;
    public bool isExplosiveBullet = false;

    private Vector2 randPos;
    public float circleRadius;

    /*
    private Vector3 spawnPos;
    private float distanceFromStart = 0;
    public float when2screenshake = 20;


    public bool CameraShake;
    public float ShakeDuration;
    public float ShakeIntensity;

    private StatusIndicator statusIndicator = null;
    private Camera Mcamera;

    */



    // Start is called before the first frame update
    void Start()
    {

        //spawnPos = transform.position;
        if (isLeftWeapon == false)
        {
            speed = GlobalPlayerVariables.bulletSpeed;
            damage = GlobalPlayerVariables.bulletDamage;
            knockbackForce = GlobalPlayerVariables.bulletKnockbackForce;
            pierce = GlobalPlayerVariables.bulletPierce;
            targetToPierce = GlobalPlayerVariables.targetsToPierce;
            dropOffPerTarget = GlobalPlayerVariables.damageDropOff;
            bulletDamageDropOff = GlobalPlayerVariables.bulletDamageDropOff;
            timeToDropDmg = GlobalPlayerVariables.timeToDropDmg;
            critRate = GlobalPlayerVariables.critRate;
            critDMG = GlobalPlayerVariables.critDmg;
            isExplosiveBullet = GlobalPlayerVariables.bulletExplosion;
        }
        if (isLeftWeapon == true)
        {
            speed = GlobalPlayerVariables.bulletSpeed2;
            damage = GlobalPlayerVariables.bulletDamage2;
            knockbackForce = GlobalPlayerVariables.bulletKnockbackForce2;
            pierce = GlobalPlayerVariables.bulletPierce2;
            targetToPierce = GlobalPlayerVariables.targetsToPierce2;
            dropOffPerTarget = GlobalPlayerVariables.damageDropOff2;
            bulletDamageDropOff = GlobalPlayerVariables.bulletDamageDropOff2;
            timeToDropDmg = GlobalPlayerVariables.timeToDropDmg2;
            critRate = GlobalPlayerVariables.critRate2;
            critDMG = GlobalPlayerVariables.critDmg2;
            isExplosiveBullet = GlobalPlayerVariables.bulletExplosion2;
        }


        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {


        //if (hitInfo.tag != "Player" && hitInfo.tag != "EnemyBullet" && hitInfo.tag != "Bullet")
        if (pierce == false)
        {
            if (isExplosiveBullet == false)
            {
                if (hitInfo.tag == "Enemy" || hitInfo.tag == "EnemyMelee" || hitInfo.tag == "Walls" || hitInfo.tag == "Colony" || hitInfo.tag == "EnemyBullet2")
                {
                    //Debug.Log(hitInfo.name);
                    Destroy(gameObject);
                }
            }
            else
            {
                if (hitInfo.tag == "Enemy" || hitInfo.tag == "EnemyMelee" || hitInfo.tag == "Walls" || hitInfo.tag == "Colony" || hitInfo.tag == "EnemyBullet2")
                {
                    //Debug.Log(hitInfo.name);
                    explode();
                }
            }
        }
        if (pierce == true)
        {
            if (hitInfo.tag == "Walls")
            {
                //Debug.Log(hitInfo.name);
                Destroy(gameObject);
            }
            if (hitInfo.tag == "Enemy" || hitInfo.tag == "EnemyMelee" || hitInfo.tag == "Colony" || hitInfo.tag == "EnemyBullet2")
            {
                //Debug.Log(hitInfo.name);

                count++;
            }
            if (count > targetToPierce)
                Destroy(gameObject);


        }

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
            //sprite.color = Color.red;
        }

        if (isBoom == true)
        {
            //rb.velocity *= 0;

                //SPAWN EXPLOSION OBJECT
                randPos = transform.position;
                randPos += Random.insideUnitCircle * circleRadius;
                Instantiate(explosion, randPos, Quaternion.Euler(0, 0, 0));
                Destroy(gameObject);
            

        }

    }





    private void OnTriggerExit2D(Collider2D collision)
    {

        if (pierce == true)
        {

            if (collision.tag == "Enemy" || collision.tag == "EnemyMelee" || collision.tag == "Colony")
            {
                damage *= (1 - dropOffPerTarget);
            }


        }
    }


    private void Update()
    {
        /*
        if (isExplosiveBullet == true)
        {
            distanceFromStart = Vector3.Distance(spawnPos, transform.position);
            if (CameraShake && distanceFromStart < when2screenshake)
                statusIndicator.StartShake(Mcamera, ShakeDuration, ShakeIntensity);
        }
        */


        timebeforedrop += Time.deltaTime;
        if (timebeforedrop > timeToDropDmg)
        {

            //damage = Time.deltaTime / (1 - bulletDamageDropOff);
            timebeforedrop = 0;
            damage *= (1 - bulletDamageDropOff);
        }
    }





}
