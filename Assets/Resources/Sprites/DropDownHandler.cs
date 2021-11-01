using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownHandler : MonoBehaviour
{
    public Dropdown drop;

    [Header("Gamemode Covers")]
    public GameObject TutorialCover;
    public GameObject CampaingCover;
    public GameObject SurvivalCover;

    private void Start()
    {


        drop.onValueChanged.AddListener(delegate
        {

            dropValueChangedHappened(drop);
        });
        
    }

    public void dropValueChangedHappened(Dropdown sender){
        //Debug.Log("selected" + sender.value);

        switch (sender.value) {
            case 0:
                TutorialCover.SetActive(true);
                CampaingCover.SetActive(false);
                SurvivalCover.SetActive(false);
                break;
            case 1:
                CampaingCover.SetActive(true);
                TutorialCover.SetActive(false);
                SurvivalCover.SetActive(false);
                break;
            case 2:
                SurvivalCover.SetActive(true);
                TutorialCover.SetActive(false);
                CampaingCover.SetActive(false);
                break;
        }
    }

}
