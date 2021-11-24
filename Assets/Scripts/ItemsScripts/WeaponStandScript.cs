using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStandScript : MonoBehaviour
{
    public GameObject DisplayedWeapon;
    private GameObject RightArm;
    private GameObject LeftArm;
    private bool PlayerIsNear;
    private Player player;
    private bool[] PrimaryWpSlot = { false, false, false };
    public bool isMeleeWeapon;
    

    // Start is called before the first frame update
    private void Start()
    {
        transform.Find("WeaponSprite").GetComponent<SpriteRenderer>().sprite = DisplayedWeapon.transform.GetComponent<SpriteRenderer>().sprite;
        if(DisplayedWeapon.transform.GetComponent<Weapon>() != null)
            transform.Find("Canvas").transform.Find("WeaponName").GetComponent<Text>().text = DisplayedWeapon.transform.GetComponent<Weapon>().WeaponLabel;
        else if(DisplayedWeapon.transform.GetComponent<MeleeWeapon>() != null)
            transform.Find("Canvas").transform.Find("WeaponName").GetComponent<Text>().text = DisplayedWeapon.transform.GetComponent<MeleeWeapon>().WeaponLabel;
        else if (DisplayedWeapon.transform.GetComponent<ShieldScript>() != null)
            transform.Find("Canvas").transform.Find("WeaponName").GetComponent<Text>().text = DisplayedWeapon.transform.GetComponent<ShieldScript>().WeaponLabel;
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayerIsNear)
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            GameObject RightArm = GameObject.FindGameObjectWithTag("RightArm");
            GameObject LeftArm = GameObject.FindGameObjectWithTag("LeftArm");
            float currSlot = 0;
            if (OptionSettings.GameisPaused == false)
            {
                bool isHoldingWeapon = false;
                bool isHoldingMelee = false;
                foreach (Transform wp in RightArm.transform)
                {
                    if (wp.GetComponent<Weapon>() != null)
                    {
                        if (wp.gameObject.activeSelf)
                        {
                            currSlot = wp.GetComponent<Weapon>().Slot;
                            isHoldingWeapon = true;
                        }
                        if (wp.GetComponent<Weapon>().Slot == 1)
                            PrimaryWpSlot[0] = true;
                        else if (wp.GetComponent<Weapon>().Slot == 2)
                            PrimaryWpSlot[1] = true;
                        else if (wp.GetComponent<Weapon>().Slot == 3)
                            PrimaryWpSlot[2] = true;
                    }

                    if (wp.GetComponent<MeleeWeapon>() != null || wp.GetComponent<ShieldScript>() != null)
                    {
                        if (wp.gameObject.activeSelf)
                        {
                            currSlot = 4;
                            isHoldingWeapon = true;
                            isHoldingMelee = true;
                        }
                    }

                    Debug.Log(wp.gameObject.name + " " + currSlot);
                }
                if (!isMeleeWeapon)
                {
                    if ((Input.GetKeyDown(KeyCode.Alpha1) && currSlot == 1) || (!isHoldingWeapon && !PrimaryWpSlot[0]))
                    {
                        if (isHoldingWeapon)
                            DestroyWeaponInSlot(1, RightArm);
                        else
                            PrimaryWpSlot[0] = true;
                        var newWeapon = Instantiate(DisplayedWeapon, RightArm.transform, false);
                        newWeapon.GetComponent<Weapon>().Slot = 1;
                    }
                    else if ((Input.GetKeyDown(KeyCode.Alpha2) && currSlot == 2) || (!isHoldingWeapon && !PrimaryWpSlot[1]))
                    {
                        if (isHoldingWeapon)
                            DestroyWeaponInSlot(2, RightArm);
                        else
                            PrimaryWpSlot[1] = true;
                        var newWeapon = Instantiate(DisplayedWeapon, RightArm.transform, false);
                        newWeapon.GetComponent<Weapon>().Slot = 2;
                    }
                    else if ((Input.GetKeyDown(KeyCode.Alpha3) && currSlot == 3) || (!isHoldingWeapon && !PrimaryWpSlot[2]))
                    {
                        if (isHoldingWeapon)
                            DestroyWeaponInSlot(3, RightArm);
                        else
                            PrimaryWpSlot[2] = true;
                        var newWeapon = Instantiate(DisplayedWeapon, RightArm.transform, false);
                        newWeapon.GetComponent<Weapon>().Slot = 3;
                    }
                    else if (Input.GetKeyDown(KeyCode.Y) && LeftArm != null)
                    {
                        GameObject oldWeapon = null;
                        if (LeftArm.transform.childCount > 0)
                        {
                            oldWeapon = LeftArm.transform.GetChild(0).gameObject;
                        }
                        var newWeapon = Instantiate(DisplayedWeapon, LeftArm.transform, false);
                        Destroy(oldWeapon);
                    }
                }
                else if(isMeleeWeapon && currSlot != 1 && currSlot != 2 && currSlot != 3)
                {
                    Debug.Log("PickupMelee");
                    if ((Input.GetKeyDown(KeyCode.Alpha4)) || (!isHoldingMelee))
                    {
                        if (isHoldingMelee)
                            DestroyWeaponInSlot(4, RightArm);
                        var newWeapon = Instantiate(DisplayedWeapon, RightArm.transform, false);
                        if(DisplayedWeapon.GetComponent<ShieldScript>() != null)
                        {
                            newWeapon.name = DisplayedWeapon.name;
                        }
                    }
                }
                //Debug.Log(PrimaryWpSlot[0] + " " + PrimaryWpSlot[1] + " " + PrimaryWpSlot[2]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerIsNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerIsNear = false;
        }
    }

    private void DestroyWeaponInSlot(float slot, GameObject RightArm)
    {
        foreach (Transform wp in RightArm.transform)
        {
        //Debug.Log("Destroy weapon");
            if (slot != 4)
            {
                if (wp.GetComponent<Weapon>() != null)
                    if (wp.GetComponent<Weapon>().Slot == slot)
                        Destroy(wp.gameObject);
            }
            else
            {
                if(wp.gameObject.activeSelf)
                    Destroy(wp.gameObject);
            }
        }
    }
}