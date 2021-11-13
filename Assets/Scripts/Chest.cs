using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    private Animator anim;
    public bool interacted = false;
    public GameObject[] droppables;
    private Vector3 chestPosition;
    // Start is called before the first frame update
    void Start()
    {
        chestPosition = transform.position;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !interacted)
        {
            // On 'e' press open up the object
            if (Input.GetKeyDown(KeyCode.F))
            {
                openChest();
            }
        }
    }

    private void openChest()
    {
        int displacement = 0;
        interacted = true;
        anim.SetBool("opened", true);
        for (int i = 0; i < droppables.Length; i++)
        {
            Instantiate(droppables[i], chestPosition, Quaternion.identity);
            // droppables[i].transform.position = chestPosition;
        }
    }
}
