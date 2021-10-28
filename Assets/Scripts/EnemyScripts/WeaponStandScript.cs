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
                foreach (Transform wp in RightArm.transform)
                {
                    if(wp.gameObject.activeSelf)
                    {
                        currSlot = wp.GetComponent<Weapon>().Slot;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Alpha1) && currSlot == 1)
                {
                    DestroyWeaponInSlot(1);
                    var newWeapon = Instantiate(DisplayedWeapon, RightArm.transform, false);
                    newWeapon.GetComponent<Weapon>().Slot = 1;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) && currSlot == 2)
                {
                    DestroyWeaponInSlot(2);
                    var newWeapon = Instantiate(DisplayedWeapon, RightArm.transform, false);
                    newWeapon.GetComponent<Weapon>().Slot = 2;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) && currSlot == 3)
                {
                    DestroyWeaponInSlot(3);
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
            Debug.Log("Destroy weapon");
            if (wp.GetComponent<Weapon>().Slot == slot)
                Destroy(wp.gameObject);
        }
    }
}
