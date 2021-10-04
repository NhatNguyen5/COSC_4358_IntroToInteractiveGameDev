using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBossUI : MonoBehaviour
{

    public GameObject EnemyUI;
    private void OnTriggerEnter2D(Collider2D collision)
    {




        if (collision.tag == "Player")
        {
            EnemyUI.SetActive(true);

        }



    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EnemyUI.SetActive(false);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
