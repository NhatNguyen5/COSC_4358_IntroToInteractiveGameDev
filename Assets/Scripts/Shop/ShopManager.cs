using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject[] ItemsToCheck;
    public GameObject PlayerLoadOut;
    public List<GameObject> PlayerOwnedWeapons;
    public GameObject selectedWeapon;
    public GameObject button;

    public GameObject playerRightArm;
    public GameObject playerLeftArm;

    public GameObject Frame1;
    public GameObject Frame2;
    public GameObject Frame3;

    public void turnOffOtherFrames()
    {
        Frame1.SetActive(false);
        Frame2.SetActive(false);
        Frame3.SetActive(false);
    }



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
        PlayerLoadOut = GameObject.FindGameObjectWithTag("Loadout");

        //ItemsToCheck = GameObject.FindGameObjectsWithTag("ItemSlot");
        player = GameObject.FindGameObjectWithTag("Player");
        playerRightArm = GameObject.FindGameObjectWithTag("RightArm");
        playerLeftArm = GameObject.FindGameObjectWithTag("LeftArm");
        /*
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
                
                
            }
        }*/
    }


    public void updateCurrentWeaponSlot(int numberOfSlot)
    {
        selectedWeaponSlot = numberOfSlot;
    }

    /*
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
                {
                

            }
        }


    }*/


    public void showText(string text)
    {
        ShowErrorOrNot.text = text;
        counter = timeToShow;
        text2show.SetActive(true);
    }



    public void purchaseSelectedItem()
    {
        if (isWeapon == true && button.GetComponent<HoldItemToSell>().ItemBeingSold != null)
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
        else
        {
            showText("PLEASE SELECT A VALID WEAPON OR ITEM");
        }
    }


    public List<Transform> weaponTransforms;
    public Transform[] getChildrenFromArm;
    public void EquipSelectedItem()
    {
        //Debug.Log(playerRightArm.transform.childCount);
        getChildrenFromArm = playerRightArm.GetComponentsInChildren<Transform>(true);
        if (weaponTransforms.Count > 0)
        {
            Debug.Log("clearing1");
            weaponTransforms.Clear();
        }
        for (int i = 0; i < getChildrenFromArm.Length; i++)
        {
            //if (playerRightArm.transform.GetChild(1) != null)
            if (getChildrenFromArm[i].transform.parent.transform.name.Contains("RightArm") == true)
                weaponTransforms.Add(getChildrenFromArm[i]);

        }
        /*
        for (var i = weaponTransforms.Count - 1; i > -1; i--)
        {
            if (weaponTransforms[i] == null)
                weaponTransforms.RemoveAt(i);
        }
        */





        //ebug.Log(holdChildren.Length);
        if (selectedWeapon != null && button.GetComponent<HoldItemToSell>().owned == true)
        {

            if (PlayerLoadOut.GetComponent<RememberLoadout>().PrimaryWeapon == selectedWeapon.name || PlayerLoadOut.GetComponent<RememberLoadout>().SecondaryWeapon == selectedWeapon.name || PlayerLoadOut.GetComponent<RememberLoadout>().ThirdWeapon == selectedWeapon.name)
            {
              
                
                for (int i = 0; i < weaponTransforms.Count; i++)
                {
                    Transform go = weaponTransforms[i];
                    //Debug.Log(go.transform.gameObject.name);
                    if (go.transform.gameObject.GetComponent<Weapon>().Slot == selectedWeaponSlot + 1 && go.transform.gameObject.name.Contains(selectedWeapon.name) == false)
                    {
                        Destroy(go.transform.gameObject);
                        weaponTransforms.RemoveAt(i);
                    }
                }

                for (int i = 0; i < weaponTransforms.Count; i++)
                {
                    Transform go = weaponTransforms[i];
                    //Debug.Log(go.transform.gameObject.name);
                    if (go.transform.gameObject.name.Contains(selectedWeapon.name) == true)
                    {
                        go.transform.gameObject.GetComponent<Weapon>().Slot = selectedWeaponSlot + 1;
                    }
                }



                PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon1 = "";
                PlayerLoadOut.GetComponent<RememberLoadout>().PrimaryWeapon = "";
                PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon2 = "";
                PlayerLoadOut.GetComponent<RememberLoadout>().SecondaryWeapon = "";
                PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon3 = "";
                PlayerLoadOut.GetComponent<RememberLoadout>().ThirdWeapon = "";



                //Debug.Log(weaponTransforms.Count);
                for (int i = 0; i < weaponTransforms.Count; i++)
                {
                    //Debug.Log("LOOP");
                    Transform go = weaponTransforms[i];
                    if (go.transform.gameObject.GetComponent<Weapon>().Slot == 1)
                    {
                        PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon1 = go.name;
                        PlayerLoadOut.GetComponent<RememberLoadout>().PrimaryWeapon = go.name;
                    }
                    if (go.transform.gameObject.GetComponent<Weapon>().Slot == 2)
                    {
                        PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon2 = go.name;
                        PlayerLoadOut.GetComponent<RememberLoadout>().SecondaryWeapon = go.name;
                    }
                    if (go.transform.gameObject.GetComponent<Weapon>().Slot == 3)
                    {
                        PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon3 = go.name;
                        PlayerLoadOut.GetComponent<RememberLoadout>().ThirdWeapon = go.name;
                    }
                }

            }
            else
            {

                bool checkIFExist = false;

                for (int i = 0; i < weaponTransforms.Count; i++)
                {
                    Transform go = weaponTransforms[i];
                    if (go.transform.gameObject.GetComponent<Weapon>().Slot == selectedWeaponSlot + 1)
                    {
                        checkIFExist = true;
                    }
                }

                if (checkIFExist == false)
                {
                    var newWeapon = Instantiate(selectedWeapon, playerRightArm.transform, false);
                    newWeapon.GetComponent<Weapon>().Slot = selectedWeaponSlot + 1;
                    newWeapon.name = selectedWeapon.name;
                    int temp = (selectedWeaponSlot + 1);
                   

                    if (newWeapon.GetComponent<Weapon>().Slot == 1)
                    {
                        PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon1 = newWeapon.name;
                        PlayerLoadOut.GetComponent<RememberLoadout>().PrimaryWeapon = newWeapon.name;
                    }
                    if (newWeapon.GetComponent<Weapon>().Slot == 2)
                    {
                        PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon2 = newWeapon.name;
                        PlayerLoadOut.GetComponent<RememberLoadout>().SecondaryWeapon = newWeapon.name;
                    }
                    if (newWeapon.GetComponent<Weapon>().Slot == 3)
                    {
                        PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon3 = newWeapon.name;
                        PlayerLoadOut.GetComponent<RememberLoadout>().ThirdWeapon = newWeapon.name;
                    }
                    showText("WEAPON EQUIPPED TO SLOT " + temp);

                }



                if (selectedWeaponSlot == 0 && checkIFExist == true)
                {

                    for (int i = 0; i < weaponTransforms.Count; i++)
                    {
                        Transform go = weaponTransforms[i];
                        Debug.Log(go.transform.gameObject.name);
                        
                        if (go.transform.gameObject.GetComponent<Weapon>().Slot == 1)
                        {



                            if (go.transform.gameObject.activeSelf == true)
                            {
                                Destroy(go.transform.gameObject);
                                var newWeapon = Instantiate(selectedWeapon, playerRightArm.transform, false);
                                newWeapon.GetComponent<Weapon>().Slot = 1;
                                newWeapon.name = selectedWeapon.name;
                                PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon1 = selectedWeapon.name;
                                PlayerLoadOut.GetComponent<RememberLoadout>().PrimaryWeapon = selectedWeapon.name;
                            }
                            else
                            {
                                Destroy(go.transform.gameObject);
                                var newWeapon = Instantiate(selectedWeapon, playerRightArm.transform, false);
                                newWeapon.GetComponent<Weapon>().Slot = 1;
                                newWeapon.name = selectedWeapon.name;
                                newWeapon.SetActive(false);
                                PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon1 = selectedWeapon.name;
                                PlayerLoadOut.GetComponent<RememberLoadout>().PrimaryWeapon = selectedWeapon.name;
                            }
                            int temp = (selectedWeaponSlot + 1);
                            showText(selectedWeapon.name.ToUpper() + " HAS BEEN EQUIPPED TO SLOT " + temp);


                        }
                        
                    }
                }
                if (selectedWeaponSlot == 1 && checkIFExist == true)
                {

                    for (int i = 0; i < weaponTransforms.Count; i++)
                    {
                        Transform go = weaponTransforms[i];
                        Debug.Log(go.transform.gameObject.name);

                        if (go.transform.gameObject.GetComponent<Weapon>().Slot == 2)
                        {



                            if (go.transform.gameObject.activeSelf == true)
                            {
                                Destroy(go.transform.gameObject);
                                var newWeapon = Instantiate(selectedWeapon, playerRightArm.transform, false);
                                newWeapon.GetComponent<Weapon>().Slot = 2;
                                newWeapon.name = selectedWeapon.name;
                                PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon2 = selectedWeapon.name;
                                PlayerLoadOut.GetComponent<RememberLoadout>().SecondaryWeapon = selectedWeapon.name;
                            }
                            else
                            {
                                Destroy(go.transform.gameObject);
                                var newWeapon = Instantiate(selectedWeapon, playerRightArm.transform, false);
                                newWeapon.GetComponent<Weapon>().Slot = 2;
                                newWeapon.name = selectedWeapon.name;
                                newWeapon.SetActive(false);
                                PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon2 = selectedWeapon.name;
                                PlayerLoadOut.GetComponent<RememberLoadout>().SecondaryWeapon = selectedWeapon.name;
                            }
                            int temp = (selectedWeaponSlot + 1);
                            showText(selectedWeapon.name.ToUpper() + " HAS BEEN EQUIPPED TO SLOT " + temp);


                        }

                    }
                }
                if (selectedWeaponSlot == 2 && checkIFExist == true)
                {

                    for (int i = 0; i < weaponTransforms.Count; i++)
                    {
                        Transform go = weaponTransforms[i];
                        Debug.Log(go.transform.gameObject.name);

                        if (go.transform.gameObject.GetComponent<Weapon>().Slot == 3)
                        {



                            if (go.transform.gameObject.activeSelf == true)
                            {
                                Destroy(go.transform.gameObject);
                                var newWeapon = Instantiate(selectedWeapon, playerRightArm.transform, false);
                                newWeapon.GetComponent<Weapon>().Slot = 3;
                                newWeapon.name = selectedWeapon.name;
                                PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon3 = selectedWeapon.name;
                                PlayerLoadOut.GetComponent<RememberLoadout>().ThirdWeapon = selectedWeapon.name;
                            }
                            else
                            {
                                Destroy(go.transform.gameObject);
                                var newWeapon = Instantiate(selectedWeapon, playerRightArm.transform, false);
                                newWeapon.GetComponent<Weapon>().Slot = 3;
                                newWeapon.name = selectedWeapon.name;
                                newWeapon.SetActive(false);
                                PlayerLoadOut.GetComponent<RememberLoadout>().startingWeapon3 = selectedWeapon.name;
                                PlayerLoadOut.GetComponent<RememberLoadout>().ThirdWeapon = selectedWeapon.name;
                            }
                            int temp = (selectedWeaponSlot + 1);
                            showText(selectedWeapon.name.ToUpper() + " HAS BEEN EQUIPPED TO SLOT " + temp);


                        }

                    }
                }



            }

        }
        else
        {
            showText("PLEASE SELECT A OWNED WEAPON");
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
