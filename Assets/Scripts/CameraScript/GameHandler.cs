using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public Player player;
    public Weapon weapon;

    private void Start()
    {
        cameraFollow.SetupPlayer(() => player);
        cameraFollow.SetupWeapon(() => weapon);
    }
}
