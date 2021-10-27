using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update

    private string ModeSelection;
    private Dropdown ModeDropdown;

    void Start()
    {
        ModeDropdown = GameObject.Find("GamemodeDropdown").GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        ModeSelection = ModeDropdown.options[ModeDropdown.value].text;

    }
}
