using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerReferences
{
    Player player;

    public Vector2 MousePosToPlayer { get; set; }
    public Vector2 MousePosToPlayerNotNorm { get; set; }
    public int numOfHeal { get; set; }

    public PlayerReferences(Player player)
    {
        this.player = player;
    }

    public void CalMousePosToPlayer() 
    {
        //Debug.Log(player.References.MousePosToPlayerNotNorm);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePosToPlayerNotNorm = mousePos - player.Stats.Position;
        MousePosToPlayer = MousePosToPlayerNotNorm.normalized;
    }
}
