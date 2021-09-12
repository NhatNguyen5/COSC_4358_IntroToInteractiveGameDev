using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProj : MonoBehaviour
{
    public float speed;
    public float despawnTime;
    public int damage;
    public bool homing;

    private Vector2 followMovement;

    private Transform player;
    private Vector2 target;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y);
        rb = GetComponent<Rigidbody2D>();
        if (homing == false)
        {
            rb.velocity = transform.right * speed;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (homing == true)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            direction.Normalize();
            followMovement = direction;

            
        }

        despawnTime -= Time.deltaTime;
        if (despawnTime <= 0)
        {
            DestroyEnemyProj();
        }

    }

    private void FixedUpdate()
    {
        if (homing == true)
            followCharacter(followMovement);
    }

    void followCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position - (direction * speed * -1 * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            DestroyEnemyProj();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.name == "Player")
            DestroyEnemyProj();
    }


    void DestroyEnemyProj()
    {
        Destroy(gameObject);
    }
}
