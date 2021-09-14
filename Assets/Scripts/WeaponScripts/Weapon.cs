using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Gun Settings")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float delay = 0.2f;
    float firingDelay = 0.0f;
    public float spread = 0.0f;
    public int amountOfShots;


    [Header("Burst Settings")]
    public bool burstFire;
    public float timeBtwBurst;
    private float burstTime = 0;
    public int timesToShoot;
    private int TimesShot = 0;
    private bool fired;


    void Update()
    {
        if (burstFire == false)
        {
            if (Input.GetButton("Fire1") && OptionSettings.GameisPaused == false && firingDelay < 0)
            {
                Shoot();
            }

            firingDelay -= Time.deltaTime;
            if (firingDelay < -10000)
                firingDelay = 0;
        }
        else
        {

            if (Input.GetButtonDown("Fire1") && OptionSettings.GameisPaused == false && firingDelay < 0)
            {

                fired = true;
            }

            if (fired == true)
            {

                if (TimesShot < timesToShoot)
                {
                    //TimesShot++;
                    if (burstTime < 0)
                    {
                        TimesShot++;
                        burst();
                    }

                }
                else
                {
                    TimesShot = 0;
                    firingDelay = delay;
                    fired = false;
                }
                burstTime -= Time.deltaTime;


            }

            firingDelay -= Time.deltaTime;
            if (firingDelay < -10000)
                firingDelay = 0;
        }

    }

    void burst()
    {

        for (int i = 0; i < amountOfShots; i++)
        {
            float WeaponSpread = Random.Range(-spread, spread);
            Quaternion newRot = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + WeaponSpread);
            Instantiate(bulletPrefab, firePoint.position, newRot);
            burstTime = timeBtwBurst;
        }

    }

    void Shoot()
    {
        firingDelay = delay;
        for (int i = 0; i < amountOfShots; i++)
        {
            float WeaponSpread = Random.Range(-spread, spread);
            Quaternion newRot = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + WeaponSpread);

            Instantiate(bulletPrefab, firePoint.position, newRot);
        }
    }
}
