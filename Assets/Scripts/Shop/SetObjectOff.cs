using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectOff : MonoBehaviour
{
    public GameObject[] listOfCols;



    public void TurnDemObjectsOffYo()
    {
        AudioManager.instance.PlayEffect("Button");
        listOfCols = transform.parent.gameObject.GetComponent<SetObjectOff>().listOfCols;
        foreach (GameObject go in listOfCols)
        {
            go.SetActive(false);
        }
    }

}
