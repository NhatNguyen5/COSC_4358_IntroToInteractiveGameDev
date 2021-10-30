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


    public Text currentScoreDeath;
    public static int totalExperienceEarned = 0;
    public int showtotalexp = 0;

    public GameObject[] PossiblePlayerWeapons;
    public GameObject[] PossibleGlobins;
    public List<GameObject> OwnedWeapons;

    public string startingWeapon1;
    public string startingWeapon2;
    public string startingWeapon3;

    //public static int rememberScore = 0;

    public string PrimaryWeapon;
    public string PrimaryWeaponDual;

    public string SecondaryWeapon;
    public string SecondaryWeaponDual;

    public string ThirdWeapon;
    public string ThirdWeaponDual;

    public GameObject RightArm;
    public GameObject LeftArm;


    //Getting values at the end of level
    public GameObject player;
    public int numberOfheals = 0;
    public int numberOfPhizerz = 0;
    public float armorRemaining = 0;
    public int proteinCounter = 0;
    public int stemCellAmount = 0;
    public int numberOfStickyNades = 0;
    public int numberOfMollys = 0;
    public int numberOfGlobins = 0;

    public int Globin5Advisor = 0;
    public int Globin5Grenadier = 0;
    public int Globin5Operator = 0;
    public int Globin5Rocketeer = 0;
    public int Globin5Support = 0;


    public int SupportHelicopter = 0;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            totalExperienceEarned = 0;
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

        Transform rightArmTrans = RightArm.GetComponent<Transform>();
        Transform leftArmTrans = LeftArm.GetComponent<Transform>();

        foreach (GameObject go in PossiblePlayerWeapons)
        {
            //Debug.Log("each");
            if (go.name == startingWeapon1)
            {
                Debug.Log("Primary");
                var newWeapon = Instantiate(go, rightArmTrans, false);
                newWeapon.GetComponent<Weapon>().Slot = 1;
                newWeapon.name = startingWeapon1;
            }
        }
        foreach (GameObject go in PossiblePlayerWeapons)
        {
            if (go.name == startingWeapon2)
            {
                var newWeapon2 = Instantiate(go, rightArmTrans, false);
                newWeapon2.GetComponent<Weapon>().Slot = 2;
                newWeapon2.name = startingWeapon2;
                newWeapon2.SetActive(false);
            }
        }
        foreach (GameObject go in PossiblePlayerWeapons)
        {
            if (go.name == startingWeapon3)
            {
                var newWeapon3 = Instantiate(go, rightArmTrans, false);
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
            PrimaryWeapon = rightArmTrans.GetChild(0).name;
            SecondaryWeapon = rightArmTrans.GetChild(1).name;
            ThirdWeapon = rightArmTrans.GetChild(2).name;

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
            Destroy(gameObject);
        }

        RightArm = GameObject.FindGameObjectWithTag("RightArm");
        LeftArm = GameObject.FindGameObjectWithTag("LeftArm");

        string[] noLoadLoadoutScene =
        {
            "Title", "TutorialLoadingScreen", "Tutorial", "GameLoadingScreen"
        };

        if ((loadPlayerStats == true || player == null) && !noLoadLoadoutScene.Contains(SceneManager.GetActiveScene().name) && !OptionSettings.GameisPaused)
        {
            Debug.Log("Loading Loadout");
            loadPlayerStats = false;
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Player>().hideLevelUPAnimation = true;
            GlobalPlayerVariables.expToDistribute += totalExperienceEarned;
            RightArm = GameObject.FindGameObjectWithTag("RightArm");
            LeftArm = GameObject.FindGameObjectWithTag("LeftArm");

            Transform rightArmTrans = RightArm.GetComponent<Transform>();
            Transform leftArmTrans = LeftArm.GetComponent<Transform>();

            foreach (GameObject go in PossiblePlayerWeapons)
            {
                //Debug.Log("each");
                if (go.name == startingWeapon1)
                {
                    Debug.Log("Primary");
                    var newWeapon = Instantiate(go, rightArmTrans, false);
                    newWeapon.GetComponent<Weapon>().Slot = 1;
                    newWeapon.name = startingWeapon1;
                }
            }
            foreach (GameObject go in PossiblePlayerWeapons)
            {
                if (go.name == startingWeapon2)
                {
                    var newWeapon2 = Instantiate(go, rightArmTrans, false);
                    newWeapon2.GetComponent<Weapon>().Slot = 2;
                    newWeapon2.name = startingWeapon2;
                    newWeapon2.SetActive(false);
                }
            }
            foreach (GameObject go in PossiblePlayerWeapons)
            {
                if (go.name == startingWeapon3)
                {
                    var newWeapon3 = Instantiate(go, rightArmTrans, false);
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
                PrimaryWeapon = rightArmTrans.GetChild(0).name;
                SecondaryWeapon = rightArmTrans.GetChild(1).name;
                ThirdWeapon = rightArmTrans.GetChild(2).name;

                //var myNewSmoke = Instantiate(poisonSmoke, Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                //myNewSmoke.transform.parent = gameObject.transform;


            }

            
            player.GetComponent<Player>().SetPlayerItemsAndArmorValues();
        }
    }

}
