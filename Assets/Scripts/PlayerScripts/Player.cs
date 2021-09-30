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
        stats.NumofHeal = stats.numofheal;
        actions.defaultSpeed = stats.Speed;
        actions.HealCounts.text = stats.NumofHeal.ToString();
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(stats.Stamina > 0)
                stats.Stamina -= Time.deltaTime;
            StaminaBar.fillAmount = stats.Stamina / stats.MaxStamina;
            isRunning = true;
            sprint = 0;
        }


        if (stats.Stamina <= stats.MaxStamina && isRunning == false && sprint >= stats.TimeBeforeStamRegen)
        { 
            stats.Stamina += Time.deltaTime;
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
            
        }
        //Debug.Log("HP" + stats.hp);
        //Debug.Log("Health" + stats.Health);

        //Debug.Log(components.PlayerRidgitBody.velocity.magnitude);
        //Debug.Log(references.MousePosToPlayer);
    }

    private void FixedUpdate()
    {
        actions.Move(transform);
        if (Input.GetKey(KeyCode.LeftShift) && stats.Stamina > 0)
            actions.Sprint();
        else
            actions.Walk();
        actions.Animate();
        
    }
}
