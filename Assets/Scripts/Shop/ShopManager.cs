using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject[] ItemsToCheck;
    public GameObject[] PlayerOwnedWeapons;
    // Start is called before the first frame update
    void Start()
    {

        PlayerOwnedWeapons = GameObject.FindGameObjectWithTag("Loadout").GetComponent<RememberLoadout>().OwnedWeapons;
        ItemsToCheck = GameObject.FindGameObjectsWithTag("ItemSlot");
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
