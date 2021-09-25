using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PlayerActions
{
    private Player player;
    public float defaultSpeed;
    private Transform leftArm;
    private Transform rightArm;

    public PlayerActions(Player player)
    {
        this.player = player;
        leftArm = player.transform.Find("LeftArm");
        rightArm = player.transform.Find("RightArm");
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
            player.Stats.IsDualWield = true;
        }
        else
        {
            leftArm.gameObject.SetActive(false);
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

}