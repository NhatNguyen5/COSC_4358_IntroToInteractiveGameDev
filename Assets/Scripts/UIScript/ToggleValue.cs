using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleValue : MonoBehaviour
{
    public Toggle fullScreenToggle;
    // Start is called before the first frame update
    void Start()
    {
        //fullscreentoggle = gameObject.GetComponent<Toggle>();
        if (Screen.fullScreen)
        {
            fullScreenToggle.isOn = true;
        }
        else
        {
            fullScreenToggle.isOn = false;
        }
    }

}
