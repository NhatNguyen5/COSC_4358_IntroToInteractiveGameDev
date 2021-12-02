using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public VideoPlayer video;
    public float FadeInAndOutSpeed;
    public float DisclaimerShowDuration;
    public float StartIntroAfter;
    [Range(0,1)]
    public float videoVolume;

    public GameObject DisclaimerScreen;

    private Image DisclaimerCover;

    private float coverTransparent = 1;

    private bool changeSceneFlag = false;

    private bool isWaiting = false;

    private bool DisclaimerShown = false;

    private bool doneFadeIn = false;

    private bool played = false;

    private void Start()
    {
        Color tempColor = new Color(0, 0, 0, 1);
        DisclaimerCover = DisclaimerScreen.transform.Find("DisclaimerCover").GetComponent<Image>();
        DisclaimerCover.color = tempColor;
    }

    private void Update()
    {
        if (!DisclaimerShown)
        {
            video.Pause();
            if (!doneFadeIn)
            {
                if (coverTransparent > 0)
                {
                    coverTransparent -= Time.deltaTime / (1/FadeInAndOutSpeed);
                }
                else
                {
                    StartCoroutine(wait(DisclaimerShowDuration));
                    doneFadeIn = true;
                }
            }

            if (doneFadeIn && !isWaiting)
            {
                //Debug.Log("HERE");
                if (coverTransparent < 1)
                {
                    coverTransparent += Time.deltaTime / FadeInAndOutSpeed;
                }
                else
                {
                    DisclaimerShown = true;
                    StartCoroutine(wait(StartIntroAfter));
                    Destroy(DisclaimerScreen);
                }
            }

            Color tempColor = new Color(0, 0, 0, coverTransparent);
            DisclaimerCover.color = tempColor;
            return;
        }




        if (!isWaiting && !video.isPlaying && !played)
        {
            video.Play();
            video.SetDirectAudioVolume(0, videoVolume);
            played = true;
        }

        if (!video.isPlaying && played)
            changeSceneFlag = true;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space) || changeSceneFlag)
        {
            SceneManager.LoadScene("Title");
        }
        //Debug.Log(video.isPlaying);
    }

    private IEnumerator wait(float dur)
    {
        isWaiting = true;
        yield return new WaitForSeconds(dur);
        isWaiting = false;
    }

}
