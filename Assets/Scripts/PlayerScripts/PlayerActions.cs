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
    private Image HealCooldownDisplay;
    private float relaMouseAngle;

    private string currSpriteCategory;

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
        HealCooldownDisplay = GameObject.Find("HealCoolDownIndicator").GetComponent<Image>();
        player.Components.PlayerTrailRenderer.endColor = new Color(184/255f, 59/255f, 60/255f); // new Color(184, 59, 60);
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
        currSpriteCategory = player.Components.PlayerTargetCategory;

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

        relaMouseAngle = player.Stats.Angle;

        if (relaMouseAngle < 0)
            relaMouseAngle = relaMouseAngle + 360;

        //Debug.Log(relaMouseAngle);
        //New 8 directions system
        /*[0]Down
         *[1]Up
         *[2]Left
         *[3]Right
         *[4]TopLeft
         *[5]TopRight
         *[6]BotLeft
         *[7]BotRight
         */
        if(relaMouseAngle <= 22.5 || relaMouseAngle > 337.5) //Right
        {
            player.Components.PlayerSpriteResolver.SetCategoryAndLabel(currSpriteCategory, CurrHemoSprite[3]);
        }
        else if(relaMouseAngle > 22.5 && relaMouseAngle <= 67.5) //TopRight
        {
            player.Components.PlayerSpriteResolver.SetCategoryAndLabel(currSpriteCategory, CurrHemoSprite[5]);
        }
        else if (relaMouseAngle > 67.5 && relaMouseAngle <= 112.5) //Up
        {
            player.Components.PlayerSpriteResolver.SetCategoryAndLabel(currSpriteCategory, CurrHemoSprite[1]);
        }
        else if (relaMouseAngle > 112.5 && relaMouseAngle <= 157.5) //TopLeft
        {
            player.Components.PlayerSpriteResolver.SetCategoryAndLabel(currSpriteCategory, CurrHemoSprite[4]);
        }
        else if (relaMouseAngle > 157.5 && relaMouseAngle <= 202.5) //Left
        {
            player.Components.PlayerSpriteResolver.SetCategoryAndLabel(currSpriteCategory, CurrHemoSprite[2]);
        }
        else if (relaMouseAngle > 202.5 && relaMouseAngle <= 247.5) //BotLeft
        {
            player.Components.PlayerSpriteResolver.SetCategoryAndLabel(currSpriteCategory, CurrHemoSprite[6]);
        }
        else if (relaMouseAngle > 247.5 && relaMouseAngle <= 292.5) //Down
        {
            player.Components.PlayerSpriteResolver.SetCategoryAndLabel(currSpriteCategory, CurrHemoSprite[0]);
        }
        else if (relaMouseAngle > 292.5 && relaMouseAngle <= 337.5) //BotRight
        {
            player.Components.PlayerSpriteResolver.SetCategoryAndLabel(currSpriteCategory, CurrHemoSprite[7]);
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
        player.Stats.NumofPhizer -= 1;
        ParticleSystem.ColorOverLifetimeModule pSsettings = player.Components.PlayerParticleSystem.colorOverLifetime;
        pSsettings.color = new ParticleSystem.MinMaxGradient(new Color(255, 255, 255), new Color(0, 78, 137));
        player.Components.PlayerParticleSystem.Play();
        float tempMH = player.Stats.MaxHealth;
        float tempMS = player.Stats.MaxStamina;
        player.Stats.MaxHealth += player.Stats.MaxHealth * player.Stats.HPAdd / 100;
        //player.Stats.Health += player.Stats.MaxHealth - tempMH;
        player.Stats.MaxStamina += player.Stats.MaxStamina * player.Stats.StamAdd / 100;
        //player.Stats.Stamina += player.Stats.MaxStamina - tempMS;
        player.Stats.HPRegen += player.Stats.HPRegenAdd;
        player.Stats.StaminaRegenRate += player.Stats.StamRegenAdd;
        player.Components.PlayerTrailRenderer.endColor = new Color(0, 76 / 255f, 134 / 255f);
        currSpriteCategory = "PhizerHemo";
    }

    public void Heal()
    {
        //Debug.Log(player.References.numOfHeal);
        if (player.Stats.MaxHealth - player.Stats.Health < player.Stats.TylenolHealAmount)
        {
            player.Stats.Health = player.Stats.MaxHealth;
            player.Stats.NumofHeal--;
            player.Components.PlayerStatusIndicator.StartFlash(0.5f, 0.25f, Color.green, 0f, Color.red, 2);
            //player.Components.PlayerStatusIndicator.ChangeTransparency((player.Stats.hp - player.Stats.Health) / player.Stats.hp);
        }
        else
        {
            player.Stats.Health += player.Stats.TylenolHealAmount;
            player.Stats.NumofHeal--;
            player.Components.PlayerStatusIndicator.StartFlash(0.25f, ((player.Stats.MaxHealth - player.Stats.Health) / player.Stats.MaxHealth), Color.green, ((player.Stats.MaxHealth - player.Stats.Health) / player.Stats.MaxHealth) /2f, Color.red, 1);
            //player.Components.PlayerStatusIndicator.ChangeTransparency((player.Stats.hp - player.Stats.Health) / player.Stats.hp);
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
        if (player.Stats.Health < player.Stats.MaxHealth)
        {
            player.Stats.Health += player.Stats.HPRegen * Time.deltaTime;
            player.Components.PlayerStatusIndicator.ChangeTransparency((player.Stats.MaxHealth - player.Stats.Health) / player.Stats.MaxHealth);
        }
        HealthBar.fillAmount = player.Stats.Health / player.Stats.MaxHealth;
    }

    
    public void ResetPlayerStats()
    {
        player.Components.PlayerParticleSystem.Stop();
        player.Stats.HPRegen -= player.Stats.HPRegenAdd;
        player.Stats.StaminaRegenRate -= player.Stats.StamRegenAdd;
        player.Stats.MaxHealth = player.Stats.maxhp;
        if (player.Stats.Health > player.Stats.MaxHealth)
            player.Stats.Health = player.Stats.MaxHealth;
        player.Stats.MaxStamina = player.Stats.maxplayerstamina;
        if (player.Stats.Stamina > player.Stats.Stamina)
            player.Stats.Stamina = player.Stats.MaxStamina;
        player.Components.PlayerTrailRenderer.endColor = new Color(184 / 255f, 59 / 255f, 60 / 255f);
        currSpriteCategory = "DefaultHemo";
    }

    public void LeftSlotCooldownDisplayUpdate(float LeftSlotFillAmount)
    {
        VaccineCooldownDisplay.fillAmount = LeftSlotFillAmount;
    }

    public void RightSlotCooldownDisplayUpdate(float RightSlotFillAmount)
    {
        HealCooldownDisplay.fillAmount = RightSlotFillAmount;
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