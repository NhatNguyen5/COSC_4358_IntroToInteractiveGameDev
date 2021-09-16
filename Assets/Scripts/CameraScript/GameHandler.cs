using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public Player player;

    private void Start()
    {
        cameraFollow.Setup(() => player.Stats.Position);  
    }
}
