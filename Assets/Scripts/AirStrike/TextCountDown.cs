using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextCountDown : MonoBehaviour
{
    public GameObject proj;
    public AirStrikeProjectile AirStrikeProj;
    public TextMeshProUGUI DistText;

    // Start is called before the first frame update
    void Start()
    {
        DistText = GetComponent<TextMeshProUGUI>();
        AirStrikeProj = proj.GetComponent<AirStrikeProjectile>();
    }

    // Update is called once per frame
    void Update()
    {

        DistText.text = AirStrikeProj.strikeTimer.ToString("#.00");
    }
}
