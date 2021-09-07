using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform player;
    public Vector2 followMovement;


    public float Xspeed;
    public float Yspeed;

    public bool canShoot;
    public bool canfollow;
    public float fireRate;
    public float health;

    public string deathSound;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
       
    }

    void Update()
    {


        if (canfollow == true)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            direction.Normalize();
            followMovement = direction;
        }
        else
        {
            rb.velocity = new Vector2(Xspeed * -1, Yspeed);
        }


    }

    private void FixedUpdate()
    {
        if (canfollow == true)
        {
            followCharacter(followMovement);
        }
    }

    void followCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position - (direction * Xspeed*-1 * Time.deltaTime));
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //damage player code goes here
            takeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Bullet")
        {
            //damage player code goes here
            takeDamage();
        }
    }

    void takeDamage()
    {
        health--;
        if (health < 1)
        {
            FindObjectOfType<AudioManager>().PlayEffect(deathSound);
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
