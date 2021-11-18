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
    private float maxZD;
    private float ADSRange = 0;
    private float ADSSpeed = 0;

    private void Awake()
    {
        myCamera = transform.GetComponent<Camera>();
        myCamera.orthographicSize = DefaultZoomLevel;
        defaultZoom = cameraZoom;
        defaultSpeed = DefaultCameraMoveSpeed;
        ADSRange = 0;
        ADSSpeed = defaultSpeed;
        maxZD = new Vector3(player.References.MousePosToPlayer.x * ADSRange, player.References.MousePosToPlayer.y * ADSRange).magnitude;
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

        if (activeWeapon != null)
        {
            ADSRange = activeWeapon.ADSRange;
            ADSSpeed = activeWeapon.ADSSpeed;
        }
        else
        {
            ADSRange = 0;
            ADSSpeed = defaultSpeed;
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
            //New Ads, broken
            /*
            var mousePos = Input.mousePosition;
            mousePos.x -= Screen.width / 2;
            mousePos.y -= Screen.height / 2;

            float normalizeNum;
            if (Screen.width > Screen.height)
                normalizeNum = Screen.height;
            else
                normalizeNum = Screen.width;

            float percentagePos = mousePos.magnitude / (normalizeNum/2);

            Vector2 tempPos = new Vector2(Mathf.Abs(mousePos.x / (Screen.width / 2)), Mathf.Abs(mousePos.y / (Screen.height / 2)));

            if (player.References.MousePosToPlayerNotNorm.magnitude >= 1 && player.References.MousePosToPlayerNotNorm.magnitude <= ADSRange)
                cameraFollowPosition = new Vector3(cameraFollowPosition.x + MP2P.x * (percentagePos - 0.5f) * (player.References.MousePosToPlayerNotNorm.magnitude / ADSRange), cameraFollowPosition.y + MP2P.y * (percentagePos - 0.5f) * (player.References.MousePosToPlayerNotNorm.magnitude / ADSRange));
            else if (player.References.MousePosToPlayerNotNorm.magnitude >= ADSRange)
                cameraFollowPosition = new Vector3(cameraFollowPosition.x + MP2P.x * (player.References.MousePosToPlayerNotNorm.magnitude / ADSRange), cameraFollowPosition.y + MP2P.y * (player.References.MousePosToPlayerNotNorm.magnitude / ADSRange));


            Debug.Log(player.References.MousePosToPlayerNotNorm.magnitude);
            */
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
            Vector3 newCameraPosition = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime ;
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
