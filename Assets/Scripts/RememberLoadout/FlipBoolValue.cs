using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipBoolValue : MonoBehaviour
{


    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        RememberLoadout.loadPlayerStats = true;
        player = GameObject.FindGameObjectWithTag("Player");
        //player.GetComponent<Player>().SetPlayerItemsAndArmorValues();

        //spawn globins
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
