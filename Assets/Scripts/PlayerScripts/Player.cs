using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerStats stats;

    [SerializeField]
    private PlayerComponents components;

    private PlayerReferences references;

    private PlayerUtilities utilities;

    private PlayerActions actions;

    public PlayerComponents Components 
    { 
        get => components; 
    }


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        actions.Move(transform);
    }
}
