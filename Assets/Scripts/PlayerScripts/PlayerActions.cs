using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions
{
    private Player player;
    public float defaultSpeed;
    private float DashDistance;
    private float DashSpeed;
    private Transform leftArm;
    private Transform rightArm;
    private Image HealthBar;
    public Text HealCounts;
    public Text VaccineCounts;
    private GameObject LeftAmmo;
    private GameObject RWeaponIcon;
    private Image VaccineCooldownDisplay;
    private float relaMouseAngle;

    private string[] CurrHemoSprite;

    //private bool isWaiting = false;

    public PlayerActions(Player player)
    {
        this.player = player;
        leftArm = player.transform.Find("LeftArm");
        rightArm = player.transform.Find("RightArm");
        HealthBar = GameObject.Find("HP").GetComponent<Image>();
        LeftAmmo = GameObject.Find("LeftAmmo");
        RWeaponIcon = GameObject.Find("WeaponBorderR");
        HealCounts = GameObject.Find("HealCounts").GetComponent<Text>();
        VaccineCounts = GameObject.Find("VaccineCounts").GetComponent<Text>();
        LeftAmmo.gameObject.SetActive(false);
        DashDistance = player.Stats.DashDistance;
        DashSpeed = player.Stats.DashSpeed;
        VaccineCooldownDisplay = GameObject.Find("VaccineCoolDownIndicator").GetComponent<Image>();
        foreach (Transform rw in rightArm)
        {
            if (rw.gameObject.activeSelf)
            {
                foreach (Transform WIcon in RWeaponIcon.transform)
                    WIcon.gameObject.SetActive(false);
                if (rw.name.Contains("Protocal"))
                {
                    RWeaponIcon.transform.Find("ProtocalIcon").gameObject.SetActive(true);
                }
                else if (rw.name.Contains("Enforcer"))
                {
                    RWeaponIcon.transform.Find("EnforcerIcon").gameObject.SetActive(true);
                }
            }
        }
        CurrHemoSprite = player.Components.PlayerSpriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(player.Components.PlayerTargetCategory).ToArray();

    }

    public void Move(Transform transform)
    {
        player.Components.PlayerRidgitBody.velocity = new Vector2(player.Stats.Direction.x * player.Stats.Speed * Time.deltaTime, player.Stats.Direction.y * player.Stats.Speed * Time.deltaTime);
    }

    public void Sprint()
    {
        if(player.Stats.Stamina >= 0)
            player.Stats.Speed = player.Stats.SprintSpeed * (GlobalPlayerVariables.BaseWeaponWeight - GlobalPlayerVariables.weaponWeight);
    }

    public void Walk()
    {
        player.Stats.Speed = player.Stats.WalkSpeed * (GlobalPlayerVariables.BaseWeaponWeight - GlobalPlayerVariables.weaponWeight);
    }

    public void Dash(Vector2 Dir)
    {
        player.Components.PlayerRidgitBody.MovePosition(player.Stats.Position + Dir * DashSpeed * (GlobalPlayerVariables.BaseWeaponWeight - GlobalPlayerVariables.weaponWeight) * Time.fixedDeltaTime);
    }

    public void Animate()
    {
        player.Components.PlayerAnimator.SetFloat("MovementX", player.References.MousePosToPlayer.x);
        player.Components.PlayerAnimator.SetFloat("MovementY", player.References.MousePosToPlayer.y);

        relaMouseAngle = player.Stats.Angle;

        if (relaMouseAngle < 0)
            relaMouseAngle = relaMouseAngle + 360;

        Debug.Log(relaMouseAngle);
        //New 8 directions system
        if(relaMouseAngle < 22.5 && relaMouseAngle > 337.5) //Right
        {

        }
        else if(relaMouseAngle < 22.5 && relaMouseAngle > 337.5) //BotRight
        {

        }
        else if (relaMouseAngle < 22.5 && relaMouseAngle > 337.5) //Down
        {

        }
        else if (relaMouseAngle < 22.5 && relaMouseAngle > 337.5) //BotLeft
        {

        }
        else if (relaMouseAngle < 22.5 && relaMouseAngle > 337.5) //Left
        {

        }
        else if (relaMouseAngle < 22.5 && relaMouseAngle > 337.5) //TopLeft
        {

        }
        else if (relaMouseAngle < 22.5 && relaMouseAngle > 337.5) //Up
        {

        }
        else if (relaMouseAngle < 22.5 && relaMouseAngle > 337.5) //TopRight
        {

        }

        if (player.Components.PlayerRidgitBody.velocity.magnitude == 0)
        {
            player.Components.PlayerAnimator.SetFloat("BounceRate", 0.5f);
        }
        else
        {
            player.Components.PlayerAnimator.SetFloat("BounceRate", player.Stats.Speed / defaultSpeed);
        }
        
    }

    public void ToggleDual()
    {
        
        if(!leftArm.gameObject.activeSelf)
        {
            leftArm.gameObject.SetActive(true);
            LeftAmmo.gameObject.SetActive(true);
            player.Stats.IsDualWield = true;

        }
        else
        {
            foreach (Transform lw in leftArm)
            {
                if (lw.gameObject.activeSelf == true)
                {
                    LeftWeapon lWeapon = lw.GetComponent<LeftWeapon>();
                    lWeapon.transform.position = leftArm.transform.position;
                    lWeapon.transform.rotation = leftArm.transform.rotation;
                }
            }

            leftArm.gameObject.SetActive(false);
            LeftAmmo.gameObject.SetActive(false);
            player.Stats.IsDualWield = false;
        }
        //Debug.Log("PA: " + player.Stats.IsUpWhenSwap);
    }

    public void SwapWeapon()
    {
        float input;
        if(float.TryParse(Input.inputString, out input))
        {
            foreach (Transform rw in rightArm)
            {
                //Debug.Log(rw.gameObject.activeSelf);
                if (rw.GetComponent<RightWeapon>().Slot == input)
                {
                    rw.gameObject.SetActive(true);
                    foreach (Transform WIcon in RWeaponIcon.transform)
                        WIcon.gameObject.SetActive(false);
                    if (rw.name.Contains("Protocal"))
                    {
                        RWeaponIcon.transform.Find("ProtocalIcon").gameObject.SetActive(true);
                    }
                    else if(rw.name.Contains("Enforcer"))
                    {
                        RWeaponIcon.transform.Find("EnforcerIcon").gameObject.SetActive(true);
                    }

                }
                else
                {
                    RightWeapon RWeapon = rw.GetComponent<RightWeapon>();
                    RWeapon.transform.position = rightArm.transform.position;
                    RWeapon.transform.rotation = rightArm.transform.rotation;
                    rw.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Phizer()
    {
        if(player.Stats.NumofPhizer > 0)
        {
            player.Stats.NumofPhizer -= 1;
            player.Stats.HPRegen += player.Stats.HPRegenAdd;
            player.Stats.StaminaRegenRate += player.Stats.StamRegenAdd;
        }
    }

    public void Heal()
    {
        if(player.Stats.NumofHeal > 0 && player.Stats.Health < player.Stats.hp)
        {
            //Debug.Log(player.References.numOfHeal);
            if (player.Stats.hp - player.Stats.Health < 50)
            {
                player.Stats.Health = player.Stats.hp;
                player.Stats.NumofHeal--;
                player.Components.PlayerStatusIndicator.StartFlash(0.5f, 0.25f, Color.green, 0f, Color.red, 2);
                //player.Components.PlayerStatusIndicator.ChangeTransparency((player.Stats.hp - player.Stats.Health) / player.Stats.hp);
            }
            else
            {
                player.Stats.Health += 50;
                player.Stats.NumofHeal--;
                player.Components.PlayerStatusIndicator.StartFlash(0.25f, ((player.Stats.hp - player.Stats.Health) / player.Stats.hp), Color.green, ((player.Stats.hp - player.Stats.Health) / player.Stats.hp)/2f, Color.red, 1);
                //player.Components.PlayerStatusIndicator.ChangeTransparency((player.Stats.hp - player.Stats.Health) / player.Stats.hp);
            }
        }
    }

    public void UpdateHeal()
    {
        HealCounts.text = player.Stats.NumofHeal.ToString();
    }

    public void UpdateVaccine()
    {
        VaccineCounts.text = player.Stats.NumofPhizer.ToString();
    }

    public void Regen()
    {
        if (player.Stats.Health < player.Stats.hp)
        {
            player.Stats.Health += player.Stats.HPRegen * Time.deltaTime;
            player.Components.PlayerStatusIndicator.ChangeTransparency((player.Stats.hp - player.Stats.Health) / player.Stats.hp);
        }
        HealthBar.fillAmount = player.Stats.Health / player.Stats.hp;
    }

    
    public void ResetPlayerStats()
    {
        player.Stats.HPRegen -= player.Stats.HPRegenAdd;
        player.Stats.StaminaRegenRate -= player.Stats.StamRegenAdd;
    }

    public void VaccineCooldownDisplayUpdate(float FillAmount)
    {
        VaccineCooldownDisplay.fillAmount = FillAmount;
    }


    /*

    private IEnumerator wait(float duration)
    {
        isWaiting = true;
        yield return new WaitForSeconds(duration);
        isWaiting = false;
    }
    */

}