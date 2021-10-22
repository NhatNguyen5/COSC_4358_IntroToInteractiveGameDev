using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ResetStats : MonoBehaviour
{
    private GlobalPlayerVariables resetting;
    private void Start()
    {
        resetting = GameObject.Find("GameManager").GetComponent<GlobalPlayerVariables>();
        if (SceneManager.GetActiveScene().name != "Title")
        {
            Debug.Log("RESET");
            resetting.resetStats();
        }
    }

}
