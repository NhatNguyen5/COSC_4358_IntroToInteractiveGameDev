using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyGrenade : MonoBehaviour
{
    // Start is called before the first frame update
    public float throwRange;
    public float throwSpeed;
    public float timer;
    public float SecondForOneFlash;
    public GrenadeExplosion greExpl;
    private Vector2 throwDir;
    private Rigidbody2D rb;
    private Player player;
    private Vector2 Target;
    private Vector2 OriPlayerPos;
    private CircleCollider2D cc2d;
    private SpriteRenderer spriteRend;
    private Transform explSprite;
    /*
    private float throwDistance;
    private Vector2 oldPos;
    private Vector2 newPos;
    private float distanceLeft;
    */
    private bool stuck = false;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CircleCollider2D>();
        explSprite = transform.Find("ExplosionSprite");
        explSprite.localScale *= greExpl.explodeRadius;
        spriteRend = explSprite.GetComponent<SpriteRenderer>();
        spriteRend.color = new Color(1, 0, 0, 0);
        if (GlobalPlayerVariables.GameOver == false)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        else
            player = this.GetComponent<Player>();
        throwDir = player.References.MousePosToPlayer;
        Target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        OriPlayerPos = player.Stats.Position;
        //oldPos = OriPlayerPos;
        //throwDistance = (Target - OriPlayerPos).magnitude;
        //Debug.Log(throwDistance);
        //distanceLeft = throwDistance;
        cc2d.isTrigger = true;
        //rb.AddForce(throwDir * throwDistance * throwSpeed * 1.75f, ForceMode2D.Impulse);
        rb.AddForce(throwDir * throwRange * throwSpeed * 1.75f, ForceMode2D.Impulse);
        var impulse = (30 * Mathf.Deg2Rad) * -10;
        rb.AddTorque(impulse, ForceMode2D.Impulse);
        StartCoroutine(countDown());
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 GrenadePos = new Vector2(transform.position.x, transform.position.y);
        float distance = (OriPlayerPos - GrenadePos).magnitude;

        if ((distance >= throwRange || (GrenadePos - Target).magnitude <= 0.5))
        {
            rb.drag = 50;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            
            transform.GetComponent<TrailRenderer>().enabled = false;
        }
        float flashInDuration = SecondForOneFlash / 2;
        for (float t = 0; t <= flashInDuration; t += Time.deltaTime)
        {
        }

            //Debug.Log(distanceLeft);
        }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (distanceLeft > 0)
        {
            newPos = collision.transform.position;
            float traveledDis = (newPos - oldPos).magnitude;
            distanceLeft -= traveledDis;
            oldPos = newPos;
        }
        */
        if (!stuck)
        {
            rb.drag = 50;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            cc2d.isTrigger = true;
            rb.isKinematic = true;
            stuck = true;
            transform.GetComponent<TrailRenderer>().enabled = false;
            if (collision.transform.tag != "Walls")
                transform.parent = collision.transform; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !stuck)
        {
            cc2d.isTrigger = false;
        }
    }

    private IEnumerator countDown()
    {
        yield return new WaitForSeconds(timer);
        Instantiate(greExpl, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
