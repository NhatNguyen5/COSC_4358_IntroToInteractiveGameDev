using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour
{
    public float throwRange;
    public float throwSpeed;
    private Vector2 throwDir;
    private Rigidbody2D rb;
    private Player player;
    private Vector2 Target;
    private bool exploded = false;
    private Vector2 OriPlayerPos;
    private Vector2 explodedPos;
    private CircleCollider2D cc2d;
    public Puddle puddle;

     
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CircleCollider2D>();
        if (GlobalPlayerVariables.GameOver == false)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        else
            player = this.GetComponent<Player>();
        throwDir = player.References.MousePosToPlayer;
        Target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        OriPlayerPos = player.Stats.Position;
        cc2d.isTrigger = true;
        rb.AddForce(throwDir * throwRange * throwSpeed * 1.75f, ForceMode2D.Impulse);
        var impulse = (30*Mathf.Deg2Rad) * -10;
        rb.AddTorque(impulse, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 MollyPos = new Vector2(transform.position.x, transform.position.y);
        float distance = (OriPlayerPos - MollyPos).magnitude;
        if (distance >= throwRange || (MollyPos - Target).magnitude <= 0.5 || exploded)
        {
            rb.drag = 100;
            explodedPos = MollyPos;
            exploded = true;
        }

        if(exploded)
        {
            Instantiate(puddle, explodedPos, Quaternion.identity);
            //detach particle effect so it won't disapear suddenly
            Transform tempPS;
            tempPS = transform.Find("Particle");
            tempPS.GetComponent<ParticleSystem>().Stop();
            tempPS.parent = null;
            Destroy(tempPS.gameObject, tempPS.GetComponent<ParticleSystem>().main.duration);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        exploded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            cc2d.isTrigger = false;
        }
    }
}
