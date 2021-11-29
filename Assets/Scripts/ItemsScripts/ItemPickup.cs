using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Start is called before the first frame update
    private Player player;

    private Transform playerTransform;

    private Animator Anim;

    private Vector2 followMovement;

    private float distance;

    private bool detected = false;

    public string TypeOfItem;

    public float speed;
    public float DetectRange;
    public float flingRange;
    public float PickUpRange;
    public float DespawnTime;
    [Header("Currency")]
    [SerializeField]
    private int ProteinAdd;
    [Header("Ammo")]
    [SerializeField]
    private float AmmoReserveAdd; 
    private float currDespawnTime;
    private bool startDespawn = true;
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
        Anim = transform.GetComponent<Animator>();
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
            
            //Debug.Log(distance);
            
                switch (TypeOfItem)
                {
                case "Heal":
                    if (player.Stats.NumofHeal < 99)
                    {
                        Follow();
                        if (distance <= PickUpRange)
                        {
                            player.Stats.NumofHeal += 1;
                            Destroy(gameObject);
                        }
                    }
                    break;

                case "Protein":
                    if (player.Stats.NumofProtein < 9999999)
                    {
                        Follow();
                        if (distance <= PickUpRange)
                        {
                            player.Stats.NumofProtein += ProteinAdd;
                            if (player.Stats.NumofProtein > 9999999)
                                player.Stats.NumofProtein = 9999999;
                            Destroy(gameObject);
                        }
                    }
                    break;

                case "Ammo":
                    //Debug.Log(GlobalPlayerVariables.Reserves);
                    if (GlobalPlayerVariables.Reserves < GlobalPlayerVariables.MaxReserves)
                    {
                        Follow();
                        if (distance <= PickUpRange)
                        {
                            GlobalPlayerVariables.Reserves += AmmoReserveAdd;
                            if (GlobalPlayerVariables.Reserves > GlobalPlayerVariables.MaxReserves)
                                GlobalPlayerVariables.Reserves = GlobalPlayerVariables.MaxReserves;
                            Destroy(gameObject);
                        }
                    }
                    break;

                case "Phizer":
                    //Debug.Log(GlobalPlayerVariables.Reserves);
                    if (player.Stats.NumofPhizer < 99)
                    {
                        Follow();
                        if (distance <= PickUpRange)
                        {
                            player.Stats.NumofPhizer += 1;
                            Destroy(gameObject);
                        }
                    }
                    break;

                case "Molly":
                    //Debug.Log(GlobalPlayerVariables.Reserves);
                    if (player.Stats.NumofMolly < 9)
                    {
                        Follow();
                        if (distance <= PickUpRange)
                        {
                            player.Stats.NumofMolly += 1;
                            Destroy(gameObject);
                        }
                    }
                    break;

                case "Sticky":
                    //Debug.Log(GlobalPlayerVariables.Reserves);
                    if (player.Stats.NumofSticky < 9)
                    {
                        Follow();
                        if (distance <= PickUpRange)
                        {
                            player.Stats.NumofSticky += 1;
                            Destroy(gameObject);
                        }
                    }
                    break;

                default:
                    Follow();
                    Debug.Log("Unknow item!");
                    break;
                }

                
        }
        else
        {
            if (startDespawn)
            {
                currDespawnTime = DespawnTime;
                startDespawn = false;
            }
            if (currDespawnTime > 0)
                currDespawnTime -= Time.deltaTime;
            else if (currDespawnTime < 0)
                currDespawnTime = 0;

            if(currDespawnTime < 5)
            {
                Anim.SetBool("IsDespawning", true);
            }

            if(currDespawnTime == 0)
            {
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
            Anim.SetBool("IsDespawning", false);
            //Debug.Log(playerTransform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerTransform = null;
            detected = false;
            startDespawn = true;
            //Debug.Log("Player left");
        }
    }

    private void Follow()
    {
        Vector3 direction = playerTransform.position - transform.position;
        distance = direction.magnitude;
        direction.Normalize();
        followMovement = direction;
        transform.Translate(direction * 1 * speed * Time.deltaTime);
    }
}
