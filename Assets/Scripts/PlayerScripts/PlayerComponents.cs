using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerComponents
{
    [SerializeField]
    private Rigidbody2D playerRidgitBody;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private StatusIndicator playerStatusIndicator;
    [SerializeField]
    private LayerMask playerLayerMask;
    [SerializeField]
    private TrailRenderer playerTrailRenderer;

    public Rigidbody2D PlayerRidgitBody
    {
        get => playerRidgitBody;
    }

    public Animator PlayerAnimator
    {
        get => playerAnimator;
    }

    public StatusIndicator PlayerStatusIndicator
    {
        get => playerStatusIndicator;
    }

    public LayerMask PlayerLayerMask
    {
        get => playerLayerMask;
    }

    public TrailRenderer PlayerTrailRenderer
    {
        get => playerTrailRenderer;
    }
}
