using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftWeapon : MonoBehaviour
{
    [Header("Gun Settings")]
    public float Slot;
    public Animator WeaponAnim;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float delay = 0.2f;
    float firingDelay = 0.0f;
    public float spread = 0.0f;
    public int amountOfShots;
    public float ADSRange;
    public float ADSSpeed;


    [Header("Burst Settings")]
    public bool burstFire;
    public float timeBtwBurst;
    private float burstTime = 0;
    public int timesToShoot;
    private int TimesShot = 0;
    private bool fired;



    [Header("Ammo and Reloading")]
    public int maxAmmoInClip;
    private int ammoCount;
    public float reloadTime;
    private float reloadCooldown;
    private Image reloadBar;
    private Text UIAmmoCount;
    private Text UIMaxAmmoCount;
    private bool reload;
    private float countFillAmount;


    private void Start()
    {
        reloadClip();
        reloadCooldown = reloadTime;
        reloadBar = GameObject.Find("ReloadBarL").GetComponent<Image>();
        UIAmmoCount = GameObject.Find("AmmoCountL").GetComponent<Text>();
        UIMaxAmmoCount = GameObject.Find("MaxAmmoCountL").GetComponent<Text>();
        setAmmoCount();
    }

    private void reloadClip()
    {
        ammoCount = maxAmmoInClip;
        firingDelay = 0;
    }

    private void setAmmoCount()
    {
        UIAmmoCount.text = ammoCount.ToString();
        UIMaxAmmoCount.text = maxAmmoInClip.ToString();
        countFillAmount = (float)ammoCount / (float)maxAmmoInClip;
        //Debug.Log(countFillAmount);
        reloadBar.fillAmount = countFillAmount;

    }


    private void Update()
    {
        setAmmoCount();
        //if (reloadCooldown < reloadTime)
        //{
        //reloadBar.fillAmount = (reloadCooldown / reloadTime);

        //set ammo count here too
        //}
        /*
        else
        { 
            reloadBar.fillAmount = 1;
        }
        */



        if (ammoCount <= 0 && Input.GetButton("Fire2") || ammoCount < maxAmmoInClip && Input.GetKeyUp(KeyCode.R))
        {
            if (reload == false && fired == false)
            {
                Debug.Log("Reload activated");
                WeaponAnim.SetBool("IsShooting", false);
                WeaponAnim.SetFloat("FireRate", 0);
                WeaponAnim.SetBool("IsReloading", true);
                WeaponAnim.SetFloat("ReloadSpeed", 1 / reloadTime);
                reloadCooldown = 0;

                reload = true;
            }
        }

        if (reloadCooldown >= reloadTime && reload == true)
        {
            reloadClip();

            reload = false;
            WeaponAnim.SetBool("IsReloading", false);
            WeaponAnim.SetFloat("ReloadSpeed", 0);

        }


        if (reload == false)
        {
            if (burstFire == false)
            {
                if (Input.GetButton("Fire2") && OptionSettings.GameisPaused == false && firingDelay < 0)
                {
                    if (ammoCount > 0)
                    {

                        Shoot();
                        ammoCount--;

                        //Debug.Log(ammoCount);
                    }
                }
                if (!Input.GetButton("Fire2"))
                {
                    WeaponAnim.SetBool("IsShooting", false);
                    WeaponAnim.SetFloat("FireRate", 0);
                }

                firingDelay -= Time.deltaTime;
                if (firingDelay < -10000)
                    firingDelay = 0;
            }
            else
            {

                if (Input.GetButtonDown("Fire2") && OptionSettings.GameisPaused == false && firingDelay < 0)
                {
                    if (ammoCount > 0)
                    {
                        fired = true;
                    }
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
                            ammoCount--;

                            Debug.Log(ammoCount);
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
                if (fired == false)
                {
                    WeaponAnim.SetBool("IsShooting", false);
                    WeaponAnim.SetFloat("FireRate", 0);
                }

                firingDelay -= Time.deltaTime;
                if (firingDelay < -10000)
                    firingDelay = 0;
            }
        }

        reloadCooldown += Time.deltaTime;

        if (reloadCooldown < -10000)
            reloadCooldown = 0;
        if (reload == true)
        {
            reloadBar.fillAmount = (reloadCooldown / reloadTime);

        }
    }

    private void burst()
    {
        WeaponAnim.SetBool("IsShooting", true);
        WeaponAnim.SetFloat("FireRate", 1f / timeBtwBurst);
        //code for adding bloom goes around here
        for (int i = 0; i < amountOfShots; i++)
        {
            float WeaponSpread = Random.Range(-spread, spread);
            Quaternion newRot = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + WeaponSpread);
            Instantiate(bulletPrefab, firePoint.position, newRot);
            burstTime = timeBtwBurst;
        }

    }

    private void Shoot()
    {
        WeaponAnim.SetBool("IsShooting", true);
        WeaponAnim.SetFloat("FireRate", 1f / delay);
        firingDelay = delay;
        //code for adding bloom goes around here
        for (int i = 0; i < amountOfShots; i++)
        {
            float WeaponSpread = Random.Range(-spread, spread);
            Quaternion newRot = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + WeaponSpread);

            Instantiate(bulletPrefab, firePoint.position, newRot);
        }
    }

}
