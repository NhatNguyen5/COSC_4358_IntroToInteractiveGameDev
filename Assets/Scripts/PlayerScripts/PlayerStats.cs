using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public Vector2 Direction { get; set; }

    public float Angle { get; set; }

    public Vector2 Position { get; set; }

    public float PFrictionz { get; set; }

    public float Speed { get; set; }

    public float Health { get; set; }

    public float MaxHealth { get; set; }

    public float HPRegen { get; set; }

    public float Stamina { get; set; }

    public float StamDrainRate { get; set; }

    public float MaxStamina { get; set; }
    public float StaminaRegenRate { get; set; }
    public float TimeBeforeStamRegen { get; set; }

    public int NumofHeal { get; set; }
    public int NumofProtein { get; set; }
    public int NumofPhizer { get; set; }
    public int NumofMolly { get; set; }

    public bool IsDualWield = false;

    public float PhizerDurationz { get; set; }
    public float PhizerCooldownz { get; set; }
    public float HPAddz { get; set; }
    public float HPRegenAddz { get; set; }
    public float StamAddz { get; set; }
    public float StamRegenAddz { get; set; }




    public float TylenolHealAmountz { get; set; }
    public float TylenoCooldownz { get; set; }

    [Header("Movement")]
    [SerializeField]
    private float PFriction;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float sprintSpeed;
    [SerializeField]
    private float dashDistance;
    [SerializeField]
    private float dashCooldown;
    [SerializeField]
    private float dashStamCost;
    [SerializeField]
    private float dashSpeed;
    [Header("Stats")]
    [SerializeField]
    private float HP;
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float HPRegenRate;
    [SerializeField]
    private float playerStamina;
    [SerializeField]
    private float maxPlayerStamina;
    [SerializeField]
    private float stamDrainRate;
    [SerializeField]
    public float staminaRegenRate;
    [SerializeField]
    private float TimeBfrStamRegen;
    [Header("Start out Inv")]
    [SerializeField]
    private int NumOfHeal;
    [SerializeField]
    private int NumOfProtein;
    [SerializeField]
    private int NumOfPhizer;
    [SerializeField]
    private int NumOfMolly;
    [Header("Phizer")]
    [SerializeField]
    private float Phizerduration;
    [SerializeField]
    private float Phizercooldown;
    [Tooltip("In Percentage")]
    [SerializeField]
    private float hpAdd;
    [SerializeField]
    private float hpRegenAdd;
    [Tooltip("In Percentage")]
    [SerializeField]
    private float stamAdd;
    [SerializeField]
    private float stamRegenAdd;
    [Header("Tylenol")]
    [SerializeField]
    private float Tylenolhealamount;
    [SerializeField]
    private float Tylenolcooldown;

    public float pfriction { get => PFriction; }
    public float WalkSpeed { get => walkSpeed; }

    public float SprintSpeed { get => sprintSpeed; }

    public float DashDistance { get => dashDistance; }

    public float DashCoolDown { get => dashCooldown; }
    public float DashSpeed { get => dashSpeed; }
    public float DashStamCost { get => dashStamCost; }

    public float hp { get => HP; }

    public float maxhp { get => maxHP; }

    public float hpregenrate { get => HPRegenRate; }

    public float stamina { get => playerStamina; }

    public float maxplayerstamina { get => maxPlayerStamina; }

    public float stamdrainrate { get => stamDrainRate; }

    public float staminaregenrate { get => staminaRegenRate; }

    public float StaminaRegenWait { get => TimeBfrStamRegen; }

    public int numofheal { get => NumOfHeal; }

    public int numofprotein { get => NumOfProtein; }

    public int numofphizer { get => NumOfPhizer; }

    public int numofmolly { get => NumOfMolly; }

    public float PhizerDuration { get => Phizerduration; }
    public float PhizerCooldown { get => Phizercooldown; }
    public float HPAdd { get => hpAdd; }
    public float HPRegenAdd { get => hpRegenAdd; }
    public float StamAdd { get => stamAdd; }
    public float StamRegenAdd { get => stamRegenAdd; }

    public float TylenolHealAmount { get => Tylenolhealamount; }
    public float TylenolCooldown { get => Tylenolcooldown; }

}
