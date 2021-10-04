using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBossUI : MonoBehaviour
{

    public GameObject EnemyUI;

    public float DetectRange;

    private void Start()
    {
        transform.GetComponent<CircleCollider2D>().radius = DetectRange;
    }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
