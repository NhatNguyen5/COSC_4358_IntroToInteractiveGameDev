using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private Text EnemySpawnRate;



    private void Awake()
    {
        actions = new PlayerActions(this);
        utilities = new PlayerUtilities(this);
        references = new PlayerReferences(this);
        stats.Health = stats.hp;
        stats.Speed = stats.WalkSpeed;
        stats.Stamina = stats.stamina;
        stats.MaxStamina = stats.maxplayerstamina;
        stats.TimeBeforeStamRegen = stats.StaminaRegen;
        stats.StaminaRegenRate = stats.staminaRegenRate;
        stats.NumofHeal = stats.numofheal;
        stats.NumofProtein = stats.numofprotein;
        actions.defaultSpeed = stats.Speed;
        actions.HealCounts.text = stats.NumofHeal.ToString();
        DashDuration = stats.DashDistance / stats.DashSpeed;
        TrailDur = DashDuration;
        ProteinCounts = GameObject.Find("ProteinCounts").GetComponent<Text>();
        EnemySpawnRate = GameObject.Find("EnemySpawnRateDisplay").GetComponent<Text>();
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
        
        if (Input.GetKey(KeyCode.LeftShift) && stats.Direction != Vector2.zero)
        {
            if(stats.Stamina > 0)
                stats.Stamina -= Time.deltaTime;
            StaminaBar.fillAmount = stats.Stamina / stats.MaxStamina;
            isRunning = true;
            sprint = 0;
        }


        if (stats.Stamina <= stats.MaxStamina && isRunning == false && sprint >= stats.TimeBeforeStamRegen)
        { 
            stats.Stamina += Time.deltaTime * stats.staminaregenrate;
            StaminaBar.fillAmount = stats.Stamina / stats.MaxStamina;
        }
        isRunning = false;
        sprint += Time.deltaTime;

        if (OptionSettings.GameisPaused == false)
        {
            if(Input.GetKeyUp(KeyCode.T))
                actions.ToggleDual();
            if (Input.GetKeyUp(KeyCode.E))
                actions.Heal();
            actions.SwapWeapon();
            //Debug.DrawRay(stats.Position, stats.Direction, Color.green, 0.1f);
            DashProc();
        }
        //Debug.Log("HP" + stats.hp);
        //Debug.Log("Health" + stats.Health);

        //Debug.Log(components.PlayerRidgitBody.velocity.magnitude);
        //Debug.Log(references.MousePosToPlayer);
        //Debug.Log(enemyManager.timeBetweenSpawns);
        EnemySpawnRate.text = (1 / enemyManager.timeBetweenSpawns).ToString();
    }

    private void FixedUpdate()
    {
        actions.Move(transform);
        if (Input.GetKey(KeyCode.LeftShift) && stats.Stamina > 0)
            actions.Sprint();
        else
            actions.Walk();
        actions.Animate();
        ProteinCounts.text = stats.NumofProtein.ToString();
    }

    private void DashProc()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currDashCooldown == 0 && stats.Stamina >= stats.DashStamCost && stats.Direction.magnitude != 0)
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
        else
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
}
