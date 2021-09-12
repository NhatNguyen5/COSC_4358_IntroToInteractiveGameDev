using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    //private Transform player;
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage;
    public float knockbackForce;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        
        
        if (hitInfo.tag != "Player" && hitInfo.tag != "EnemyBullet" && hitInfo.tag != "Bullet")
        {
            //Debug.Log(hitInfo.name);
            Destroy(gameObject);
        }
      
    }


  

}
