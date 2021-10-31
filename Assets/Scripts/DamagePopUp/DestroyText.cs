using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyText : MonoBehaviour
{
    public float DestroyTime = .5f;
    private Rigidbody2D rb;
    private Vector2 movement = new Vector2(0, 0);
    private float TextSpeed;
    public string layerToPushTo;

    Vector3 Offset = new Vector3(0, 0, 0);

    private void Start()
    {
        GetComponent<Renderer>().sortingLayerName = layerToPushTo;
        rb = GetComponent<Rigidbody2D>();
    }

    //default damage popup
    public void spawnPos(float x = 0, float y = 0, float Speed = 0)
    {
        //float xcount = Random.Range(-0.5f, 0.5f);
        //float ycount = Random.Range(-0.5f, 0.5f);

        float xcount = Random.Range(-0.5f, 0.5f);
        float ycount = Random.Range(-0.5f, 0.5f);
        Vector3 Offset = new Vector3(x + xcount, y + ycount, 0);
        transform.localPosition += Offset;

        var otherPosn = transform.localPosition;
        transform.localPosition = new Vector3(otherPosn.x, otherPosn.y, -1);


        movement.x = x;
        movement.y = y;
        //OLD
        //TextSpeed = Speed;

        TextSpeed = Speed/3;
        //Offset = new Vector3(x, y, 0);
        //rb.MovePosition(rb.position + movement);
        Destroy(gameObject, DestroyTime);
        //movement.x = 
        //transform.localPosition += Offset;
    }
    

    /*maplestory damage popup
    public void spawnPos(float x = 0, float y = 0, float Speed = 0)
    {
        //float xcount = Random.Range(-0.5f, 0.5f);
        //float ycount = Random.Range(-0.5f, 0.5f);

        float xcount = Random.Range(-0.5f, 0.5f);
        //float ycount = Random.Range(-0.5f, 0.5f);
        float ycount = Random.Range(1, 1f);
        Vector3 Offset = new Vector3(x + xcount, y + ycount, 0);
        transform.localPosition += Offset;

        var otherPosn = transform.localPosition;
        transform.localPosition = new Vector3(otherPosn.x, otherPosn.y, -1);

        //movement.x = x;

        movement.x = 0;
        movement.y = Mathf.Abs(y);
        //OLD
        //TextSpeed = Speed;

        TextSpeed = Speed/2;
        //Offset = new Vector3(x, y, 0);
        //rb.MovePosition(rb.position + movement);
        Destroy(gameObject, DestroyTime);
        //movement.x = 
        //transform.localPosition += Offset;
    }
    */

    public void Update()
    {
        movement.Normalize();
        rb.MovePosition(rb.position + movement * TextSpeed * Time.fixedDeltaTime);

        //transform.position.x = transform.position.x + x1; 
        //transform.localPosition += Offset;

    }
}

