using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerStats stats;

    [SerializeField]
    private PlayerComponents components;

    [SerializeField]
    private PlayerReferences references;

    private PlayerUtilities utilities;

    private PlayerActions actions;
    public PlayerComponents Components { get => components; }
    public PlayerStats Stats { get => stats; }
    public PlayerReferences References { get => references; }

    private Image StaminaBar;

    private bool isRunning;

    private float sprint = 0;

    private float currDashCooldown = 0;

    private float DashDuration;

    private float currDashDuration = 0;

    private float TrailDur;

    private float currTrailDur = 0;

    private bool dashing;

    private Vector2 dashDir;

    private Text ProteinCounts;

    public EnemyManager enemyManager;

    private GameObject EnemySpawnRate;

    private float MinTbs;

    private float MaxTbs;

    private float defaultTbs;

    private float spawnRate;
    private float defaultSR;

    [HideInInspector]
    public bool enableControl = true;

    private bool phaseOverWrite = false;

    private bool resetPlayerStatsRequest = false;

    private bool inEffect = false;

    private bool LeftSlotAvailableToUse = true;

    private bool RightSlotAvailableToUse = true;

    private float LeftSlotCooldownDisplay;

    private float RightSlotCooldownDisplay;

    private int currArmor;

    [SerializeField]
    private ArmorUp ArmorUpEff;
    [SerializeField]
    private ArmorDown ArmorDownEff;
    [SerializeField]
    private LevelUpEff levelUpEff;
    [SerializeField]
    private GameObject levelUpPopUp;
    [SerializeField]
    private CameraFollow CamFollow;

    //private float currSpeed;

    //private bool setSpeedBack = false;
    [SerializeField]
    private GameObject beat;

    //private GameObject gendBeat;

    private float beatTimer = 0;

    [SerializeField]
    private Molotov Molly;
    [SerializeField]
    private StickyGrenade Gre;

    public string dashSound;

    //LEVELCAP
    [Header("Level Stats")]
    public GameObject levelText1;
    public GameObject levelText10;
    public GameObject levelText100;
    public Image expBar;

    private Text Text1;
    private Text Text2;
    private Text Text3;

    public int baseLevelThreshHold = 200;
    //public int hardLevelCap = 120;
    public float levelThreshhold = 100;
    [HideInInspector]
    public float Currentlevel = 0;
    private bool PhizerIsActive = false;


    //growth rates
    
    public int levelCapGrowthRate = 200;
    
    public int healthGrowthRate = 8;
    
    public float hpRegenGrowthRate = 0.19f;
    
    public float maxStaminaGrowthRate = 1f;
    
    public float staminaRegenGrowthRate = 0.2f;
   
    public float walkSpeedGrowthRate = 1f;
    [HideInInspector]
    public float holdWalkSpeed = 0f;

    
    public float sprintSpeedGrowthRate = 1.5f;
    [HideInInspector]
    public float holdSprintSpeed = 0f;

    
    public float maxAmmoReserveGrowthRate = 10f;

    
    public float ammoReserveRegen = 0.5f;

    
    public float critRateGrowthRate = 0.01f;

    public float reloadSpeedGrowthRate = 0.01f;

    //private int hardcap = 0; //leaving this here for now

    public float itemUsageGrowthRate = 0.0035f;

    private void Awake()
    {

        /*
        public static float baseMaxHealth = 0; done
        public static float baseHealthRegen = 0; done
        public static float baseMaxStamina = 0; done
        public static float baseStaminaRegen = 0; done
        public static float baseSprintWalkSpeed = 0; done
        public static float baseMaxAmmoReserve = 0; done
        public static float baseAmmoReserveRegen = 0; done
        public static float baseBulletCritRate = 0; done
        public static float baseReloadSpeed = 0; done
        
        public static float baseItemUsageCoolDownPhizer = 0; 
        public static float baseItemUsageCoolDownTylenol = 0; 
        */




        Text1 = levelText1.GetComponent<Text>();
        Text2 = levelText10.GetComponent<Text>();
        Text3 = levelText100.GetComponent<Text>();
        Text1.text = "0";
        Text2.text = "0";
        Text3.text = "0";



        levelThreshhold = baseLevelThreshHold;
        actions = new PlayerActions(this);
        utilities = new PlayerUtilities(this);
        references = new PlayerReferences(this);
        stats.PFrictionz = stats.pfriction;
        stats.Armorz = stats.armor;
        stats.ArmorPerArmorLevelz = stats.armorperarmorlevel;
        stats.DamageReducePerArmorLevelz = stats.damagereduceperarmorlevel;
        //EXP
        stats.Experience = stats.EXP;

        stats.Health = stats.hp;

        stats.MaxHealth = stats.maxhp;
        GlobalPlayerVariables.baseMaxHealth = stats.maxhp;

        stats.HPRegen = stats.hpregenrate;
        GlobalPlayerVariables.baseHealthRegen = stats.hpregenrate;

        stats.Speed = stats.WalkSpeed;
        holdWalkSpeed = stats.WalkSpeed;
        holdSprintSpeed = stats.SprintSpeed;
        GlobalPlayerVariables.baseWalkSpeed = stats.WalkSpeed;
        GlobalPlayerVariables.baseSprintSpeed = stats.SprintSpeed;
        //GlobalPlayerVariables.baseDashSpeed = stats.DashSpeed; WONT TOUCH THIS FOR NOW

        stats.Stamina = stats.stamina;
        stats.MaxStamina = stats.maxplayerstamina;
        GlobalPlayerVariables.baseMaxStamina = stats.maxplayerstamina;
        stats.StamDrainRate = stats.stamdrainrate;
        stats.TimeBeforeStamRegen = stats.StaminaRegenWait;
        stats.StaminaRegenRate = stats.staminaRegenRate;
        GlobalPlayerVariables.baseStaminaRegen = stats.staminaRegenRate;
        stats.NumofHeal = stats.numofheal;
        stats.NumofProtein = stats.numofprotein;
        stats.NumofPhizer = stats.numofphizer;
        stats.NumofMolly = stats.numofmolly;

        stats.PhizerDurationz = stats.PhizerDuration;
        stats.PhizerCooldownz = stats.PhizerCooldown;
        GlobalPlayerVariables.baseItemUsageCoolDownPhizer = stats.PhizerCooldown;


        stats.HPRegenAddz = stats.HPRegenAdd;
        stats.StamRegenAddz = stats.StamRegenAdd;

        stats.TylenoCooldownz = stats.TylenolCooldown;
        GlobalPlayerVariables.baseItemUsageCoolDownTylenol = stats.TylenolCooldown;
        stats.TylenolHealAmountz = stats.TylenolHealAmount;

        actions.defaultSpeed = stats.Speed;
        actions.HealCounts.text = stats.NumofHeal.ToString();
        actions.VaccineCounts.text = stats.NumofPhizer.ToString();
        DashDuration = stats.DashDistance / stats.DashSpeed;
        TrailDur = DashDuration;
        ProteinCounts = GameObject.Find("ProteinCounts").GetComponent<Text>();
        EnemySpawnRate = GameObject.Find("EnemySpawnRateDisplay");
        MinTbs = enemyManager.MinTbs;
        MaxTbs = enemyManager.MaxTbs;
        defaultTbs = enemyManager.timeBetweenSpawns;
        defaultSR = 1/enemyManager.timeBetweenSpawns;

        stats.ArmorLevel = Mathf.FloorToInt(stats.Armorz / stats.ArmorPerArmorLevelz);

        components.PlayerParticleSystem.Stop();

        currArmor = stats.ArmorLevel;
    }


    // Start is called before the first frame update
    private void Start()
    {
        //levelUP();
        StaminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();
    }


    void levelUP()
    {
        stats.Experience -= levelThreshhold;
        Currentlevel += 1f;
        //Debug.Log("LEVEL UP, Experience: " + stats.Experience + " Current level: " + Currentlevel);
        //LEVELING FUNCTION
        //levelThreshhold = (float)hardLevelCap / (1 + Mathf.Pow(1.5f, (11f - 0.9f * Currentlevel)));
        levelThreshhold = levelCapGrowthRate * Currentlevel + baseLevelThreshHold;


        //HealthFunction
        if(PhizerIsActive == false)
            stats.MaxHealth = healthGrowthRate * Currentlevel + GlobalPlayerVariables.baseMaxHealth;
        else if(PhizerIsActive == true)
            Stats.MaxHealth += healthGrowthRate;
        if (stats.Health + healthGrowthRate > stats.MaxHealth)
            stats.Health = stats.MaxHealth;
        else
            stats.Health += healthGrowthRate;

        //HealthRegen Function
        if (PhizerIsActive == false)
        {
            //Debug.Log(hpRegenGrowthRate);
            stats.HPRegen = hpRegenGrowthRate * Currentlevel + GlobalPlayerVariables.baseHealthRegen;
            //Debug.Log(stats.HPRegen + " growthrate " + hpRegenGrowthRate + " curr level " + Currentlevel);
        }
        else if (PhizerIsActive == true)
        {
            Stats.HPRegen += hpRegenGrowthRate;
            //Debug.Log(stats.HPRegen + " growthrate " + hpRegenGrowthRate + " curr level " + Currentlevel);
        }

        //MaxStamina function
        if (PhizerIsActive == false)
            stats.MaxStamina = maxStaminaGrowthRate * Currentlevel + GlobalPlayerVariables.baseMaxStamina;
        else if (PhizerIsActive == true)
            stats.MaxStamina += maxStaminaGrowthRate;
        if (stats.Stamina + maxStaminaGrowthRate > stats.MaxStamina)
            stats.Stamina = stats.MaxStamina;
        else
            stats.MaxStamina += maxStaminaGrowthRate;


        //StaminaRegen function
        if (PhizerIsActive == false)
            stats.StaminaRegenRate = staminaRegenGrowthRate * Currentlevel + GlobalPlayerVariables.baseStaminaRegen;
        else if (PhizerIsActive == true)
            stats.StaminaRegenRate += staminaRegenGrowthRate;

        //Speed functions
        holdWalkSpeed = walkSpeedGrowthRate * Currentlevel + GlobalPlayerVariables.baseWalkSpeed;
        holdSprintSpeed = sprintSpeedGrowthRate * Currentlevel + GlobalPlayerVariables.baseSprintSpeed;

        //ammoreserve function
        GlobalPlayerVariables.MaxReserves = maxAmmoReserveGrowthRate * Currentlevel + GlobalPlayerVariables.baseMaxAmmoReserve;

        //ammoreserveregen function
        GlobalPlayerVariables.rechargeRateMultiplyer = ammoReserveRegen * Currentlevel + GlobalPlayerVariables.baseAmmoReserveRegen;



        //bulletcritrate function
        GlobalPlayerVariables.BaseCritRate = critRateGrowthRate * Currentlevel + GlobalPlayerVariables.baseBulletCritRate;
        GlobalPlayerVariables.BaseCritRate2 = critRateGrowthRate * Currentlevel + GlobalPlayerVariables.baseBulletCritRate;

        //reload speed

        GlobalPlayerVariables.reloadSpeedBonus = reloadSpeedGrowthRate * Currentlevel + GlobalPlayerVariables.baseReloadSpeed;

        //item speed function

        GlobalPlayerVariables.baseItemUsageCoolDown = itemUsageGrowthRate * Currentlevel;





        //show level
        //int calc = ((int)Currentlevel % 100);
        //Debug.Log(Currentlevel);

        Text3.text = (((int)Currentlevel / 100) % 10).ToString();
        Text2.text = (((int)Currentlevel / 10) % 10).ToString();
        Text1.text = ((int)Currentlevel % 10).ToString();


        //Text3.text = temp.ToString();


       // temp = ((int)Currentlevel / 10)%10;
        //Text2.text = temp.ToString();
       
        //Text1.text = temp.ToString();

        /*
    if (stats.HPRegen + hpRegenGrowthRate > stats.MaxHealth)
        stats.Health = stats.MaxHealth;
    else
        stats.Health += healthGrowthRate;
    */


        expBar.fillAmount = stats.Experience / levelThreshhold;

    }


    // Update is called once per frame
    private void Update()
    {
        //Debug.Log("itemusagecooldown " + GlobalPlayerVariables.baseItemUsageCoolDown + " curr level " + Currentlevel);
        //Debug.Log("reload speed multiplier " + GlobalPlayerVariables.reloadSpeedBonus + " curr level " + Currentlevel);
        //Debug.Log("critrate1 " + GlobalPlayerVariables.BaseCritRate + " bcr2 " + GlobalPlayerVariables.BaseCritRate2 + " curr level " + Currentlevel);
        //Debug.Log("Max reserves " + GlobalPlayerVariables.MaxReserves + " recharge rate " + GlobalPlayerVariables.rechargeRateMultiplyer + " curr level " + Currentlevel);
        //Debug.Log("walk speed " + holdWalkSpeed + " sprint speed " + holdSprintSpeed + " curr level " + Currentlevel);
        //Debug.Log(stats.StaminaRegenRate);
        //Debug.Log(stats.HPRegen);
        if (GlobalPlayerVariables.expToDistribute > 0)
        {
            stats.Experience += GlobalPlayerVariables.expToDistribute;
            GlobalPlayerVariables.expToDistribute = 0;
            expBar.fillAmount = stats.Experience / levelThreshhold;
        }
        if (stats.Experience >= levelThreshhold)
        {
            
            Instantiate(levelUpEff, stats.Position, Quaternion.identity);
            GameObject levelPopUpTemp;
            levelPopUpTemp = Instantiate(levelUpPopUp, transform, false);
            levelPopUpTemp.GetComponent<TextMeshPro>().text = ("LEVEL " + (Currentlevel + 1));
            Destroy(levelPopUpTemp, 3.45f);

            components.PlayerStatusIndicator.StartFlash(0.25f, 0.25f, Color.yellow, ((stats.MaxHealth - stats.Health) / stats.MaxHealth) / 2f, Color.red, 1);
            levelUP();
        }
        utilities.HandleInput();
        references.CalMousePosToPlayer();
        actions.UpdateCountsUI();
        actions.Regen();
        stats.ArmorLevel = Mathf.CeilToInt(stats.Armorz / stats.ArmorPerArmorLevelz);

        if (Input.GetKey(KeyCode.LeftShift) && stats.Direction != Vector2.zero)
        {
            if(stats.Stamina > 0)
                stats.Stamina -= Time.deltaTime*(stats.StamDrainRate - stats.StaminaRegenRate);
            StaminaBar.fillAmount = stats.Stamina / (maxStaminaGrowthRate * Currentlevel + GlobalPlayerVariables.baseMaxStamina); //stats.Stamina / stats.MaxStamina;
            isRunning = true;
            sprint = 0;
        }


        if (stats.Stamina <= stats.MaxStamina && isRunning == false && sprint >= stats.TimeBeforeStamRegen)
        { 
            stats.Stamina += Time.deltaTime * stats.StaminaRegenRate;
            StaminaBar.fillAmount = stats.Stamina / (maxStaminaGrowthRate * Currentlevel + GlobalPlayerVariables.baseMaxStamina); //stats.Stamina / stats.MaxStamina;
        }
        isRunning = false;
        sprint += Time.deltaTime;

        if (OptionSettings.GameisPaused == false)
        {
            if(Input.GetKeyUp(KeyCode.T))
                actions.ToggleDual();
            if (Input.GetKeyUp(KeyCode.E) && RightSlotAvailableToUse)
            {
                if (stats.NumofHeal > 0 && stats.Health < stats.MaxHealth)
                {
                    actions.Heal();
                    RightSlotCooldownDisplay = stats.TylenolCooldown * (1 - GlobalPlayerVariables.baseItemUsageCoolDown);
                    StartCoroutine(RightSlotItemCooldown(stats.TylenolCooldown * (1 - GlobalPlayerVariables.baseItemUsageCoolDown)));
                }
            }
            if(Input.GetKeyUp(KeyCode.G))
            {
                /*
                if (stats.NumofMolly > 0)
                {
                    Quaternion newRot = Quaternion.Euler(stats.Direction.x, stats.Direction.y, 0);
                    Instantiate(Molly, stats.Position, newRot);
                    stats.NumofMolly -= 1;
                }
                */
                {
                    Quaternion newRot = Quaternion.Euler(stats.Direction.x, stats.Direction.y, 0);
                    Instantiate(Gre, stats.Position, newRot);
                }
            }
            if (Input.GetKeyUp(KeyCode.Q) && LeftSlotAvailableToUse && PhizerIsActive == false)
            {
                if (stats.NumofPhizer > 0)
                {
                    actions.Phizer();
                    LeftSlotCooldownDisplay = stats.PhizerCooldown * (1 - GlobalPlayerVariables.baseItemUsageCoolDown);
                    StartCoroutine(ResetStats(stats.PhizerDuration));
                    StartCoroutine(LeftSlotItemCooldown(stats.PhizerCooldown * (1 - GlobalPlayerVariables.baseItemUsageCoolDown)));
                }
            }
            if(Input.GetKey(KeyCode.M))
            {
                
                Debug.Log("Map");
            }
            actions.SwapWeapon();
            //Debug.DrawRay(stats.Position, stats.Direction, Color.green, 0.1f);
            DashProc();
        }
        //Debug.Log(VaccineCooldownDisplay);
        if(resetPlayerStatsRequest && !inEffect)
        {
            //Debug.Log("RequestReset");
            actions.ResetPlayerStats();
            resetPlayerStatsRequest = false;
        }
        //Debug.Log(stats.StaminaRegenRate);
        if(LeftSlotCooldownDisplay > 0 || RightSlotCooldownDisplay > 0)
        {
            LeftSlotCooldownDisplay -= Time.deltaTime;
            actions.LeftSlotCooldownDisplayUpdate(LeftSlotCooldownDisplay/stats.PhizerCooldown);
        }
        else
        {
            LeftSlotCooldownDisplay = 0;
        }

        if (RightSlotCooldownDisplay > 0 || RightSlotCooldownDisplay > 0)
        {
            RightSlotCooldownDisplay -= Time.deltaTime;
            actions.RightSlotCooldownDisplayUpdate(RightSlotCooldownDisplay / stats.TylenolCooldown); ;
        }
        else
        {
            RightSlotCooldownDisplay = 0;
        }

        UpdateSpawnrate();
        ArmorEffect();
    }

    private void FixedUpdate()
    {
        
        if(enableControl)
        {
            actions.Move(transform);
            if (Input.GetKey(KeyCode.LeftShift) && stats.Stamina > 0)
                actions.Sprint();
            else
                actions.Walk();
        }
        
        actions.Animate();
        ProteinCounts.text = stats.NumofProtein.ToString();
    }

    private void UpdateSpawnrate()
    {
        EnemySpawnRate.transform.Find("HeartMonitorBG")
            .transform.Find("HeartMonitorLine").GetComponent<RawImage>()
            .color = new Color(0.75f*((MaxTbs - enemyManager.timeBetweenSpawns) / (MaxTbs - MinTbs)),
                               0.75f * ((enemyManager.timeBetweenSpawns - MinTbs) / (MaxTbs - MinTbs)),
                               0.75f * (1 - Mathf.Abs(2 * ((enemyManager.timeBetweenSpawns - MinTbs) / (MaxTbs - MinTbs)) - 1)));

        if (enemyManager.timeBetweenSpawns == 1000000)
        {
            spawnRate = defaultSR;
        }
        else
        {
            spawnRate = 1 / enemyManager.timeBetweenSpawns;
        }

        if (beatTimer > 0)
        {
            beatTimer -= Time.deltaTime;
        }
        else
        {
            beatTimer = 0;
        }

        //Debug.Log(beatTimer);

        if (beatTimer == 0)
        {
            beatTimer = 0.75f / (spawnRate / defaultSR);
            //for(int i = 0; i < 2; i++)
            StartCoroutine(beatGen(0.75f));
            //StartCoroutine(beatGen(0.75f / spawnRate));

        }
    }

    private void DashProc()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currDashCooldown == 0 && stats.Stamina >= stats.DashStamCost && stats.Direction.magnitude != 0 && !phaseOverWrite)
        {
            //Debug.Log("Dash");

            FindObjectOfType<AudioManager>().PlayEffect(dashSound);
            components.PlayerTrailRenderer.enabled = true;
            dashDir = stats.Direction;
            currTrailDur = TrailDur;
            currDashCooldown = stats.DashCoolDown;
            stats.Stamina -= stats.DashStamCost;
            currDashDuration = DashDuration;
            dashing = true;
        }

        if (dashing)
        {
            gameObject.layer = LayerMask.NameToLayer("Phase");
            components.PlayerTrailRenderer.time = currTrailDur;
            actions.Dash(dashDir);
        }

        if (currDashCooldown > 0)
            currDashCooldown -= Time.deltaTime;
        else if (currDashCooldown < 0)
            currDashCooldown = 0;

        if (currDashDuration > 0)
            currDashDuration -= Time.deltaTime;
        else if (currDashDuration < 0)
            currDashDuration = 0;
        else if(currDashDuration == 0 && !phaseOverWrite)
        {
            gameObject.layer = LayerMask.NameToLayer("Actor");
            dashing = false;
        }

        if (currTrailDur > 0)
            currTrailDur -= Time.deltaTime;
        else if (currTrailDur < 0)
            currTrailDur = 0;
        else
            components.PlayerTrailRenderer.enabled = false;
    }

    public void ArmorEffect()
    {
        if (currArmor != stats.ArmorLevel)
        {
            if (currArmor < stats.ArmorLevel)
            {
                Instantiate(ArmorUpEff, stats.Position, Quaternion.identity);
            }
            else if(currArmor > stats.ArmorLevel)
            {
                Instantiate(ArmorDownEff, stats.Position, Quaternion.identity);
            }

            currArmor = stats.ArmorLevel;
        }
    }

    private IEnumerator beatGen(float duration)
    {
        GameObject gendBeat;
        Transform HMBG = EnemySpawnRate.transform.Find("HeartMonitorBG").transform.Find("HeartMonitorLine");
        gendBeat = Instantiate(beat, HMBG, false);
        //gendBeat.transform.localScale = HMBG.localScale;
        gendBeat.GetComponent<RawImage>().color = new Color(0.75f * ((MaxTbs - enemyManager.timeBetweenSpawns) / (MaxTbs - MinTbs)),
                                                            0.75f * ((enemyManager.timeBetweenSpawns - MinTbs) / (MaxTbs - MinTbs)),
                                                            0.75f * (1 - Mathf.Abs(2*((enemyManager.timeBetweenSpawns - MinTbs) / (MaxTbs - MinTbs)) - 1)));
        //Debug.Log(gendBeat.GetComponent<RawImage>().color);
        //gendBeat.GetComponent<Animator>().SetFloat("BeatRate", spawnRate);
        yield return new WaitForSeconds(duration);
        Destroy(gendBeat);
    }

    public IEnumerator Phasing(float duration)
    {
        
        phaseOverWrite = true;
        gameObject.layer = LayerMask.NameToLayer("Phase");

        yield return new WaitForSeconds(duration);
        phaseOverWrite = false;
        gameObject.layer = LayerMask.NameToLayer("Actor");
    }

    public IEnumerator TakeOver(float duration) //doesn't work!!
    {

        enableControl = false;

        yield return new WaitForSeconds(duration);

        phaseOverWrite = true;
    }

    private IEnumerator ResetStats(float AfterSeconds)
    {
        inEffect = true;
        resetPlayerStatsRequest = true;
        PhizerIsActive = true; //might have to change this later to make it more general
        yield return new WaitForSeconds(AfterSeconds);
        PhizerIsActive = false; //same with this one
        inEffect = false;
    }

    private IEnumerator LeftSlotItemCooldown(float AfterSeconds)
    {
        LeftSlotAvailableToUse = false;
        yield return new WaitForSeconds(AfterSeconds);
        LeftSlotAvailableToUse = true;
    }

    private IEnumerator RightSlotItemCooldown(float AfterSeconds)
    {
        RightSlotAvailableToUse = false;
        yield return new WaitForSeconds(AfterSeconds);
        RightSlotAvailableToUse = true;
    }


    private GameObject RememberLoudout;
    public void GetPlayerItemsAndArmorValues()
    {
        RememberLoudout = GameObject.FindGameObjectWithTag("Loadout");
        RememberLoadout gitValues = RememberLoudout.GetComponent<RememberLoadout>();

        gitValues.numberOfheals = stats.NumofHeal;
        gitValues.numberOfPhizerz = stats.NumofPhizer;
        gitValues.armorRemaining = Stats.Armorz;
        gitValues.proteinCounter = Stats.NumofProtein;
        //gitValues.stemCellAmount = Stats.;
        //gitValues.numberOfStickyNades = stats.num;
        gitValues.numberOfMollys = Stats.NumofMolly;



    }

    public void SetPlayerItemsAndArmorValues()
    {
        RememberLoudout = GameObject.FindGameObjectWithTag("Loadout");
        RememberLoadout gitValues = RememberLoudout.GetComponent<RememberLoadout>();

        stats.NumofHeal = gitValues.numberOfheals;
        stats.NumofPhizer = gitValues.numberOfPhizerz;
        Stats.Armorz = gitValues.armorRemaining;
        Stats.NumofProtein = gitValues.proteinCounter;
        //gitValues.stemCellAmount = Stats.;
        //gitValues.numberOfStickyNades = stats.num;
        Stats.NumofMolly = gitValues.numberOfMollys;



    }




}
