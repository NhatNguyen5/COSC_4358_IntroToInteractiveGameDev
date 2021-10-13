using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour
{
    public float throwRange;
    public float throwSpeed;
    private Vector2 throwDir;
    public float burnDuration;
    public float puddleSize;
    public float damageOverTime;
    private Rigidbody2D rb;
    private Player player;
    private Vector2 Target;
    private bool exploded = false;
    private Vector2 OriPlayerPos;
    private CircleCollider2D cc2d;
     
    // Start is called before the first frame update
    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MoliePos = new Vector2(transform.position.x, transform.position.y);
        if ((MoliePos - Target).magnitude >= 0.5 && exploded == false)
        {
            rb.MovePosition(MoliePos + 2 * throwDir * throwSpeed * Time.fixedDeltaTime);
            if((OriPlayerPos - MoliePos).magnitude > throwRange)
            {
                exploded = true;
            }
        }
        else
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
