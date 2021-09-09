using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{


    public float speed = 0;
    public float stoppingDistance = 0;
    public float retreatDistance = 0;
    public bool retreat;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject projectile;
    public Transform firePoint;
    public Transform player;
    //private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;
    }


    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance) //follow player
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance) //stop
        {
            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance && retreat == true) //retreat
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        
        if (timeBtwShots <= 0)
        {
            Instantiate(projectile, firePoint.position, firePoint.rotation);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }

    }

    



}
