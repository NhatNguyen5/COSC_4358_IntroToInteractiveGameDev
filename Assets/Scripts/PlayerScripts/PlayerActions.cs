using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions
{
    private Player player;

    public PlayerActions(Player player)
    {
        this.player = player;
    }

    public void Move(Transform transform)
    {
        player.Components.PlayerRidgitBody.velocity = new Vector2(player.Stats.Direction.x * player.Stats.Speed * Time.deltaTime, player.Stats.Direction.y * player.Stats.Speed * Time.deltaTime);

        if (player.Stats.Direction.x != 0)
        {
            if (player.Stats.Direction.x < 0)
            {
                transform.Rotate(0f, 180f, 0f);
            }
            else
            {
                transform.Rotate(0f, 0f, 0f);
            }
        }
    }


}
