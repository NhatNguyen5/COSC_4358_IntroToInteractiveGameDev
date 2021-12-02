using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallRedunDantScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveChest()
    {
        GameObject.Find("PityMoneyChest").transform.position = new Vector2(-37.43f, 4.24f);
    }
}
