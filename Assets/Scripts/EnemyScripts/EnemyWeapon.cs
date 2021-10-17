using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D rb;
    private bool facingDir = true;
    private float aimAngle;
    private Enemy1 enemy1;
    [HideInInspector]
    public float AimDir = 0;
    private float angle;
    public bool isNotEnemy1 = false;
    public bool isNotHoldingWeapon = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(GlobalPlayerVariables.GameOver == false)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        if(isNotEnemy1 == false)
            enemy1 = transform.parent.GetComponent<Enemy1>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && GlobalPlayerVariables.GameOver == false && enemy1 != null &&  isNotEnemy1 == false)
        {
            Vector2 direction = player.position - transform.position;
            if (enemy1.lineofsight)
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            else
                angle = enemy1.facing;
            transform.eulerAngles = new Vector3(0, 0, angle);
            AimDir = angle;
            Vector3 aimLocalScale = Vector3.one;
            if (angle > 90 || angle < -90)
            {
                //transform.position.y *= -1;
                aimLocalScale.y = -1f;
                transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
                //aimLocalScale.y = -1f * scaleX;
            }
            else
            {
                aimLocalScale.y = +1f;
                //aimLocalScale.y = -1f * scaleX;
                transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
            }
            transform.localScale = aimLocalScale;
        }
        
        if(isNotHoldingWeapon == true)
        {
            if (GlobalPlayerVariables.GameOver == false && player!=null)
            {
                Vector2 direction = player.position - transform.position;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, angle);
                //AimDir = angle;
                Vector3 aimLocalScale = Vector3.one;
                if (angle > 90 || angle < -90)
                {
                    //transform.position.y *= -1;
                    aimLocalScale.y = -1f;
                    transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
                    //aimLocalScale.y = -1f * scaleX;
                }
                else
                {
                    aimLocalScale.y = +1f;
                    //aimLocalScale.y = -1f * scaleX;
                    transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
                }
                transform.localScale = aimLocalScale;
            }
        }
    }

    private void Flip()
    {
        //Switch the way the player is labelled as facing.
        facingDir = !facingDir;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
    }



}
