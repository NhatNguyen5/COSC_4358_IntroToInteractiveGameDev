using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public VideoPlayer video;

    private bool startUp = true;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(startUp)
        {
            if(video.isPlaying)
            {
                startUp = false;
            }
        }

        if (Input.anyKey || (!startUp && !video.isPlaying))
        {
            SceneManager.LoadScene(1);
        }
        //Debug.Log(video.isPlaying);
    }
}
