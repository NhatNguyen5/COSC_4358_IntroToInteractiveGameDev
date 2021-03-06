using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTurnSwap : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform RightArm;
    public Transform LeftArm;
    private float angle;
    private bool swap = false;
    //private bool IsUpWhenSwap;

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (OptionSettings.GameisPaused == false)
        {
            angle = GetComponent<Player>().Stats.Angle;
            if (angle < 0)
            {
                angle = 360 + angle;
            }

            if ((angle >= 0 && angle <= 180) && swap == true)
            {
                Vector3 temp = RightArm.position;
                RightArm.position = LeftArm.position;
                LeftArm.position = temp;

                swap = false;
            }


            if ((angle > 180 && angle < 360) && swap == false)
            {
                Vector3 temp = RightArm.position;
                RightArm.position = LeftArm.position;
                LeftArm.position = temp;

                swap = true;
            }
        }
        

        //Debug.Log(angle);
    }
}
