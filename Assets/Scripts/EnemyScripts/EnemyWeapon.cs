using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{

    public Transform player;
    private Rigidbody2D rb;
    private bool facingDir = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {


        if (player != null)
        {
            Vector2 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Vector3 rotation = new Vector3(0f, 0f, angle);
            transform.eulerAngles = new Vector3(0, 0, angle);
            Vector3 aimLocalScale = Vector3.one;
            if (angle > 90 || angle < -90)
            {
                //transform.position.y *= -1;
                aimLocalScale.y = -1f;
                //aimLocalScale.y = -1f * scaleX;
            }
            else
            {
                aimLocalScale.y = +1f;
                //aimLocalScale.y = -1f * scaleX;
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
