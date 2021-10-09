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

    // Start is called before the first frame update
    void Start()
    {
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
        }


        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {


        //if (hitInfo.tag != "Player" && hitInfo.tag != "EnemyBullet" && hitInfo.tag != "Bullet")
        if (pierce == false)
        {
            if (hitInfo.tag == "Enemy" || hitInfo.tag == "EnemyMelee" || hitInfo.tag == "Walls")
            {
                //Debug.Log(hitInfo.name);
                Destroy(gameObject);
            }
        }
        if (pierce == true)
        {
            if (hitInfo.tag == "Walls")
            {
                //Debug.Log(hitInfo.name);
                Destroy(gameObject);
            }
            if (hitInfo.tag == "Enemy" || hitInfo.tag == "EnemyMelee")
            {
                //Debug.Log(hitInfo.name);

                count++;
            }
            if (count > targetToPierce)
                Destroy(gameObject);


        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (pierce == true)
        {

            if (collision.tag == "Enemy" || collision.tag == "EnemyMelee")
            {
                damage *= (1 - dropOffPerTarget);
            }


        }
    }


    private void Update()
    {
        timebeforedrop += Time.deltaTime;
        if (timebeforedrop > timeToDropDmg)
        {

            //damage = Time.deltaTime / (1 - bulletDamageDropOff);
            timebeforedrop = 0;
            damage *= (1 - bulletDamageDropOff);
        }
    }





}
