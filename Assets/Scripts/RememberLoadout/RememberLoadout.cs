using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RememberLoadout : MonoBehaviour
{
    // Start is called before the first frame update
    public static RememberLoadout instance;
    public static bool loadPlayerStats = false;



    public static int totalExperienceEarned = 0;

    public GameObject[] PossiblePlayerWeapons;

    public string startingWeapon1;
    public string startingWeapon2;
    public string startingWeapon3;




    public string PrimaryWeapon;
    public string PrimaryWeaponDual;

    public string SecondaryWeapon;
    public string SecondaryWeaponDual;

    public string ThirdWeapon;
    public string ThirdWeaponDual;

    public GameObject RightArm;
    public GameObject LeftArm;



    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }



    void Start()
    {
        
        RightArm = GameObject.FindGameObjectWithTag("RightArm");
        LeftArm = GameObject.FindGameObjectWithTag("LeftArm");

        Transform rightArmTrans = RightArm.GetComponent<Transform>();
        Transform leftArmTrans = LeftArm.GetComponent<Transform>();




        foreach (GameObject go in PossiblePlayerWeapons)
        {
            Debug.Log("each");
            if (go.name == startingWeapon1)
            {
                Debug.Log("Primary");
                var newWeapon = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
                newWeapon.GetComponent<Weapon>().Slot = 1;
                newWeapon.name = startingWeapon1;
                newWeapon.transform.parent = RightArm.transform;
            }
            if (go.name == startingWeapon2)
            {
                var newWeapon2 = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
                newWeapon2.GetComponent<Weapon>().Slot = 2;
                newWeapon2.name = startingWeapon2;
                newWeapon2.transform.parent = RightArm.transform;
                newWeapon2.SetActive(false);
            }
            if (go.name == startingWeapon3)
            {
                var newWeapon3 = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
                newWeapon3.GetComponent<Weapon>().Slot = 3;
                newWeapon3.name = startingWeapon3;
                newWeapon3.transform.parent = RightArm.transform;
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





    }

    // Update is called once per frame
    void Update()
    {

        if (SceneManager.GetActiveScene().name == "Title" || GlobalPlayerVariables.GameOver == true)
        {
            Destroy(gameObject);
        }

        if (loadPlayerStats == true)
        {
            loadPlayerStats = false;
            GlobalPlayerVariables.expToDistribute += totalExperienceEarned;
            RightArm = GameObject.FindGameObjectWithTag("RightArm");
            LeftArm = GameObject.FindGameObjectWithTag("LeftArm");


            foreach (GameObject go in PossiblePlayerWeapons)
            {
                if (go.name == PrimaryWeapon)
                {
                    var newWeapon = Instantiate(go, new Vector3(0,0,0), Quaternion.identity);
                    newWeapon.GetComponent<Weapon>().Slot = 1;
                    newWeapon.name = PrimaryWeapon;
                    newWeapon.transform.parent = RightArm.transform;
                }
                if (go.name == SecondaryWeapon)
                {
                    var newWeapon2 = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
                    newWeapon2.GetComponent<Weapon>().Slot = 2;
                    newWeapon2.name = SecondaryWeapon;
                    newWeapon2.transform.parent = RightArm.transform;
                    newWeapon2.SetActive(false);
                }
                if (go.name == ThirdWeapon)
                {
                    var newWeapon3 = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
                    newWeapon3.GetComponent<Weapon>().Slot = 3;
                    newWeapon3.name = ThirdWeapon;
                    newWeapon3.transform.parent = RightArm.transform;
                    newWeapon3.SetActive(false);
                }


            }



        }
    }
}
