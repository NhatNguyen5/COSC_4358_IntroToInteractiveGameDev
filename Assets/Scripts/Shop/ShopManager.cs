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

    public int selectedWeaponSlot;

    public GameObject player;

    public GameObject text2show;
    public Text ShowErrorOrNot;
    public float timeToShow;
    private float counter;

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


    public void updateCurrentWeaponSlot(int numberOfSlot)
    {
        selectedWeaponSlot = numberOfSlot;
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


    public void showText(string text)
    {
        ShowErrorOrNot.text = text;
        counter = timeToShow;
        text2show.SetActive(true);
    }



    public void purchaseSelectedItem()
    {
        if (isWeapon == true)
        {
            if (button.GetComponent<HoldItemToSell>().owned == false)
            {

                if (player.GetComponent<Player>().getProteinAmount() >= button.GetComponent<HoldItemToSell>().cost)
                {
                    if (player.GetComponent<Player>().getcurrentlevel() >= button.GetComponent<HoldItemToSell>().levelReq)
                    {

                        PlayerOwnedWeapons.Add(selectedWeapon);
                        player.GetComponent<Player>().subtractProtienCounter(button.GetComponent<HoldItemToSell>().cost);
                        button.GetComponent<HoldItemToSell>().owned = true;
                        button.GetComponent<HoldItemToSell>().cost = 0;
                        button.GetComponent<HoldItemToSell>().switchText();
                        showText("PURCHASE SUCCESSFUL");
                        //add text object to notify player something has been bought
                    }
                    else
                    {
                        showText("ERROR NOT A HIGH ENOUGH LEVEL");
                    }
                }
                else
                {
                    showText("ERROR NOT ENOUGH PROTEIN");
                }


                //if(player.GetComponent<>)
            }
            else
            {
                showText("THIS WEAPON IS ALREADY OWNED");
            }
        }
    }

    public void EquipSelectedItem()
    {
        if (selectedWeapon != null)
        {
            //start process to equip a weapon
        }
        else
        {
            showText("PLEASE SELECT A WEAPON");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;
        }
        if (counter <= 0)
        {
            text2show.SetActive(false);
        }
    }
}
