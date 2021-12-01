using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public static bool CurrentlyInShop = false;
    public GameObject ShopManager;
    public GameObject shop;
    bool loadOnce = false;

    /*
    void loadStats()
    {
        if (loadOnce == false) 
        {
            loadOnce = true;
            ShopManager.GetComponent<ShopManager>().openShopAndUpdate();

        }
    }
    */

    void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("ENTER COLLIDER");
        if (other.CompareTag("Player") == true && GameObject.Find("Thymus") == null) {
            //loadStats();
            shop.SetActive(true);
            CurrentlyInShop = true;
            Debug.Log("IN SHOP");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("LEAVING COLLIDER");
        if (other.CompareTag("Player") == true)
        {
            Debug.Log("LEAVING SHOP");
            shop.SetActive(false);
            CurrentlyInShop = false;
        }
    }

    public void leaveShopOnTitle()
    {
        CurrentlyInShop = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        CurrentlyInShop = false;



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
