using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Func<Player> GetPlayerFunc;
    private Func<Weapon> GetWeaponFunc;
    public float cameraMoveSpeed;
    public float ADSTrigger;
    public bool AutoADS;

    public void SetupPlayer(Func<Player> GetPlayerFunc)
    {
        this.GetPlayerFunc = GetPlayerFunc;
    }
    public void SetupWeapon(Func<Weapon> GetWeaponFunc)
    {
        this.GetWeaponFunc = GetWeaponFunc;
    }

    // Update is called once per frame
    void Update()
    {
        Player player = GetPlayerFunc();
        Weapon weapon = GetWeaponFunc();
        float ADSRange = weapon.ADSRange;
        Vector3 cameraFollowPosition = player.Stats.Position;
        Vector3 MP2P = new Vector3 (player.References.MousePosToPlayer.x * ADSRange, player.References.MousePosToPlayer.y * ADSRange);

        if(AutoADS && player.References.MousePosToPlayerNotNorm.magnitude > ADSTrigger)
            cameraFollowPosition = cameraFollowPosition + MP2P;

        if(!AutoADS && Input.GetKey(KeyCode.Mouse1))
        {
            cameraFollowPosition = cameraFollowPosition + MP2P;
        }


        cameraFollowPosition.z = transform.position.z;

        Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);
        if (distance > 0)
        {
            Vector3 newCameraPosition = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;
            float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

            if(distanceAfterMoving > distance)
            {
                newCameraPosition = cameraFollowPosition;
            }

            transform.position = newCameraPosition;
        }
    }
}
