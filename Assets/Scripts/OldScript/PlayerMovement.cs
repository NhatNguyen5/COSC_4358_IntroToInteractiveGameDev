using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    public float dashSpeed = 20f;
    
    private float dashTime = 0.0f;
    
    public float startDashTime = 0.1f;
    
    private float dashCooldown = 0f; //cooldown
    
    public float coolDownTime = 1f;

    private bool isFacingRight = true; //control facing direction

    Vector2 mousePos;


    public Rigidbody2D rb; //player position (also a vector)

    Vector2 movement; //Stores and X and a Y
    //public Animator animator;
    // Update is called once per frame

    //public string dashSound;


    private void Update()
    {
        
        //input
        movement.x = Input.GetAxisRaw("Horizontal"); //these two has values such as 1,-1
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Rotate player
        if (isFacingRight && mousePos.x < rb.position.x)
        {
            Flip();
        }
        else if(!isFacingRight && mousePos.x > rb.position.x)
        {
            Flip();
        }

        dash();

        if (dashTime > 0)
        {
            rb.MovePosition(rb.position + movement * dashSpeed * Time.fixedDeltaTime);
            dashTime -= Time.deltaTime; 
        }
        else if (dashTime < 0)
            dashTime = 0;

        if (dashCooldown > 0)
            dashCooldown -= Time.deltaTime;
        else if (dashCooldown < 0)
            dashCooldown = 0;

        
        /*
        if (movement != Vector2.zero)
        {
            animator.SetBool("moving", true);
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
        else
        {
            animator.SetBool("moving", false);
        }*/

    }

    void FixedUpdate() //called 50 times per second
    {
        //movement
        if (dashTime > 0)
        {
            rb.MovePosition(rb.position + movement * dashSpeed * Time.fixedDeltaTime);
            //dashTime -= Time.deltaTime;
            //Debug.Log(moveSpeed * Time.fixedDeltaTime);
        }
        else
        { 
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            //Debug.Log(moveSpeed * Time.fixedDeltaTime);
        }
    }

    void dash()
    {

        //check if dash is not on cool down
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldown == 0)
        {
            //FindObjectOfType<AudioManager>().PlayEffect(dashSound);
            dashCooldown = coolDownTime;
            dashTime = startDashTime;
        }
    }
    private void Flip()
    {
        //Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}
