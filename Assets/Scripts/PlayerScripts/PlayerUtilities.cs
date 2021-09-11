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
        player.Stats.Direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 WeaponDir = mousePos - player.Stats.Position;
        player.Stats.Angle = Mathf.Atan2(WeaponDir.y, WeaponDir.x) * Mathf.Rad2Deg;
    }
}
