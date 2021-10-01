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
    public float PickUpRange;
    private Vector2 randomDirection;
    private Vector3 initialPos;

    private void Start()
    {
        transform.GetComponent<BoxCollider2D>().edgeRadius = DetectRange;
        float xDir = Random.Range(-100, 100);
        float yDir = Random.Range(-100, 100);
        initialPos = transform.position;
        randomDirection = new Vector2(xDir, yDir).normalized;
    }

    // Update is called once per frame
    private void Update()
    {
        if((initialPos- transform.position).magnitude <= 1)
            transform.Translate(randomDirection * 1 * speed * Time.deltaTime);
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        if(collider.tag == "Player")
        {
            playerTransform = collider.GetComponent<Transform>();
            player = collider.GetComponent<Player>();
            detected = true;
            //Debug.Log(playerTransform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            playerTransform = null;
            detected = false;
            //Debug.Log("Player left");
        }
    }
}
