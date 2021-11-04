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

    public string CampaignLoadingScreenName;
    public string SurvivalLoadingScreenName;

    void Start()
    {
        ModeDropdown = GameObject.Find("GamemodeDropdown").GetComponent<Dropdown>();
        PlayButton = transform.GetComponent<Button>();
        PlayButton.onClick.AddListener(delegate { ChangeScene(); });
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
            sceneName = CampaignLoadingScreenName; ;
        }
        else if (ModeSelection == "SURVIVAL")
        {
            sceneName = SurvivalLoadingScreenName;
        }
        //PlayButton.onClick.RemoveListener(ChangeScene);
    }

    private void ChangeScene()
    {
        PlayButton.onClick.RemoveAllListeners();
        Debug.Log("listener deleted");
        SceneManager.LoadScene(sceneName);
    }
}
