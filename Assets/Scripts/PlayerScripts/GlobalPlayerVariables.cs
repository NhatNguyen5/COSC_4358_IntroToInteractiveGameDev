using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalPlayerVariables : MonoBehaviour
{
    public static float TotalScore;
    public float rechargeRate = 1;

    public static float weaponWeight = 1;
    public static float BaseWeaponWeight = 1;

    public float bcr1 = 1;
    public float bcd1 = 1;
    public float bcr2 = 1;
    public float bcd2 = 1;


    //ammo reserves
    public float playerMaxReserves;
    public float chargeMultiplyer = 1;
    public static float rechargeRateMultiplyer = 1;
    public static float reserveCount = 0;
    public static float Reserves = 500;
    public static float MaxReserves = 1000;

    //public static float playerHealth = 100;
    //right weapon stats
    public static float BaseCritRate = 1;
    public static float BaseCritDamage = 1;
    public static float bulletSpeed = 0f;
    public static float bulletDamage = 0;
    public static float bulletKnockbackForce = 0;
    public static bool bulletPierce = false;
    public static int targetsToPierce = 0;
    public static float damageDropOff = 0;

    public static float critRate = 0; //RANGE FROM 0 TO 1
    public static float critDmg = 0;

    //damage drop off for bullets
    public static float bulletDamageDropOff = 0;
    public static float timeToDropDmg = 0;

    //left weapon stats
    public static float BaseCritRate2 = 1;
    public static float BaseCritDamage2 = 1;
    public static float bulletSpeed2 = 0f;
    public static float bulletDamage2 = 0;
    public static float bulletKnockbackForce2 = 0;
    public static bool bulletPierce2 = false;
    public static int targetsToPierce2 = 0;
    public static float damageDropOff2 = 0;

    public static float critRate2 = 0; //RANGE FROM 0 TO 1
    public static float critDmg2 = 0;


    //damage drop off for bullets
    public static float bulletDamageDropOff2 = 0;
    public static float timeToDropDmg2 = 0;


    public static bool GameOver = false;


    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            resetStats();
        }
    }

    private void resetStats()
    {
        //Debug.Log("Stats Reset");

        BaseCritRate = bcr1;
        BaseCritDamage = bcd1;
        BaseCritRate2 = bcr2;
        BaseCritDamage2 = bcd2;


        critDmg = BaseCritDamage;
        critRate = BaseCritRate;

        critDmg2 = BaseCritDamage2;
        critRate2 = BaseCritRate2;

        Reserves = playerMaxReserves;
        reserveCount = playerMaxReserves;

        rechargeRateMultiplyer = chargeMultiplyer;


        TotalScore = 0;
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            //recharge bar
            if (Reserves < playerMaxReserves)
            {
                Reserves += Time.deltaTime * rechargeRate;
            }
        }
    }



}
