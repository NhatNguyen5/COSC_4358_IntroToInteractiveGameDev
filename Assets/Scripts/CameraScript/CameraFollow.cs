using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera myCamera;

    [Header("Target")]
    public Player player;
    public Transform RightArm;

    [Header("Movement")]
    public float DefaultCameraMoveSpeed;
    [HideInInspector]
    public float cameraMoveSpeed;
    public float ADSTrigger;
    public bool AutoADS;

    [Header("Zoom")]
    public float DefaultZoomLevel;
    public float cameraZoom;
    public float cameraZoomSpeed;

    private Weapon activeWeapon;
    private float defaultSpeed;
    private Vector3 follLocation;
    private float defaultZoom;
    private bool moveHold = false;
    private bool zoomHold = false;

    private void Awake()
    {
        myCamera = transform.GetComponent<Camera>();
        myCamera.orthographicSize = DefaultZoomLevel;
        defaultZoom = cameraZoom;
        defaultSpeed = DefaultCameraMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        //Player player = GetPlayerFunc();
        //Weapon weapon = GetWeaponFunc();
        if (RightArm != null)
        {
            foreach (Transform rw in RightArm)
            {
                //Debug.Log(rw.gameObject.activeSelf);
                if (rw.gameObject.activeSelf)
                {
                    activeWeapon = RightArm.transform.Find(rw.name).GetComponent<Weapon>();
                    //Debug.Log("Found");
                    break;
                }
            }
        }

        float ADSRange = 0;
        float ADSSpeed = defaultSpeed;
        if (activeWeapon != null)
        {
            ADSRange = activeWeapon.ADSRange;
            ADSSpeed = activeWeapon.ADSSpeed;
        }
        
        if (!moveHold)
        {
            follLocation = player.Stats.Position;
            cameraMoveSpeed = defaultSpeed;
        }
        
        Vector3 cameraFollowPosition = follLocation;
        Vector3 MP2P = new Vector3(player.References.MousePosToPlayer.x * ADSRange, player.References.MousePosToPlayer.y * ADSRange);

        if (AutoADS && player.References.MousePosToPlayerNotNorm.magnitude > ADSTrigger)
            cameraFollowPosition = cameraFollowPosition + MP2P;

        if (!AutoADS && Input.GetKey(KeyCode.Mouse1) && !player.Stats.IsDualWield)
        {
            cameraFollowPosition = cameraFollowPosition + MP2P;
            if (!moveHold)
                cameraMoveSpeed = ADSSpeed;
        }
        else if(!Input.GetKey(KeyCode.Mouse1))
        {
            if (!moveHold)
                cameraMoveSpeed = DefaultCameraMoveSpeed;
        }


        cameraFollowPosition.z = transform.position.z;

        Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);
        if (distance > 0)
        {
            Vector3 newCameraPosition = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;
            float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

            if (distanceAfterMoving > distance)
            {
                newCameraPosition = cameraFollowPosition;
            }
            
            transform.position = newCameraPosition;
        }

    }
    private void HandleZoom()
    {
        if (!zoomHold)
            cameraZoom = defaultZoom;
        float cameraZoomDiff = cameraZoom - myCamera.orthographicSize;

        myCamera.orthographicSize += cameraZoomDiff * cameraZoomSpeed * Time.deltaTime;

        if (cameraZoomDiff > 0)
        {
            if(myCamera.orthographicSize > cameraZoom)
            {
                myCamera.orthographicSize = cameraZoom;
            }
        }
        else
        {
            if(myCamera.orthographicSize < cameraZoom)
            {
                myCamera.orthographicSize = cameraZoom;
            }
        }
    }

    public IEnumerator MoveTo(Vector3 location, float duration, float speed)
    {
        follLocation = location;
        cameraMoveSpeed = speed;
        moveHold = true;
        /*
        Debug.Log("Moved to");
        Vector3 cameraFollowPosition = location;
        cameraFollowPosition.z = transform.position.z;

        Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        Debug.Log(cameraMoveDir);

        Vector3 newCameraPosition = transform.position + cameraMoveDir * speed * Time.deltaTime;
        transform.position = newCameraPosition;
        */
        yield return new WaitForSeconds(duration);
        moveHold = false;
    }

    public IEnumerator ZoomTo(float zoom, float duration)
    {
        cameraZoom = zoom;
        zoomHold = true;
        yield return new WaitForSeconds(duration);
        zoomHold = false;
    }

}
