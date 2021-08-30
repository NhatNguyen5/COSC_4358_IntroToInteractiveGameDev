using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    
    //public static bool GameisPaused = false;


    public GameObject OptionMenuUI;
    public GameObject PauseMenuUI;

    public void resume() 
    {
        OptionSettings.GameisPaused = false;
        OptionMenuUI.SetActive(false);
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        
    }

    void pause() 
    {
        //PauseOption.SetActive(true);
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        OptionSettings.GameisPaused = true;
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
