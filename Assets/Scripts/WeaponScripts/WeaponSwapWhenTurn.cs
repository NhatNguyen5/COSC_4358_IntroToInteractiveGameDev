using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapWhenTurn : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform RightArm;
    public Transform LeftArm;
    private float angle;
    private bool swap = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        angle = GetComponent<Player>().Stats.Angle;
        if(angle < 0)
        {
            angle = 360 + angle;
        }
        if ((angle > 5 && angle < 175) && swap == false)
        {
            Vector3 temp = RightArm.position;
            RightArm.position = LeftArm.position;
            LeftArm.position = temp;
            //Debug.Log("Swapped");
            swap = true;
        }
        if (angle > 185 && angle < 355 && swap == true)
        {
            Vector3 temp = RightArm.position;
            RightArm.position = LeftArm.position;
            LeftArm.position = temp;
            //Debug.Log("Swapped back");
            swap = false;
        }


        //Debug.Log(angle);
    }
}
