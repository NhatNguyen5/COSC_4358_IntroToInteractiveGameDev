using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RememberLoadout : MonoBehaviour
{
    // Start is called before the first frame update
    public static RememberLoadout instance;
    public bool loadPlayerStats = false;
    public static bool penalty = false;


    public Text currentScoreDeath;
    public static int totalExperienceEarned = 0;
    public static int holdEXPEachLevel = 0;
    public int showtotalexp = 0;

    public GameObject[] PossiblePlayerWeapons;
    public GameObject[] PossibleGlobins;
    public List<GameObject> OwnedWeapons;

    public string startingWeapon1;
    public string startingWeapon2;
    public string startingWeapon3;
    public string startingWeapon4;

    //public static int rememberScore = 0;

    public string PrimaryWeapon;
    public string PrimaryWeaponDual;

    public string SecondaryWeapon;
    public string SecondaryWeaponDual;

    public string ThirdWeapon;
    public string ThirdWeaponDual;

    public string ForthWeapon;
    public string ForthWeaponDual;

    public GameObject RightArm;
    public GameObject LeftArm;


    //Getting values at the end of level
    public GameObject player;
    public static int numberOfheals = 0;
    public static int numberOfPhizerz = 0;
    public static float armorRemaining = 0;
    public static int proteinCounter = 0;
    public static int stemCellAmount = 0;
    public static int numberOfStickyNades = 0;
    public static int numberOfMollys = 0;
    public static int numberOfGlobins = 0;

    public static int Globin5Advisor = 0;
    public static int Globin5Grenadier = 0;
    public static int Globin5Operator = 0;
    public static int Globin5Rocketeer = 0;
    public static int Globin5Support = 0;

    public static int ImmunalCarrierLight = 0;
    public static int ImmunalCarrierMedium = 0;
    public static int ImmunalCarrierHeavy = 0;


    public static int Bopterlift = 0;
    public static int Boptervac = 0;

    public static int HoldProtein = 0;

    public static string weap1 = "";
    public static string weap2 = "";
    public static string weap3 = "";
    public static string weap4 = "";


    public void setTein(int number)
    {
        HoldProtein = number;
    }


    private void ResetValues()
    {
        numberOfheals = 0;
        numberOfPhizerz = 0;
        armorRemaining = 0;
        proteinCounter = 0;
        stemCellAmount = 0;
        numberOfStickyNades = 0;
        numberOfMollys = 0;
        numberOfGlobins = 0;

        Globin5Advisor = 0;
        Globin5Grenadier = 0;
        Globin5Operator = 0;
        Globin5Rocketeer = 0;
        Globin5Support = 0;

        ImmunalCarrierLight = 0;
        ImmunalCarrierMedium = 0;
        ImmunalCarrierHeavy = 0;


        Bopterlift = 0;
        Boptervac = 0;

        HoldProtein = 0;

        weap1 = "";
        weap2 = "";
        weap3 = "";
        weap4 = "";

}


    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            if (penalty == true)
            {
                totalExperienceEarned = (int)(Mathf.Round((float)totalExperienceEarned * 0.80f));
                proteinCounter = HoldProtein;

                if (weap1 != "")
                {
                    startingWeapon1 = weap1;
                }
                if (weap2 != "")
                {
                    startingWeapon2 = weap2;
                }
                if (weap3 != "")
                {
                    startingWeapon3 = weap3;
                }
                if (weap4 != "")
                {
                    startingWeapon4 = weap4;
                }


            }
            else
            {
                totalExperienceEarned = 0;
                HoldProtein = 0;
                ResetValues();
            }
            penalty = true;
            DontDestroyOnLoad(gameObject);
        }
    }



    void Start()
    {
        //currentScoreDeath = GameObject.Find("DeathCurrentScore").GetComponent<Text>();

        player = null;

        /*
        RightArm = GameObject.FindGameObjectWithTag("RightArm");
        LeftArm = GameObject.FindGameObjectWithTag("LeftArm");

        Transform RightArm.transform = RightArm.GetComponent<Transform>();
        Transform leftArmTrans = LeftArm.GetComponent<Transform>();

        foreach (GameObject go in PossiblePlayerWeapons)
        {
            //Debug.Log("each");
            if (go.name == startingWeapon1)
            {
                Debug.Log("Primary");
                var newWeapon = Instantiate(go, RightArm.transform, false);
                newWeapon.GetComponent<Weapon>().Slot = 1;
                newWeapon.name = startingWeapon1;
            }
        }
        foreach (GameObject go in PossiblePlayerWeapons)
        {
            if (go.name == startingWeapon2)
            {
                var newWeapon2 = Instantiate(go, RightArm.transform, false);
                newWeapon2.GetComponent<Weapon>().Slot = 2;
                newWeapon2.name = startingWeapon2;
                newWeapon2.SetActive(false);
            }
        }
        foreach (GameObject go in PossiblePlayerWeapons)
        {
            if (go.name == startingWeapon3)
            {
                var newWeapon3 = Instantiate(go, RightArm.transform, false);
                newWeapon3.GetComponent<Weapon>().Slot = 3;
                newWeapon3.name = startingWeapon3;
                newWeapon3.SetActive(false);
            }
        }

        if (LeftArm != null)
        {
            LeftArm.SetActive(false);
        }

        if (loadPlayerStats == false)
        {
            PrimaryWeapon = RightArm.transform.GetChild(0).name;
            SecondaryWeapon = RightArm.transform.GetChild(1).name;
            ThirdWeapon = RightArm.transform.GetChild(2).name;

            //var myNewSmoke = Instantiate(poisonSmoke, Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            //myNewSmoke.transform.parent = gameObject.transform;


        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        //rememberScore = totalExperienceEarned;

        showtotalexp = totalExperienceEarned;

        if (SceneManager.GetActiveScene().name == "Title" || GlobalPlayerVariables.GameOver == true)
        {
            //if (GlobalPlayerVariables.GameOver == true)
            //{
            //    currentScoreDeath.GetComponent<Text>().text = "SCORE: " + totalExperienceEarned.ToString();
            //}
            GlobalPlayerVariables.TotalScore = showtotalexp;


            if (SceneManager.GetActiveScene().name == "Title")
            {
                holdEXPEachLevel = 0;
                penalty = false;
                //totalExperienceEarned = 0;
            }


            Destroy(gameObject);
        }

        //RightArm = GameObject.FindGameObjectWithTag("RightArm");
        //LeftArm = GameObject.FindGameObjectWithTag("LeftArm");

        string[] noLoadLoadoutScene =
        {
            "Title"//, "TutorialLoadingScreen", "Tutorial", "GameLoadingScreen"
        };

        if ((loadPlayerStats == true || player == null) && !noLoadLoadoutScene.Contains(SceneManager.GetActiveScene().name) && !OptionSettings.GameisPaused)
        {
            Debug.Log("Loading Loadout");
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                loadPlayerStats = false;
                player.GetComponent<Player>().hideLevelUPAnimation = true;
                GlobalPlayerVariables.expToDistribute += totalExperienceEarned;
                holdEXPEachLevel = totalExperienceEarned;
                RightArm = GameObject.FindGameObjectWithTag("RightArm");
                LeftArm = GameObject.FindGameObjectWithTag("LeftArm");

                foreach (GameObject go in PossiblePlayerWeapons)
                {
                    //Debug.Log("each");
                    if (go.name == startingWeapon1)
                    {
                        Debug.Log("Primary");
                        var newWeapon = Instantiate(go, RightArm.transform, false);
                        if(newWeapon.GetComponent<Weapon>() != null)
                            newWeapon.GetComponent<Weapon>().Slot = 1;
                        newWeapon.name = startingWeapon1;
                    }
                }
                foreach (GameObject go in PossiblePlayerWeapons)
                {
                    if (go.name == startingWeapon2)
                    {
                        var newWeapon2 = Instantiate(go, RightArm.transform, false);
                        if (newWeapon2.GetComponent<Weapon>() != null)
                            newWeapon2.GetComponent<Weapon>().Slot = 2;

                        newWeapon2.name = startingWeapon2;
                        newWeapon2.SetActive(false);
                    }
                }
                foreach (GameObject go in PossiblePlayerWeapons)
                {
                    if (go.name == startingWeapon3)
                    {
                        var newWeapon3 = Instantiate(go, RightArm.transform, false);
                        if (newWeapon3.GetComponent<Weapon>() != null)
                            newWeapon3.GetComponent<Weapon>().Slot = 3;

                        newWeapon3.name = startingWeapon3;
                        newWeapon3.SetActive(false);
                    }
                }
                foreach (GameObject go in PossiblePlayerWeapons)
                {
                    if (go.name == startingWeapon4)
                    {
                        var newWeapon4 = Instantiate(go, RightArm.transform, false);
                        if (newWeapon4.GetComponent<MeleeWeapon>() != null)
                            newWeapon4.GetComponent<MeleeWeapon>().Slot = 4;
                        else
                            newWeapon4.GetComponent<ShieldScript>().Slot = 4;
                        newWeapon4.name = startingWeapon4;
                        newWeapon4.SetActive(false);
                    }
                }

                if (LeftArm != null)
                {
                    LeftArm.SetActive(false);
                }


                if (loadPlayerStats == false)
                {
                    PrimaryWeapon = startingWeapon1;
                    SecondaryWeapon = startingWeapon2;
                    ThirdWeapon = startingWeapon3;
                    ForthWeapon = startingWeapon4;
                    //var myNewSmoke = Instantiate(poisonSmoke, Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                    //myNewSmoke.transform.parent = gameObject.transform;


                }


                player.GetComponent<Player>().SetPlayerItemsAndArmorValues();
            }
        }
    }

}
