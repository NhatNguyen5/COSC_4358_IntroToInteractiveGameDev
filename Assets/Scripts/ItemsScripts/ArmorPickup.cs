using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPickup : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player = collision.GetComponent<Player>();
            if (player.Stats.Armorz < 700)
                player.Stats.Armorz += 100;
            else if(player.Stats.Armorz >= 700 && player.Stats.Armorz < 799)
                player.Stats.Armorz = 799;
            //Debug.Log(player.Stats.ArmorLevel + " " + player.Stats.Armorz);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.GetComponent<Player>();
            if (player.Stats.Armorz < 798)
                player.Stats.Armorz += 2;
            Debug.Log(player.Stats.ArmorLevel + " " + player.Stats.Armorz);
        }
    }
}
