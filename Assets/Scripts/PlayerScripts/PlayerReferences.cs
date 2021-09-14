using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferences
{
    Player player;

    public Vector2 MousePosToPlayer { get; set; }

    public PlayerReferences(Player player)
    {
        this.player = player;
    }

    public void CalMousePosToPlayer() 
    { 
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePosToPlayer = (mousePos - player.Stats.Position).normalized;
    }
}
