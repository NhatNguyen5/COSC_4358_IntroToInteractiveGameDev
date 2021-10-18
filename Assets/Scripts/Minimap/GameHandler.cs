using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minimap;
public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            MinimapWindow.Show();
        }

        if (Input.GetKeyDown(KeyCode.N)) {
            MinimapWindow.Hide();
        }
    }
}
