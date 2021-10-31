using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenScript : MonoBehaviour
{
    public Image ProgressBar;
    public string SceneName;

    public Text percentText;
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(LoadAsyncOperation());

        percentText = GameObject.Find("Loading Percent").GetComponent<Text>();
    }


    private void FixedUpdate()
    {
        percentText.text = (ProgressBar.fillAmount*100).ToString() + "%";
    }

    private IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(SceneName);
        while (gameLevel.progress < 1)
        {
            ProgressBar.fillAmount = gameLevel.progress;
            
            yield return new WaitForEndOfFrame();
        }
    }
}
