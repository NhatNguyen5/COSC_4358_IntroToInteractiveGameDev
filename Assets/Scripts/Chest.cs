using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    [System.Serializable]
    public struct Droppable
    {
        public GameObject[] items;
        public int chanceOfDrop;
    }

    private Animator anim;
    public bool interacted = false;
    private Vector3 chestPosition;
    private int totalPercent = 0;

    [Header("Drops")]
    public Droppable[] droppables;

    // Start is called before the first frame update
    void Start()
    {
        chestPosition = transform.position;
        anim = GetComponent<Animator>();
        for (int i = 0; i < droppables.Length; i++)
        {
            totalPercent = totalPercent + droppables[i].chanceOfDrop;
        }
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
        interacted = true;
        anim.SetBool("opened", true);
        int randomNum = Random.Range(0, totalPercent);
        int numAccrued = 0;
        Debug.Log("total Percent = " + totalPercent);
        Debug.Log("RandomNum = " + randomNum);
        for (int i = 0; i < droppables.Length; i++)
        {
            if (droppables[i].chanceOfDrop == 0)
                continue;
            numAccrued = numAccrued + droppables[i].chanceOfDrop;
            Debug.Log("numAccrued = " + numAccrued);
            if (randomNum <= numAccrued)
            {
                for (int j = 0; j < droppables[i].items.Length; j++)
                {
                    Instantiate(droppables[i].items[j], chestPosition, Quaternion.identity);
                }
                break;
            }
        }
/*
        for (int i = 0; i < droppables.Length; i++)
        {
            Instantiate(droppables[i], chestPosition, Quaternion.identity);
            // droppables[i].transform.position = chestPosition;
        }*/
    }
}
