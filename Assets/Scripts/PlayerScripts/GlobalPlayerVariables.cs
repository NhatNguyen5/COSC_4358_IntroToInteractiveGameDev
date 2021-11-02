using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalPlayerVariables : MonoBehaviour
{
    public static GlobalPlayerVariables instance;

    public GameObject DefendMode;
    public GameObject AttackMode;

    public GameObject flagMarker;

    private ThymusScript ThymusController = null;
    /*
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
    */
    public static bool EnableAI = false;
    public static bool EnablePlayerControl = true;

    IEnumerator waitthis()
    {
        yield return new WaitForSeconds(5);
        EnablePlayerControl = true;
    }

    private float fadeInTime = 1;
    public RawImage FadeInImg;


    //SCORE
    public static float TotalScore = 0;

    //Hold xp backlog
    public static int expToDistribute;

    //base values to do calculations with
    public static float baseMaxHealth = 0;
    public static float baseHealthRegen = 0;
    public static float baseMaxStamina = 0;
    public static float baseStaminaRegen = 0;
    public static float baseWalkSpeed = 0;
    public static float baseSprintSpeed = 0;
    //public static float baseDashSpeed = 0; WONT TOUCH THIS FOR NOw
    public static float baseMaxAmmoReserve = 0;
    public static float baseAmmoReserveRegen = 0;
    public static float baseBulletCritRate = 0;
    public static float baseReloadSpeed = 0;

    public static float baseItemUsageCoolDown = 0;
    //ITEMS
    //public static float baseItemUsageCoolDownMolly = 0;
    public static float baseItemUsageCoolDownPhizer = 0;
    public static float baseItemUsageCoolDownTylenol = 0;







    //EXP SYSTEM AND VARIABLES TO CHANGE WHEN LEVELING UP
    public static float maxHealthBonus = 0;
    public static float healthRegenBonus = 0;
    public static float maxStaminaBonus = 0;
    public static float staminaRegenBonus = 0;
    public static float sprintWalkSpeedBonus = 0;
    public static float maxAmmoReserveBonus = 0;
    public static float ammoReserveRegenBonus = 0;
    public static float bulletCritRateBonus = 0;
    public static float reloadSpeedBonus = 1;




    //public static float itemUsageCoolDownBonusMolly = 1;
    public static float itemUsageCoolDownBonusPhizer = 1;
    public static float itemUsageCoolDownBonusTylenol = 1;




    public float rechargeRate = 1;

    public static float weaponWeight = 0;
    public static float BaseWeaponWeight = 1;

    public float bcr1 = 1;
    public float bcd1 = 1;
    public float bcr2 = 1;
    public float bcd2 = 1;


    //ammo reserves
    public float playerMaxReserves;
    public float chargeMultiplyer = 1;
    public static float rechargeRateMultiplyer = 1;
    public static float reserveCount = 0;


    public static float Reserves = 500;
    
    public static float MaxReserves = 1000;

    //public static float playerHealth = 100;
    //right weapon stats
    public static float BaseCritRate = 1;
    public static float BaseCritDamage = 1;
    public static float bulletSpeed = 0f;
    public static float bulletDamage = 0;
    public static float bulletKnockbackForce = 0;
    public static bool bulletPierce = false;
    public static bool bulletExplosion = false;


    public static int targetsToPierce = 0;
    public static float damageDropOff = 0;

    public static float critRate = 0; //RANGE FROM 0 TO 1
    public static float critDmg = 0;

    //damage drop off for bullets
    public static float bulletDamageDropOff = 0;
    public static float timeToDropDmg = 0;

    //left weapon stats
    public static float BaseCritRate2 = 1;
    public static float BaseCritDamage2 = 1;
    public static float bulletSpeed2 = 0f;
    public static float bulletDamage2 = 0;
    public static float bulletKnockbackForce2 = 0;
    public static bool bulletPierce2 = false;
    public static bool bulletExplosion2 = false;

    public static int targetsToPierce2 = 0;
    public static float damageDropOff2 = 0;

    public static float critRate2 = 0; //RANGE FROM 0 TO 1
    public static float critDmg2 = 0;


    //damage drop off for bullets
    public static float bulletDamageDropOff2 = 0;
    public static float timeToDropDmg2 = 0;


    public static bool GameOver = false;

    public static bool Defend = true;

    //stat tracking

    public static int GlobinsAndPlayerAlive = 0;
    public static int TotalEnemiesAlive = 0;
    public static int enemiesKilled = 0;


    private void Awake()
    {
        GlobinsAndPlayerAlive = 0;
        TotalEnemiesAlive = 0;
        enemiesKilled = 0;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Title")
        {
            Debug.Log("RESET");
            //resetObject = true;
            resetStats();
            resetObject = true;
        }
        StartCoroutine(FadeIn(1));
        if (GameObject.Find("Thymus") != null)
        {
            ThymusController = GameObject.Find("Thymus").GetComponent<ThymusScript>();
            StartCoroutine(ThymusAppear(2));
        }
        //StartCoroutine(waitthis());
    }

    public void resetStats()
    {
        //Debug.Log("Stats Reset");

        //Pause = false;
        OptionSettings.GameisPaused = false;
        GameOver = false;

        BaseCritRate = bcr1;
        BaseCritDamage = bcd1;
        BaseCritRate2 = bcr2;
        BaseCritDamage2 = bcd2;


        critDmg = BaseCritDamage;
        critRate = BaseCritRate;

        critDmg2 = BaseCritDamage2;
        critRate2 = BaseCritRate2;

        Reserves = playerMaxReserves;
        MaxReserves = playerMaxReserves;

        rechargeRateMultiplyer = chargeMultiplyer;





        TotalScore = 0;

        //BackLog Reset
        expToDistribute = 0;

        //Bonus Reset;
        maxHealthBonus = 0;
        healthRegenBonus = 0;
        maxStaminaBonus = 0;
        staminaRegenBonus = 0;
        sprintWalkSpeedBonus = 0;
        maxAmmoReserveBonus = 0;
        ammoReserveRegenBonus = 0;
        bulletCritRateBonus = 0;
        reloadSpeedBonus = 1;
        //itemUsageCoolDownBonus = 1;

        //LEVELING PURPOSES
        baseMaxAmmoReserve = playerMaxReserves;
        baseAmmoReserveRegen = chargeMultiplyer;
        baseBulletCritRate = BaseCritRate;
        baseReloadSpeed = 1;
        // baseItemUsageCoolDown = 1;

        baseItemUsageCoolDown = 0;


        //weapon weight

        weaponWeight = 0;


    }

    //globin positioning
    Vector3 newPosition = Vector3.zero;
    public static Transform newTransformCoords;
    float deactivateText = 2.5f;
    float countdown = 0;
    bool resetObject = false;
    public void Update()
    {
        /*
        if (SceneManager.GetActiveScene().name == "Title")
        {
            Debug.Log("TITLE");
            resetObject = true;
        }*/
        if(ThymusController != null)
            if(!ThymusController.Appear)
            {
                EnableAI = true;
                EnablePlayerControl = true;
            }

        if (SceneManager.GetActiveScene().name != "Title")
        {

            if (resetObject == true)
            {
                Debug.Log("RESETTING OBJECTS");
                DefendMode = GameObject.Find("Defend");
                AttackMode = GameObject.Find("Attack");
                if(DefendMode != null) 
                    DefendMode.SetActive(false);
                if (AttackMode != null)
                    AttackMode.SetActive(false);
                if (flagMarker != null)
                    flagMarker.SetActive(false);
                Defend = true;
                resetObject = false;
                countdown = 0;
            }

            //Debug.Log(Reserves + " " + MaxReserves);
            //recharge bar
            if (Reserves < MaxReserves)
            {
                Reserves += Time.deltaTime * rechargeRateMultiplyer;
            }

            
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
               

                if (Defend == true)
                {
                    Debug.Log("Follow Cursor");
                    Defend = false;
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPosition.z = 0f;
                    Debug.Log(mouseWorldPosition);
                    transform.position = mouseWorldPosition;
                    newTransformCoords = transform;
                    DefendMode.SetActive(false);
                    AttackMode.SetActive(true);
                    flagMarker.SetActive(true);
                    countdown = deactivateText;


                }
                else if (Defend == false)
                {
                    Debug.Log("Defend Player");
                    Defend = true;
                    DefendMode.SetActive(true);
                    AttackMode.SetActive(false);
                    flagMarker.SetActive(false);
                    countdown = deactivateText;
                }
                //Debug.Log("Mouse 2 ");
            }

            if (countdown <= 0)
            {
                DefendMode.SetActive(false);
                AttackMode.SetActive(false);
            }
            else
            {
                countdown -= Time.deltaTime;
            }

        }

        if (fadeInTime > 0)
            fadeInTime -= Time.deltaTime;
        if (fadeInTime < 0)
            fadeInTime = 0;
        Color temp = new Color(0, 0, 0, fadeInTime);
        FadeInImg.color = temp;


        if (GameOver == true)
        {
            GameObject.Find("DeathCurrentScore").GetComponent<Text>().text = "SCORE: " + TotalScore.ToString();

        }

    }

    private IEnumerator FadeIn(float Dur)
    {
        yield return new WaitForSeconds(Dur);
        EnableAI = true;
        EnablePlayerControl = true;
    }

    private IEnumerator ThymusAppear(float after)
    {
        yield return new WaitForSeconds(after);
        ThymusController.Appear = true;
        EnableAI = false;
        EnablePlayerControl = false;
    }


    public void switchGlobinMode()
    {

    }

}
