using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

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
    [SerializeField]
    private CapsuleCollider2D playerCapsuleCollider2D;
    [SerializeField]
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField]
    private SpriteLibrary spriteLibrary = default;
    [SerializeField]
    private SpriteResolver targetResolver = default;
    [SerializeField]
    private string targetCategory = default;
    [SerializeField]
    private ParticleSystem playerParticleSystem;


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

    public CapsuleCollider2D PlayerCapsuleCollider2D
    {
        get => playerCapsuleCollider2D;
    }

    public SpriteRenderer PlayerSpriteRenderer
    {
        get => playerSpriteRenderer;
    }

    public SpriteLibrary PlayerSpriteLibrary
    {
        get => spriteLibrary;
    }

    public SpriteResolver PlayerSpriteResolver
    {
        get => targetResolver;
    }

    public string PlayerTargetCategory
    {
        get => targetCategory;
    }

    public ParticleSystem PlayerParticleSystem
    {
        get => playerParticleSystem;
    }
}
