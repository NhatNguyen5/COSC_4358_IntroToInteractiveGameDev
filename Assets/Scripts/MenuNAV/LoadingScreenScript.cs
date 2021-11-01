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

    private float progress;

    // Start is called before the first frame update
    private void Start()
    {
        //StartCoroutine(LoadAsyncOperation());
        percentText = GameObject.Find("LoadingPercent").GetComponent<Text>();
        LoadLevel(SceneName);
    }

    private void FixedUpdate()
    {
        percentText.text = (ProgressBar.fillAmount * 100f).ToString() + "%";
    }
    /*
    private IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(SceneName);
        while (gameLevel.progress < 1)
        {
            ProgressBar.fillAmount = gameLevel.progress;

            yield return new WaitForEndOfFrame();

        }
    }
    */

    public void LoadLevel (string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone) {
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            ProgressBar.fillAmount = progress;
            //Debug.Log(operation.progress);
            yield return null;
        }
    }
}