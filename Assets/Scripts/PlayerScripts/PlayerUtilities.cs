using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtilities
{
    Player player;

    public PlayerUtilities(Player player)
    {
        this.player = player;
    }

    public void HandleInput()
    {
        if (GlobalPlayerVariables.EnablePlayerControl)
            player.Stats.Direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        else
            player.Stats.Direction = Vector2.zero;
        player.Stats.Position = player.Components.PlayerRidgitBody.position;
        player.Stats.Angle = Mathf.Atan2(player.References.MousePosToPlayer.y, player.References.MousePosToPlayer.x) * Mathf.Rad2Deg;
    }
}
