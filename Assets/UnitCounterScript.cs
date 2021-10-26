using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCounterScript : MonoBehaviour
{
    GameObject[] enemies;
    GameObject[] enemiesMelee;
    GameObject[] allies;
    public Text allyCountText;
    public Text enemyCountText;

    private Image antibodyBar;

    private float enemyLength;
    private float allyLength;

    private float totalUnits;
    //public Image pathogenBar;

    private float antibodyRatio;

    private void Start()
    {
        antibodyBar = GameObject.Find("AntibodyAmountBar").GetComponent<Image>();
        //pathogenBar = GameObject.Find("PathogenAmountBar").GetComponent<Image>();

    }
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesMelee = GameObject.FindGameObjectsWithTag("EnemyMelee");
        allies = GameObject.FindGameObjectsWithTag("Globin");

       
        enemyLength = float.Parse((enemies.Length + enemiesMelee.Length).ToString());
        allyLength = float.Parse(allies.Length.ToString());

        enemyCountText.text = enemyLength.ToString();
        allyCountText.text = allyLength.ToString();

        totalUnits = allyLength + enemyLength;

        antibodyBar.fillAmount = allyLength / totalUnits;
    }
}
