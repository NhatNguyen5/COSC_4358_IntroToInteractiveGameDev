using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerStats stats;

    [SerializeField]
    private PlayerComponents components;

    [SerializeField]
    private PlayerReferences references;

    private PlayerUtilities utilities;

    private PlayerActions actions;
    public PlayerComponents Components { get => components; }
    public PlayerStats Stats { get => stats; }
    public PlayerReferences References { get => references; }

    private void Awake()
    {
        actions = new PlayerActions(this);
        utilities = new PlayerUtilities(this);
        references = new PlayerReferences(this);
        stats.Health = stats.hp;
        stats.Speed = stats.WalkSpeed;
        actions.defaultSpeed = stats.Speed;
    }


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        utilities.HandleInput();
        references.CalMousePosToPlayer();
        //Debug.Log("HP" + stats.hp);
        //Debug.Log("Health" + stats.Health);

        //Debug.Log(components.PlayerRidgitBody.velocity.magnitude);
        //Debug.Log(references.MousePosToPlayer);
    }

    private void FixedUpdate()
    {
        actions.Move(transform);
        if (Input.GetKey(KeyCode.LeftShift))
            actions.Sprint();
        else
            actions.Walk();
        actions.Animate();
    }
}
