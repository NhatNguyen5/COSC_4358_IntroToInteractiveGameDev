using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text fpsDisplay;
    private int updateText = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (updateText == 0)
        {
            float fps = 1 / Time.unscaledDeltaTime;
            fpsDisplay.text = "" + fps + " fps";
        }
        updateText = (updateText + 1) % 20;
    }
}
