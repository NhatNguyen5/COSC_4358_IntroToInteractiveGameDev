using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerComponents
{
    [SerializeField]
    private Rigidbody2D playerRidgitBody;

    public Rigidbody2D PlayerRidgitBody 
    {
        get => playerRidgitBody;
    }
}
