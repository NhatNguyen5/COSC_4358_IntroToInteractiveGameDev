using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerStats stats;

    [SerializeField]
    private PlayerComponents components;

    private PlayerReferences references;

    private PlayerUtilities utilities;

    private PlayerActions actions;
    public PlayerComponents Components { get => components; }
    public PlayerStats Stats { get => stats; }

    private void Awake()
    {
        actions = new PlayerActions(this);
        utilities = new PlayerUtilities(this);

        stats.Speed = stats.WalkSpeed;
    }


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        utilities.HandleInput();
        stats.Position = components.PlayerRidgitBody.position;
        Debug.Log(components.PlayerRidgitBody.velocity.magnitude);
        Debug.Log(Stats.Angle);
    }

    private void FixedUpdate()
    {
        actions.Move(transform);
    }
}
