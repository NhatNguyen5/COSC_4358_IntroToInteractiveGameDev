using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchImage : MonoBehaviour
{
    public GameObject[] ImagesToDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void displayImage(string nameOfImage)
    {
        bool foundValue = false;
        foreach (GameObject go in ImagesToDisplay)
        {
            go.SetActive(false);
            //Debug.Log("each");
            if (go.name == nameOfImage)
            {
                foundValue = true;
                go.SetActive(true);
            }
        }

        if (foundValue == false)
        {
            ImagesToDisplay[0].SetActive(true);
        }

    }


}
