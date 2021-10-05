using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitScript : MonoBehaviour
{

    public EnemyManager enemyColony;
    public GameObject WinScreen;
    private SpriteRenderer sprite;
    private BoxCollider2D bxCol2D;
    private PolygonCollider2D plgCol2D;
    private Player player;
    private RawImage img;
    private bool reached = false;
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        sprite = transform.Find("Helicopter").GetComponent<SpriteRenderer>();
        bxCol2D = GetComponent<BoxCollider2D>();
        plgCol2D = GetComponent<PolygonCollider2D>();
        img = GameObject.Find("FadeOut").GetComponent<RawImage>();

        img.enabled = false;
        sprite.enabled = false;
        bxCol2D.enabled = false;
        plgCol2D.enabled = false;
        
        
    }
    //center of the bcl (-35.83, 18.32)
    // Update is called once per frame
    private void Update()
    {
        if(enemyColony.colonyHealth <= 0)
        {
            img.enabled = true;
            sprite.enabled = true;
            bxCol2D.enabled = true;
            plgCol2D.enabled = true;
        }
        Vector2 desPos = new Vector2(-35.83f, 18.32f);
        player.Stats.Direction = new Vector2(desPos.x - player.Stats.Position.x, desPos.y - player.Stats.Position.y).normalized;
        if (player != null)
        {
            if (player.Stats.Position != desPos)
            {
                player.Components.PlayerRidgitBody.velocity = new Vector2(player.Stats.Direction.x * player.Stats.Speed * Time.deltaTime, player.Stats.Direction.y * player.Stats.Speed * Time.deltaTime);
            }
        }

        
        if (reached)
        {
            Color imgCl = img.color;
            imgCl.a += Time.deltaTime/4;
            img.color = imgCl;
            
        }
        //Debug.Log(player.Stats.Position + "  " + desPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            player.enableControl = false;
            player.Components.PlayerRidgitBody.drag = 5;
            Debug.Log("You Won!");
            OptionSettings.GameisPaused = true;
            GlobalPlayerVariables.GameOver = true;
            WinScreen.SetActive(true);
            reached = true;
        }
    }



}
