using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{

    float scaleY;
    private Transform aimTransform;
    //float scaleX;
    
    private void Start()
    {
        aimTransform = GameObject.Find("Aim").transform;
        scaleY = transform.localScale.y;
        //scaleX = transform.localScale.x;
    }
    private void Update()
    {
        if(OptionSettings.GameisPaused == false)
            handleAiming();
    }

    private void handleAiming()
    {
        
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            //transform.position.y *= -1;
            aimLocalScale.y = -1f;
            //aimLocalScale.y = -1f * scaleX;
        }
        else
        {
            aimLocalScale.y = +1f;
            //aimLocalScale.y = -1f * scaleX;
        }
        aimTransform.localScale = aimLocalScale;
    }


    private Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0;
        return vec;
    }

    private Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }


}
