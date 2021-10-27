using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButtonScript : MonoBehaviour
{
    // Start is called before the first frame update

    private string ModeSelection;
    private Dropdown ModeDropdown;
    private Button PlayButton;
    private string sceneName;

    void Start()
    {
        ModeDropdown = GameObject.Find("GamemodeDropdown").GetComponent<Dropdown>();
        PlayButton = transform.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        ModeSelection = ModeDropdown.options[ModeDropdown.value].text;
        if(ModeSelection == "TUTORIAL")
        {
            sceneName = "TutorialLoadingScreen";
        }
        else if(ModeSelection == "CAMPAIGN")
        {
            sceneName = "GameLoadingScreen";
        }

        PlayButton.onClick.AddListener(delegate { ChangeScene(); });

        //PlayButton.onClick.RemoveListener(ChangeScene);
    }

    private void ChangeScene()
    {
        StartCoroutine(changeScene());
        SceneManager.LoadScene(sceneName);
    }
    
    private IEnumerator changeScene()
    {
        yield return new WaitForSeconds(0.5f);
        PlayButton.onClick.RemoveAllListeners();
        Debug.Log("listener deleted");
        //SceneManager.LoadScene(sceneName);
    }
}
