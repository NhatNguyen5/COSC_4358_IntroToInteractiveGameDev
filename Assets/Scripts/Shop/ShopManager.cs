using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject[] ItemsToCheck;
    public List<GameObject> PlayerOwnedWeapons;
    public GameObject selectedWeapon;
    public GameObject button;
    public GameObject player;
    public int levelReqOfSelectedItem;
    public int costOfSelectedItem;
    public bool ownedWeapon;
    public bool isWeapon;


    // Start is called before the first frame update
    void Start()
    {

        PlayerOwnedWeapons = GameObject.FindGameObjectWithTag("Loadout").GetComponent<RememberLoadout>().OwnedWeapons;

        ItemsToCheck = GameObject.FindGameObjectsWithTag("ItemSlot");
        player = GameObject.FindGameObjectWithTag("Player");
        foreach (GameObject ownedweapon in PlayerOwnedWeapons)
        {

            foreach (GameObject iteminshop in ItemsToCheck)
            {
               
                //Debug.Log("p1");
                HoldItemToSell ye = iteminshop.GetComponent<HoldItemToSell>();
                if (ye.ItemBeingSold != null)
                {
                    if (ye.isWeapon == true && ye.ItemBeingSold.name == ownedweapon.name)
                    {
                        //Debug.Log("p2");
                        ye.cost = 0;
                        ye.owned = true;
                    }
                }
                /*
                else
                {
                    Debug.Log("Item has not been assigned");
                }
                */
                
            }
        }
    }


    void updateprices()
    {
        foreach (GameObject ownedweapon in PlayerOwnedWeapons)
        {

            foreach (GameObject iteminshop in ItemsToCheck)
            {

                //Debug.Log("p1");
                HoldItemToSell ye = iteminshop.GetComponent<HoldItemToSell>();
                if (ye.ItemBeingSold != null)
                {
                    if (ye.isWeapon == true && ye.ItemBeingSold.name == ownedweapon.name)
                    {
                        //Debug.Log("p2");
                        ye.cost = 0;
                        ye.owned = true;
                    }
                }
                /*
                else
                {
                    Debug.Log("Item has not been assigned");
                }
                */

            }
        }


    }


    public void purchaseSelectedItem()
    {
        if (isWeapon == true)
        {
            if (button.GetComponent<HoldItemToSell>().owned == false)
            {

                if (player.GetComponent<Player>().getProteinAmount() > button.GetComponent<HoldItemToSell>().cost)
                {

                    PlayerOwnedWeapons.Add(selectedWeapon);
                    player.GetComponent<Player>().subtractProtienCounter(button.GetComponent<HoldItemToSell>().cost);
                    button.GetComponent<HoldItemToSell>().owned = true;
                    button.GetComponent<HoldItemToSell>().cost = 0;
                    button.GetComponent<HoldItemToSell>().switchText();

                    //add text object to notify player something has been bought
                }


                //if(player.GetComponent<>)
            }
        }
    }

    public void EquipSelectedItem()
    { 
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
