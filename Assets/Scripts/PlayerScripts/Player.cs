using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

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

    //private float currSpeed;

    //private bool setSpeedBack = false;
    [SerializeField]
    private GameObject beat;

    //private GameObject gendBeat;

    private float beatTimer = 0;

    private void Awake()
    {
        actions = new PlayerActions(this);
        utilities = new PlayerUtilities(this);
        references = new PlayerReferences(this);
        stats.Health = stats.hp;
        stats.MaxHealth = stats.maxhp;
        stats.HPRegen = stats.hpregenrate;
        stats.Speed = stats.WalkSpeed;
        stats.Stamina = stats.stamina;
        stats.MaxStamina = stats.maxplayerstamina;
        stats.StamDrainRate = stats.stamdrainrate;
        stats.TimeBeforeStamRegen = stats.StaminaRegenWait;
        stats.StaminaRegenRate = stats.staminaRegenRate;
        stats.NumofHeal = stats.numofheal;
        stats.NumofProtein = stats.numofprotein;
        stats.NumofPhizer = stats.numofphizer;

        stats.PhizerDurationz = stats.PhizerDuration;
        stats.PhizerCooldownz = stats.PhizerCooldown;
        stats.HPRegenAddz = stats.HPRegenAdd;
        stats.StamRegenAddz = stats.StamRegenAdd;

        stats.TylenoCooldownz = stats.TylenolCooldown;
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
        defaultSR = 1/enemyManager.timeBetweenSpawns;
        
    }


    // Start is called before the first frame update
    private void Start()
    {
        StaminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        utilities.HandleInput();
        references.CalMousePosToPlayer();
        actions.UpdateHeal();
        actions.UpdateVaccine();
        actions.Regen();


        if (Input.GetKey(KeyCode.LeftShift) && stats.Direction != Vector2.zero)
        {
            if(stats.Stamina > 0)
                stats.Stamina -= Time.deltaTime*(stats.StamDrainRate - stats.StaminaRegenRate);
            StaminaBar.fillAmount = stats.Stamina / stats.MaxStamina;
            isRunning = true;
            sprint = 0;
        }


        if (stats.Stamina <= stats.MaxStamina && isRunning == false && sprint >= stats.TimeBeforeStamRegen)
        { 
            stats.Stamina += Time.deltaTime * stats.StaminaRegenRate;
            StaminaBar.fillAmount = stats.Stamina / stats.MaxStamina;
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
                    RightSlotCooldownDisplay = stats.TylenolCooldown;
                    StartCoroutine(RightSlotItemCooldown(stats.TylenolCooldown));
                }
            }
            if (Input.GetKeyUp(KeyCode.Q) && LeftSlotAvailableToUse)
            {
                if (stats.NumofPhizer > 0)
                {
                    actions.Phizer();
                    LeftSlotCooldownDisplay = stats.PhizerCooldown;
                    StartCoroutine(ResetStats(stats.PhizerDuration));
                    StartCoroutine(LeftSlotItemCooldown(stats.PhizerCooldown));
                }
            }
            actions.SwapWeapon();
            //Debug.DrawRay(stats.Position, stats.Direction, Color.green, 0.1f);
            DashProc();
        }
        //Debug.Log(VaccineCooldownDisplay);
        if(resetPlayerStatsRequest && !inEffect)
        {
            Debug.Log("RequestReset");
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
            .color = new Color(1 + (MinTbs - enemyManager.timeBetweenSpawns) / enemyManager.timeBetweenSpawns, 1 - (MaxTbs - enemyManager.timeBetweenSpawns) / enemyManager.timeBetweenSpawns, 0);

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

    private IEnumerator beatGen(float duration)
    {
        GameObject gendBeat;
        Transform HMBG = EnemySpawnRate.transform.Find("HeartMonitorBG");
        gendBeat = Instantiate(beat, HMBG, false);
        //gendBeat.transform.localScale = HMBG.localScale;
        gendBeat.GetComponent<RawImage>().color = new Color(1 + (MinTbs - enemyManager.timeBetweenSpawns) / enemyManager.timeBetweenSpawns, 1 - (MaxTbs - enemyManager.timeBetweenSpawns) / enemyManager.timeBetweenSpawns, 0);
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
        yield return new WaitForSeconds(AfterSeconds);
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

}
