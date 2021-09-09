using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions
{
    private Player player;
    public void Move(Transform transform)
    {
        player.Components.PlayerRidgitBody.velocity = Vector2.right;
    }
}
