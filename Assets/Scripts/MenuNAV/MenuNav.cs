using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNav : MonoBehaviour
{
    public GameObject HideCanvas;
    public GameObject ShowCanvas;

    public void HideAndShow()
    {
        HideCanvas.SetActive(false);
        ShowCanvas.SetActive(true);
    }

}
