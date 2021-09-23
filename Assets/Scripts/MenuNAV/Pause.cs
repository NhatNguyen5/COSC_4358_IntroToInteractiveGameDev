using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    public CrossHairCursor crosshair;
    //public static bool GameisPaused = false;


    public GameObject OptionMenuUI;
    public GameObject PauseMenuUI;
    private GameObject PlayerIndicator;
    private GameObject StatusIndicator;


    private void Start()
    {
        PlayerIndicator = GameObject.Find("PlayerIndicator");
        StatusIndicator = GameObject.Find("StatusIndicator");
    }

    public void resume() 
    {
        Time.timeScale = 1f;
        OptionSettings.GameisPaused = false;
        OptionMenuUI.SetActive(false);
        PauseMenuUI.SetActive(false);
        PlayerIndicator.SetActive(true);
        StatusIndicator.SetActive(true);
        
    }

    void pause() 
    {
        //PauseOption.SetActive(true);
        PauseMenuUI.SetActive(true);
        OptionSettings.GameisPaused = true;
        PlayerIndicator.SetActive(false);
        StatusIndicator.SetActive(false);
        Time.timeScale = 0f;


    }




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionSettings.GameisPaused == true)
                resume();
            else
                pause();

        }


    }
}
