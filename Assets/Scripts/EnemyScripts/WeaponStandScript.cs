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

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        transform.Find("WeaponSprite").GetComponent<SpriteRenderer>().sprite = DisplayedWeapon.transform.GetComponent<SpriteRenderer>().sprite;
        transform.Find("Canvas").transform.Find("WeaponName").GetComponent<Text>().text = DisplayedWeapon.transform.GetComponent<Weapon>().WeaponLabel;
        RightArm = GameObject.FindGameObjectWithTag("RightArm");
        LeftArm = GameObject.FindGameObjectWithTag("LeftArm");
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayerIsNear)
        {
            float currSlot = 0;
            if (OptionSettings.GameisPaused == false)
            {
                bool isHoldingWeapon = false;
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

                    if (wp.GetComponent<MeleeWeapon>() != null)
                    {
                        if (wp.gameObject.activeSelf)
                        {
                            currSlot = 4;
                            isHoldingWeapon = true;
                        }
                    }

                    Debug.Log(wp.gameObject.name + " " + currSlot);
                }

                if ((Input.GetKeyDown(KeyCode.Alpha1) && currSlot == 1) || (!isHoldingWeapon && !PrimaryWpSlot[0]))
                {
                    if (isHoldingWeapon)
                        DestroyWeaponInSlot(1);
                    else
                        PrimaryWpSlot[0] = true;
                    var newWeapon = Instantiate(DisplayedWeapon, RightArm.transform, false);
                    newWeapon.GetComponent<Weapon>().Slot = 1;
                }
                else if ((Input.GetKeyDown(KeyCode.Alpha2) && currSlot == 2) || (!isHoldingWeapon && !PrimaryWpSlot[1]))
                {
                    if (isHoldingWeapon)
                        DestroyWeaponInSlot(2);
                    else
                        PrimaryWpSlot[1] = true;
                    var newWeapon = Instantiate(DisplayedWeapon, RightArm.transform, false);
                    newWeapon.GetComponent<Weapon>().Slot = 2;
                }
                else if ((Input.GetKeyDown(KeyCode.Alpha3) && currSlot == 3) || (!isHoldingWeapon && !PrimaryWpSlot[2]))
                {
                    if (isHoldingWeapon)
                        DestroyWeaponInSlot(3);
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

    private void DestroyWeaponInSlot(float slot)
    {
        foreach(Transform wp in RightArm.transform)
        {
            //Debug.Log("Destroy weapon");
            if(wp.GetComponent<Weapon>() != null)
                if (wp.GetComponent<Weapon>().Slot == slot)
                    Destroy(wp.gameObject);
        }
    }
}
