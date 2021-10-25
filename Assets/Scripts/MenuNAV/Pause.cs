using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private CursorManager.CursorType cursorType;


    public CrossHairCursor crosshair;
    //public static bool GameisPaused = false;


    public GameObject OptionMenuUI;
    public GameObject PauseMenuUI;
    public GameObject PlayerUI;
    //public GameObject StatusIndicator;


    public void Start()
    {
        CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Target);
        Debug.Log("Setting Cursor To Target START");
    }


    public void resume() 
    {
        Time.timeScale = 1f;
        PlayerUI.SetActive(true);
        OptionMenuUI.SetActive(false);
        PauseMenuUI.SetActive(false);
        OptionSettings.GameisPaused = false;

        CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Target);
        Debug.Log("Setting Cursor To Target RESUME");
    }

    public void button()
    {
        GameObject Temp = GameObject.Find("GameMusicandPause");
        Pause pauseButton = Temp.GetComponent<Pause>();
        pauseButton.resume();
    }


    void pause() 
    {
        //PauseOption.SetActive(true);
        PauseMenuUI.SetActive(true);
        PlayerUI.SetActive(false);
        OptionSettings.GameisPaused = true;
        Time.timeScale = 0f;

        CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Arrow);
        Debug.Log("Setting Cursor To ARROW PAUSE");
    }




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GlobalPlayerVariables.GameOver == false)
        {
            if (OptionSettings.GameisPaused == true)
                resume();
            else
                pause();

        }


    }
}
