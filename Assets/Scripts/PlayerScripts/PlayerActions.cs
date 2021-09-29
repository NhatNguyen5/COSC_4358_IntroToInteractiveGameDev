using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions
{
    private Player player;
    public float defaultSpeed;
    private Transform leftArm;
    private Transform rightArm;
    private Image HealthBar;
    private Image reloadBarL;
    private Image reloadBorderL;
    private Text UIAmmoCountL;
    private Text UIMaxAmmoCountL;

    public PlayerActions(Player player)
    {
        this.player = player;
        leftArm = player.transform.Find("LeftArm");
        rightArm = player.transform.Find("RightArm");
        HealthBar = GameObject.Find("HP").GetComponent<Image>();
        reloadBarL = GameObject.Find("ReloadBarL").GetComponent<Image>();
        UIAmmoCountL = GameObject.Find("AmmoCountL").GetComponent<Text>();
        UIMaxAmmoCountL = GameObject.Find("MaxAmmoCountL").GetComponent<Text>();
        reloadBorderL = GameObject.Find("ReloadBorderL").GetComponent<Image>();
        reloadBarL.gameObject.SetActive(false);
        UIAmmoCountL.gameObject.SetActive(false);
        UIMaxAmmoCountL.gameObject.SetActive(false);
        reloadBorderL.gameObject.SetActive(false);
    }

    public void Move(Transform transform)
    {
        player.Components.PlayerRidgitBody.velocity = new Vector2(player.Stats.Direction.x * player.Stats.Speed * Time.deltaTime, player.Stats.Direction.y * player.Stats.Speed * Time.deltaTime);
    }

    public void Sprint()
    {
        if(player.Stats.Stamina >= 0)
            player.Stats.Speed = player.Stats.SprintSpeed;
    }

    public void Walk()
    {
        player.Stats.Speed = player.Stats.WalkSpeed;
    }

    public void Animate()
    {
        player.Components.PlayerAnimator.SetFloat("MovementX", player.References.MousePosToPlayer.x);
        player.Components.PlayerAnimator.SetFloat("MovementY", player.References.MousePosToPlayer.y);
        if(player.Components.PlayerRidgitBody.velocity.magnitude == 0)
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
            reloadBarL.gameObject.SetActive(true);
            UIAmmoCountL.gameObject.SetActive(true);
            UIMaxAmmoCountL.gameObject.SetActive(true);
            reloadBorderL.gameObject.SetActive(true);
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
            reloadBarL.gameObject.SetActive(false);
            UIAmmoCountL.gameObject.SetActive(false);
            UIMaxAmmoCountL.gameObject.SetActive(false);
            reloadBorderL.gameObject.SetActive(false);
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
                }
                else
                {
                    rw.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Heal()
    {
        if(player.References.numOfHeal > 0 && player.Stats.Health < player.Stats.hp)
        {
            //Debug.Log(player.References.numOfHeal);
            if (player.Stats.hp - player.Stats.Health < 25)
            {
                player.Stats.Health = player.Stats.hp;
                player.References.numOfHeal--;
                HealthBar.fillAmount = player.Stats.Health / player.Stats.hp;
                player.Components.PlayerStatusIndicator.StartFlash(0.5f, 0.25f, Color.green, 0f, Color.red, 2);
                //player.Components.PlayerStatusIndicator.ChangeTransparency((player.Stats.hp - player.Stats.Health) / player.Stats.hp);
            }
            else
            {
                player.Stats.Health += 25;
                player.References.numOfHeal--;
                HealthBar.fillAmount = player.Stats.Health / player.Stats.hp;
                player.Components.PlayerStatusIndicator.StartFlash(0.25f, ((player.Stats.hp - player.Stats.Health) / player.Stats.hp), Color.green, ((player.Stats.hp - player.Stats.Health) / player.Stats.hp)/2f, Color.red, 1);
                //player.Components.PlayerStatusIndicator.ChangeTransparency((player.Stats.hp - player.Stats.Health) / player.Stats.hp);
            }
        }
    }
}