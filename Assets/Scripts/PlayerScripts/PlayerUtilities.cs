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
        player.Stats.Direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        player.Stats.Position = player.Components.PlayerRidgitBody.position;
        player.Stats.Angle = Mathf.Atan2(player.References.MousePosToPlayer.y, player.References.MousePosToPlayer.x) * Mathf.Rad2Deg;
    }
}
