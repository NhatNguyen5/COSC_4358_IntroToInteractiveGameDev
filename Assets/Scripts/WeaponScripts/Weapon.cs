using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Gun Settings")]
    public float Slot;
    public Sprite WeapnIcon;
    public float IconScale = 1;
    public string WeaponLabel;
    public Animator WeaponAnim;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float delay = 0.2f;
    float firingDelay = 0.0f;
    public float spread = 0.0f;
    public int amountOfShots;
    public float ADSRange;
    public float ADSSpeed;

    public float weaponWeight = 0;


    //GUN REWORK
    [Header("Bloom Settings")]
    public bool bloom;
    public float bStartRange;
    public float bEndRange;
    public float increaseBloom;
    public float bloomResetTimer;
    private float bloomTimer;
    //public float bulletDMG;
    [Header("Crit Settings")]
    public float critRate;
    public float critDamage;

    [Header("Bullet Settings")]
    public bool explodingBullets;
    public bool pierce;
    public int targetsToPierce;
    public float dropOffPerTarget;
    public float bulletSpeed = 45f;
    //public Rigidbody2D rb;
    public float bulletDamage = 25f;
    public float bullletKnockBackForce = 0.25f;

    public float bulletDamageDropOff;
    public float timeToDropDmg;

    private float textAmmoXPosition;

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
    private Image ammoBar;
    private Image ammoBar2;
    private Text UIAmmoCount;
    private Text UIMaxAmmoCount;
    private bool reload;
    private float countFillAmount;
    public int CostPerBullet;
    public float bulletLifeTime = 3f;

    [HideInInspector]
    public bool IsRightArm;

    private bool Pullout = false;

    private Player player;
    //public PlayerActions SwapWeapon;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        reloadOnStartUp();
        reloadCooldown = reloadTime;
        if (transform.parent.name == "RightArm")
        {
            IsRightArm = true;
        }
        else if (transform.parent.name == "LeftArm")
        {
            IsRightArm = false;
        }

        if (IsRightArm)
        {
            ammoBar = GameObject.Find("AmmoCountBar").GetComponent<Image>();
            reloadBar = GameObject.Find("ReloadBarR").GetComponent<Image>();
            UIAmmoCount = GameObject.Find("AmmoCountR").GetComponent<Text>();
            UIMaxAmmoCount = GameObject.Find("MaxAmmoCountR").GetComponent<Text>();
        }
    }


    private void reloadOnStartUp()
    {
        ammoCount = maxAmmoInClip;
    }

    private void reloadClip()
    {

        if ((GlobalPlayerVariables.Reserves - maxAmmoInClip * CostPerBullet) > 0 && ammoCount == 0)
        {
            //Debug.Log("first");
            GlobalPlayerVariables.Reserves -= maxAmmoInClip * CostPerBullet;
            ammoCount = maxAmmoInClip;
        }
        else if ((GlobalPlayerVariables.Reserves - (maxAmmoInClip - ammoCount) * CostPerBullet) > 0 && ammoCount > 0)
        {
            //Debug.Log("second");
            ammoCount = maxAmmoInClip - ammoCount;
            GlobalPlayerVariables.Reserves -= ammoCount * CostPerBullet;
            ammoCount = maxAmmoInClip;
        }
        else if ((GlobalPlayerVariables.Reserves) > 0 && ammoCount >= 0)
        {
            //Debug.Log("third");

            //int keeptrack = 0;
            //keeptrack = maxAmmoInClip - ammoCount; //23 shots 30-23 = 7
            /*
            if (keeptrack <= (int)GlobalPlayerVariables.Reserves)//res = 14
            {
                GlobalPlayerVariables.Reserves -= keeptrack; //14-7 = 7
            }
            if (keeptrack >= (int)GlobalPlayerVariables.Reserves) //30>14
            {
                keeptrack = (int)GlobalPlayerVariables.Reserves; //14
                //GlobalPlayerVariables.Reserves -= keeptrack; //14-14 = 0
            }


            ammoCount += keeptrack; //23+7 = 30
            //PLUS EQUAL MESSES WITH LOGIC
            */

            int remaining = 0;
            remaining = (int)GlobalPlayerVariables.Reserves / CostPerBullet;
            //Debug.Log("REMAINING " + remaining);
            //Debug.Log("KEEPTRACK " +keeptrack);

            GlobalPlayerVariables.Reserves -= remaining * CostPerBullet; //14-14
            ammoCount += remaining;
        }
        firingDelay = 0;
        //Debug.Log(GlobalPlayerVariables.Reserves);
        if(IsRightArm)
            reloadBar.fillAmount = GlobalPlayerVariables.Reserves / GlobalPlayerVariables.MaxReserves;
    }

    private void setAmmoCount()
    {
        if (UIAmmoCount != null && UIMaxAmmoCount != null)
        {
            UIAmmoCount.text = ammoCount.ToString();
            UIMaxAmmoCount.text = maxAmmoInClip.ToString();
            countFillAmount = GlobalPlayerVariables.Reserves / GlobalPlayerVariables.MaxReserves;
        }
        //Debug.Log(countFillAmount);
        //reloadBar.fillAmount = countFillAmount;

    }

    private void setDamage()
    {
        if (IsRightArm)
        {
            GlobalPlayerVariables.critRate = critRate * GlobalPlayerVariables.BaseCritRate;
            GlobalPlayerVariables.critDmg = critDamage * GlobalPlayerVariables.BaseCritDamage;
            GlobalPlayerVariables.bulletDamage = bulletDamage;
            GlobalPlayerVariables.bulletKnockbackForce = bullletKnockBackForce;
            GlobalPlayerVariables.bulletExplosion = explodingBullets;
            GlobalPlayerVariables.bulletPierce = pierce;
            GlobalPlayerVariables.bulletSpeed = bulletSpeed;
            GlobalPlayerVariables.targetsToPierce = targetsToPierce;
            GlobalPlayerVariables.damageDropOff = dropOffPerTarget;
            GlobalPlayerVariables.bulletDamageDropOff = bulletDamageDropOff;
            GlobalPlayerVariables.timeToDropDmg = timeToDropDmg;
        }
        else
        {
            GlobalPlayerVariables.critRate2 = critRate * GlobalPlayerVariables.BaseCritRate2;
            GlobalPlayerVariables.critDmg2 = critDamage * GlobalPlayerVariables.BaseCritDamage2;
            //Debug.Log(GlobalPlayerVariables.critDmg2);
            GlobalPlayerVariables.bulletDamage2 = bulletDamage;
            GlobalPlayerVariables.bulletKnockbackForce2 = bullletKnockBackForce;
            GlobalPlayerVariables.bulletExplosion2 = explodingBullets;
            GlobalPlayerVariables.bulletPierce2 = pierce;
            GlobalPlayerVariables.bulletSpeed2 = bulletSpeed;
            GlobalPlayerVariables.targetsToPierce2 = targetsToPierce;
            GlobalPlayerVariables.damageDropOff2 = dropOffPerTarget;
            GlobalPlayerVariables.bulletDamageDropOff2 = bulletDamageDropOff;
            GlobalPlayerVariables.timeToDropDmg2 = timeToDropDmg;
        }

        GlobalPlayerVariables.bulletLifeTime = bulletLifeTime;
    }


    private void Update()
    {

        //refill bar on weapon switch
        //if (PlayerActions.SwapWeapon() {
        //    ammoBar.fillAmount = (float)ammoCount / (float)maxAmmoInClip;
        //}

        if (!IsRightArm && player.Stats.IsDualWield && Pullout)
        {
            Pullout = false;
            ammoBar2 = GameObject.Find("AmmoCountBar2").GetComponent<Image>();
            UIAmmoCount = GameObject.Find("AmmoCountL").GetComponent<Text>();
            UIMaxAmmoCount = GameObject.Find("MaxAmmoCountL").GetComponent<Text>();
            setAmmoCount();
        }
        else
        {
            Pullout = true;
        }


        if (IsRightArm) // RightArm
        {
            RightArmUpdate();
        }
        else if(!IsRightArm && player.Stats.IsDualWield)// LeftArm
        {
            LeftArmUpdate();
        }
    }

    private void RightArmUpdate()
    {
        GlobalPlayerVariables.weaponWeight = weaponWeight;
        reloadBar.fillAmount = GlobalPlayerVariables.Reserves / GlobalPlayerVariables.MaxReserves;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log("Set variable");
            setDamage();
        }

        setAmmoCount();

        if (bStartRange > bEndRange)
        {
            bStartRange = bEndRange;
        }

        if (bStartRange < 0)
        {
            bStartRange = 0;
        }

        if (bloomTimer < 0)
        {
            bStartRange = 0;
            bloomTimer = 0;
        }

        /*
        if (reloadCooldown < reloadTime)
        {
        ammoBar.fillAmount = (reloadCooldown / reloadTime);

        //set ammo count here too
        } else { 
            ammoBar.fillAmount = 1;
        }
        */



        //Debug.Log("AMMO COUNT " + ammoCount);
        if (ammoCount <= 0)
        {
            firingDelay = 0;
        }


        if ((ammoCount <= 0 && Input.GetKey(KeyCode.Mouse0) || ammoCount < maxAmmoInClip && Input.GetKeyUp(KeyCode.R)) && firingDelay == 0)
        {
            if (GlobalPlayerVariables.Reserves > 0)
            {
                if (reload == false && fired == false)
                {


                    //Debug.Log("Reload activated");
                    WeaponAnim.SetBool("IsShooting", false);
                    WeaponAnim.SetFloat("FireRate", 0);
                    WeaponAnim.SetBool("IsReloading", true);
                    WeaponAnim.SetFloat("ReloadSpeed", 1 / reloadTime);
                    reloadCooldown = 0;

                    reload = true;
                    bStartRange = 0;
                }
            }
        }
        //Debug.Log(reloadCooldown);
        if (reloadCooldown == reloadTime && reload == true)
        {
            reloadClip();

            reload = false;
            WeaponAnim.SetBool("IsReloading", false);
            WeaponAnim.SetFloat("ReloadSpeed", 0);

        }


        if (reload == false && Shop.CurrentlyInShop == false && GameObject.Find("Thymus") == null)
        {
            if (burstFire == false)
            {
                if (Input.GetKey(KeyCode.Mouse0) && OptionSettings.GameisPaused == false && firingDelay == 0)
                {
                    if (ammoCount > 0)
                    {

                        Shoot();
                        bStartRange += increaseBloom;
                        bloomTimer = bloomResetTimer;
                        ammoCount--;

                        //decrease bar height
                        ammoBar.fillAmount = (float)ammoCount / (float)maxAmmoInClip;

                        //RectTransform rt = UIAmmoCount.rectTransform;
                        //Vector3 startPos = rt.anchoredPosition;

                        //textAmmoXPosition = textAmmoXPosition.transform.position;


                        //rt.anchoredPosition = rt.anchoredPosition(textAmmoXPosition, UIAmmoCount.Transform.position.y);

                        //Debug.Log("startPos: " + startPos);


                        //.GetComponent<RectTransform>();
                        //UIAmmoCount.anchoredPosition = new Vector3();

                        //transform.Translate();

                        //textAmmoPosition = UIAmmoCount.GetComponent<Transform>().position
                        //(float)ammoCount / (float)maxAmmoInClip;


                        //Debug.Log("ammo count: " + ammoCount);
                        //Debug.Log("maxAmmoInClip: " + maxAmmoInClip);
                        //Debug.Log("ammo/max:" + ammoCount / maxAmmoInClip);

                        //Debug.Log(ammoCount);

                    }
                }
                if (!Input.GetKey(KeyCode.Mouse0))
                {
                    WeaponAnim.SetBool("IsShooting", false);
                    WeaponAnim.SetFloat("FireRate", 0);
                }
                if (bStartRange >= 0)
                    bStartRange -= Time.deltaTime;

                if(firingDelay > 0)
                    firingDelay -= Time.deltaTime;
                else
                    firingDelay = 0;
            }
            else
            {

                if (Input.GetKeyDown(KeyCode.Mouse0) && OptionSettings.GameisPaused == false && firingDelay == 0)
                {
                    if (ammoCount > 0)
                    {
                        fired = true;
                    }
                }

                if (fired == true)
                {
                                        
                    if (TimesShot < timesToShoot && ammoCount > 0)
                    {
                        //TimesShot++;
                        if (burstTime < 0)
                        {
                            TimesShot++;
                            burst();
                            bStartRange += increaseBloom;
                            bloomTimer = bloomResetTimer;
                            ammoCount--;

                            //decrease bar height
                            ammoBar.fillAmount = (float)ammoCount / (float)maxAmmoInClip;

                            //Debug.Log(ammoCount);
                        }

                    }
                    else
                    {
                        TimesShot = 0;
                        firingDelay = delay;
                        fired = false;
                    }
                    //bStartRange -= Time.deltaTime;
                    burstTime -= Time.deltaTime;


                }
                if (fired == false)
                {
                    WeaponAnim.SetBool("IsShooting", false);
                    WeaponAnim.SetFloat("FireRate", 0);
                }
                if (bStartRange >= 0)
                    bStartRange -= Time.deltaTime;

                if(firingDelay > 0)
                    firingDelay -= Time.deltaTime;
                else
                    firingDelay = 0;
            }
        }


        if (reloadCooldown < reloadTime)
            reloadCooldown += Time.deltaTime * GlobalPlayerVariables.reloadSpeedBonus; //might be inefficient keep close eye on this
        else
            reloadCooldown = reloadTime;

        bloomTimer -= Time.deltaTime;
        
        
        if (reload == true)
        {
            ammoBar.fillAmount = (reloadCooldown / reloadTime);

        }
        
        
    }

    private void LeftArmUpdate()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            //Debug.Log("Set variable");
            setDamage();
        }


        setAmmoCount();

        if (bStartRange > bEndRange)
        {
            bStartRange = bEndRange;
        }

        if (bStartRange < 0)
        {
            bStartRange = 0;
        }

        if (bloomTimer < 0)
        {
            bStartRange = 0;
            bloomTimer = 0;
        }



        /*
        if (reloadCooldown < reloadTime)
        {
            ammoBar.fillAmount = (reloadCooldown / reloadTime);
            //set ammo count here too
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

        if (reloadCooldown == reloadTime && reload == true)
        {
            reloadClip();

            reload = false;
            WeaponAnim.SetBool("IsReloading", false);
            WeaponAnim.SetFloat("ReloadSpeed", 0);

        }


        if (reload == false && Shop.CurrentlyInShop == false)
        {
            if (burstFire == false)
            {
                if (Input.GetButton("Fire2") && OptionSettings.GameisPaused == false && firingDelay == 0)
                {
                    if (ammoCount > 0)
                    {

                        Shoot();
                        bStartRange += increaseBloom;
                        bloomTimer = bloomResetTimer;
                        ammoCount--;

                        //decrease bar height

                        ammoBar2.fillAmount = (float)ammoCount / (float)maxAmmoInClip;
                        

                        //Debug.Log(ammoCount);
                    }
                }
                if (!Input.GetButton("Fire2"))
                {
                    WeaponAnim.SetBool("IsShooting", false);
                    WeaponAnim.SetFloat("FireRate", 0);
                }
                if (bStartRange >= 0)
                    bStartRange -= Time.deltaTime;
                if(firingDelay > 0)
                    firingDelay -= Time.deltaTime;
                else
                    firingDelay = 0;
            }
            else
            {

                if (Input.GetButtonDown("Fire2") && OptionSettings.GameisPaused == false && firingDelay == 0)
                {
                    if (ammoCount > 0)
                    {
                        fired = true;
                    }
                }

                if (fired == true)
                {

                    if (TimesShot < timesToShoot && ammoCount > 0)
                    {
                        //TimesShot++;
                        if (burstTime < 0)
                        {
                            TimesShot++;
                            burst();
                            bStartRange += increaseBloom;
                            bloomTimer = bloomResetTimer;
                            ammoCount--;

                            ammoBar2.fillAmount = (float)ammoCount / (float)maxAmmoInClip;

                            //Debug.Log(ammoCount);
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
                if (bStartRange >= 0)
                    bStartRange -= Time.deltaTime;
                if(firingDelay > 0)
                    firingDelay -= Time.deltaTime;
                else
                    firingDelay = 0;
            }
        }
        if (reloadCooldown < reloadTime)
            reloadCooldown += Time.deltaTime;
        else
            reloadCooldown = reloadTime;
        bloomTimer -= Time.deltaTime;
        
        
        if (reload == true)
        {
            ammoBar2.fillAmount = (reloadCooldown / reloadTime);

        }
        

    }

    private void burst()
    {
        WeaponAnim.SetBool("IsShooting", true);
        WeaponAnim.SetFloat("FireRate", 1f / timeBtwBurst);
        //code for adding bloom goes around here
        for (int i = 0; i < amountOfShots; i++)
        {
            float WeaponSpread = 0;
            if (bloom == false)
                WeaponSpread = Random.Range(-spread, spread);
            else
            {
                WeaponSpread = Random.Range(-bStartRange, bStartRange);
            }
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
            float WeaponSpread = 0;
            if (bloom == false)
                WeaponSpread = Random.Range(-spread, spread);
            else
            {
                WeaponSpread = Random.Range(-bStartRange, bStartRange);
            }
            Quaternion newRot = Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + WeaponSpread);

            Instantiate(bulletPrefab, firePoint.position, newRot);
        }
    }
    private void OnDisable()
    {
        UIAmmoCount.text = "0";
        UIMaxAmmoCount.text = "0";
    }
}
