using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldItemToSell : MonoBehaviour
{

    public int cost;
    public int levelReq;
    public GameObject ItemBeingSold;
    public bool owned = false;
    public bool isWeapon = false;

    private int num1;
    private int num2;
    private int num3;

    FindText parentOfObj;
    public ShopManager SM;

    bool checkIfOwned = false;

    public void switchText()
    {
        parentOfObj.itemCost.text = cost.ToString();
    }

    public void levelReqs()
    {
        parentOfObj.LevelReq3.text = num3.ToString();
        parentOfObj.LevelReq2.text = num2.ToString();
        parentOfObj.LevelReq1.text = num1.ToString();
    }

    public void updateSelect()
    {
        ShopManager ShopM = SM.GetComponent<ShopManager>();
        if (isWeapon == true)
            ShopM.selectedWeapon = ItemBeingSold;
        else
            ShopM.selectedWeapon = null;
        ShopM.costOfSelectedItem = cost;
        ShopM.levelReqOfSelectedItem = levelReq;
        ShopM.ownedWeapon = owned;
        ShopM.isWeapon = isWeapon;
        ShopM.button = gameObject;

    }


    //public GameObject DaLoadout;

    // Start is called before the first frame update
    void Start()
    {
        SM = GameObject.FindGameObjectWithTag("ShopManager").GetComponent<ShopManager>();
        parentOfObj = transform.parent.GetComponent<FindText>();
        num3 = (((int)levelReq / 100) % 10);
        num2 = (((int)levelReq / 10) % 10);
        num1 = ((int)levelReq % 10);


        if (ItemBeingSold != null)
        {
            Debug.Log("SETTING UP");
            List<GameObject> PlayerOwnedWeapons = GameObject.FindGameObjectWithTag("Loadout").GetComponent<RememberLoadout>().OwnedWeapons;
            foreach (GameObject ownedweapon in PlayerOwnedWeapons)
            {


                if (ItemBeingSold != null)
                {
                    if (isWeapon == true && ItemBeingSold.name == ownedweapon.name)
                    {
                        //Debug.Log("p2");
                        cost = 0;
                        owned = true;
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
        /*
        parentOfObj.itemCost.text = cost.ToString();
        parentOfObj.LevelReq3.text = num3.ToString();
        parentOfObj.LevelReq2.text = num2.ToString();
        parentOfObj.LevelReq1.text = num1.ToString();
        */
    }

}
