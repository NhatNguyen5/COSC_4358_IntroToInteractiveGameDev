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

    private RightWeapon activeWeapon;

    private void Awake()
    {
        myCamera = transform.GetComponent<Camera>();
        myCamera.orthographicSize = DefaultZoomLevel;
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
        foreach (Transform rw in RightArm)
        {
            //Debug.Log(rw.gameObject.activeSelf);
            if (rw.gameObject.activeSelf)
            {
                activeWeapon = RightArm.transform.Find(rw.name).GetComponent<RightWeapon>();
                //Debug.Log("Found");
                break;
            }
        }
        float ADSRange = activeWeapon.ADSRange;
        float ADSSpeed = activeWeapon.ADSSpeed;
        float cameraMoveSpeed = DefaultCameraMoveSpeed;
        Vector3 cameraFollowPosition = player.Stats.Position;
        Vector3 MP2P = new Vector3(player.References.MousePosToPlayer.x * ADSRange, player.References.MousePosToPlayer.y * ADSRange);

        if (AutoADS && player.References.MousePosToPlayerNotNorm.magnitude > ADSTrigger)
            cameraFollowPosition = cameraFollowPosition + MP2P;

        if (!AutoADS && Input.GetKey(KeyCode.Mouse1) && !player.Stats.IsDualWield)
        {
            cameraFollowPosition = cameraFollowPosition + MP2P;
            cameraMoveSpeed = ADSSpeed;
        }
        else if(!Input.GetKey(KeyCode.Mouse1))
        {
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
}
