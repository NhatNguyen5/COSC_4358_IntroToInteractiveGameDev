using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    public Text score;
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text>();
    }

    private void OnEnable()
    {
        score = GetComponent<Text>();
        score.text = "SCORE: " + RememberLoadout.totalExperienceEarned.ToString();
    }
}
