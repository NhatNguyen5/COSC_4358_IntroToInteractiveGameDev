using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Start is called before the first frame update
    private Player player;

    private Transform playerTransform;

    private Vector2 followMovement;

    private float distance;

    private bool detected = false;

    public string TypeOfItem;

    public float speed;
    public float DetectRange;
    public float flingRange;
    public float PickUpRange;
    private Vector2 randomDirection;
    private BoxCollider2D bcd;
    private bool triggerDetect = true;

    private void Start()
    {
        bcd = transform.GetComponent<BoxCollider2D>();
        bcd.edgeRadius = 0;
        bcd.isTrigger = false;
        float xDir = Random.Range(-100, 100);
        float yDir = Random.Range(-100, 100);
        randomDirection = new Vector2(xDir, yDir).normalized;
        transform.GetComponent<Rigidbody2D>().AddForce(randomDirection * flingRange, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    private void Update()
    {
        //Vector3 dashPosition = player.Stats.Position + player.Stats.Direction * DashDistance;

        //Debug.Log(flingRange);
        //Debug.Log(transform.GetComponent<Rigidbody2D>().velocity.magnitude);
        if(transform.GetComponent<Rigidbody2D>().velocity.magnitude == 0 && triggerDetect)
        {
            bcd.edgeRadius = DetectRange;
            bcd.isTrigger = true;
            //Debug.Log("change");
            triggerDetect = false;
        }

        if (detected)
        {
            Vector3 direction = playerTransform.position - transform.position;
            distance = direction.magnitude;
            direction.Normalize();
            followMovement = direction;
            transform.Translate(direction * 1 * speed * Time.deltaTime);
            //Debug.Log(distance);
            if (distance <= PickUpRange)
            {
                switch(TypeOfItem)
                {
                    case "Heal":
                        player.Stats.NumofHeal += 1;
                        break;
                    default:
                        Debug.Log("Unknow item!");
                        break;
                }
                
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            playerTransform = collision.collider.GetComponent<Transform>();
            player = collision.collider.GetComponent<Player>();
            detected = true;
            bcd.edgeRadius = DetectRange;
            bcd.isTrigger = true;
            //Debug.Log(playerTransform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Player")
        {
            playerTransform = collision.GetComponent<Transform>();
            player = collision.GetComponent<Player>();
            detected = true;
            //Debug.Log(playerTransform.position);
        }
    }

    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerTransform = null;
            detected = false;
            //Debug.Log("Player left");
        }
    }
}