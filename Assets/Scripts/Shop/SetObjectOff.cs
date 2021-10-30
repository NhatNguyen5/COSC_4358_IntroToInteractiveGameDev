using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectOff : MonoBehaviour
{
    public GameObject[] listOfCols;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnDemObjectsOffYo()
    {
        listOfCols = transform.parent.gameObject.GetComponent<SetObjectOff>().listOfCols;
        foreach (GameObject go in listOfCols)
        {
            go.SetActive(false);
        }
    }

}
