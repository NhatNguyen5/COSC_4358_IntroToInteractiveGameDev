using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public float delay = 0.2f;
    float firingDelay = 0.0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && OptionSettings.GameisPaused == false && firingDelay < 0)
        {
            Shoot();
        }
        firingDelay -= Time.deltaTime;
        if (firingDelay < -10000)
            firingDelay = 0;

    }

    void Shoot()
    {
        firingDelay = delay;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
