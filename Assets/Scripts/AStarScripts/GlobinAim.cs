using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobinAim : MonoBehaviour
{
    public Globin Glob;
    //public Transform target;
    //private Rigidbody2D rb;
    private bool facingDir = true;
    //private float aimAngle;
    //private Enemy1 enemy1;

    public Animator WeaponAnim;

    [HideInInspector]
    public float AimDir = 0;
    private float angle;
    //public bool isNotEnemy1 = false;
    //public bool isNotHoldingWeapon = false;
    // Start is called before the first frame update
    void Start()
    {
        Glob = transform.parent.GetComponent<Globin>();
        //rb = GetComponent<Rigidbody2D>();
        //if (Glob.EnemyTarget != null)
           // target = Glob.EnemyTarget;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Glob.EnemyTarget != null)
        {
            Vector2 direction = Glob.EnemyTarget.position - transform.position;
            if (Glob.canSeeEnemy == true)
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            else
            {
                //angle = enemy1.facing;
                Vector2 dir = Glob.rb.velocity;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
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
        else
        {
            //angle = enemy1.facing;
            Vector2 dir = Glob.rb.velocity;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
    }

    private void Flip()
    {
        //Switch the way the player is labelled as facing.
        facingDir = !facingDir;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
    }



}