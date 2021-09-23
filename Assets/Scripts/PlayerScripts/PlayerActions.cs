using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions
{
    private Player player;
    public float defaultSpeed;

    public PlayerActions(Player player)
    {
        this.player = player;
    }

    public void Move(Transform transform)
    {
        player.Components.PlayerRidgitBody.velocity = new Vector2(player.Stats.Direction.x * player.Stats.Speed * Time.deltaTime, player.Stats.Direction.y * player.Stats.Speed * Time.deltaTime);
    }

    public void Sprint()
    {
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

}