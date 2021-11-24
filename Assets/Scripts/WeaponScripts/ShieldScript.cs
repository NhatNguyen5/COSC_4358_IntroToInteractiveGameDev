using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShieldScript : MonoBehaviour
{

    [Header("Gun Settings")]
    public float Slot;
    public Sprite WeapnIcon;
    public float IconScale = 1;
    public string WeaponLabel;
    public float BaseDamage;
    public float GrowthRate = 1;
    public float BaseSwingSpeed = 1;
    public float delay = 0.2f;
    float firingDelay = 0.0f;
    public float ADSRange;
    public float ADSSpeed;


    public float weaponWeight = 1;

    private BoxCollider2D hitBox;
    private Animator animCtrl;
    private Image reloadBar;
    private Image ammoBar;
    private Image ammoBar2;
    private Text UIAmmoCount;
    private Text UIMaxAmmoCount;
    private int swingCount = 1;
    public float knockBackForce;
    [HideInInspector]
    public bool deploy = false;

    private int numDep = 1;

    private float deployDelay;

    private int shieldDirIdx;
    private int shieldDirLeftIdx;
    private int shieldDirRightIdx;

    [HideInInspector]
    public bool IsRightArm;

    private bool Pullout = false;

    private string[] direction = { 
        "ShieldRight",
        "ShieldTopRight",
        "ShieldUp",
        "ShieldTopLeft",
        "ShieldLeft",
        "ShieldBotLeft",
        "ShieldDown",
        "ShieldBotRight"
        };

    private Player player;
    //public PlayerActions SwapWeapon;
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        IsRightArm = true;

        if (IsRightArm)
        {
            ammoBar = GameObject.Find("AmmoCountBar").GetComponent<Image>();
            reloadBar = GameObject.Find("ReloadBarR").GetComponent<Image>();
            UIAmmoCount = GameObject.Find("AmmoCountR").GetComponent<Text>();
            UIMaxAmmoCount = GameObject.Find("MaxAmmoCountR").GetComponent<Text>();
        }
        deployDelay = delay;
        animCtrl = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (OptionSettings.GameisPaused == false)
        {
            GlobalPlayerVariables.weaponWeight = weaponWeight;
            if (deploy)
            {
                transform.position = player.transform.position;
                deployShield();
                GlobalPlayerVariables.weaponWeight = 1;
                animCtrl.SetBool("ShieldDeploy", true);
                Collider2D[] shieldedBullets = Physics2D.OverlapCircleAll(transform.position, 1.6f);
                foreach (var shieldedBullet in shieldedBullets)
                {
                    if (shieldedBullet.CompareTag("EnemyBullet") || shieldedBullet.CompareTag("EnemyBullet2"))
                    {
                        if (shieldedBullet.CompareTag("EnemyBullet"))
                        {
                            shieldedBullet.GetComponent<EnemyProj>().DestroyEnemyProj();

                        }
                        else
                        {
                            shieldedBullet.GetComponent<EnemyProj2>().DestroyEnemyProj();
                        }
                    }
                }
            }
            else
            {
                animCtrl.SetBool("ShieldDeploy", false);
                animateThis();
            }


            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (!deploy)
                {
                    deploy = true;
                }
                else
                {
                    deploy = false;
                    GlobalPlayerVariables.weaponWeight = weaponWeight;
                }
            }
        }
        
        //Debug.Log(animCtrl.GetCurrentAnimatorStateInfo(0).IsName("1stSwingAnim"));
        //Debug.Log(swingCount);
    }

    private void animateThis()
    {
        float relaMouseAngle = player.Stats.Angle;

        if (relaMouseAngle < 0)
            relaMouseAngle = relaMouseAngle + 360;

        //Debug.Log(relaMouseAngle);
        //New 8 directions system
        /*[0]Down
         *[1]Up
         *[2]Left
         *[3]Right
         *[4]TopLeft
         *[5]TopRight
         *[6]BotLeft
         *[7]BotRight
         */

        /*
        if (relaMouseAngle <= 22.5 || relaMouseAngle > 337.5) //Right
        {
            enableDir("ShieldRight");
        }
        else if (relaMouseAngle > 22.5 && relaMouseAngle <= 67.5) //TopRight
        {
            enableDir("ShieldTopRight");
        }
        else if (relaMouseAngle > 67.5 && relaMouseAngle <= 112.5) //Up
        {
            enableDir("ShieldUp");
        }
        else if (relaMouseAngle > 112.5 && relaMouseAngle <= 157.5) //TopLeft
        {
            enableDir("ShieldTopLeft");
        }
        else if (relaMouseAngle > 157.5 && relaMouseAngle <= 202.5) //Left
        {
            enableDir("ShieldLeft");
        }
        else if (relaMouseAngle > 202.5 && relaMouseAngle <= 247.5) //BotLeft
        {
            enableDir("ShieldBotLeft");
        }
        else if (relaMouseAngle > 247.5 && relaMouseAngle <= 292.5) //Down
        {
            enableDir("ShieldDown");
        }
        else if (relaMouseAngle > 292.5 && relaMouseAngle <= 337.5) //BotRight
        {
            enableDir("ShieldBotRight");
        }
        */
        Debug.Log(shieldDirIdx);
        shieldDirIdx = Mathf.FloorToInt((relaMouseAngle + 22.5f) / 45);
        shieldDirIdx = shieldDirIdx > 7 ? 0 : shieldDirIdx;
        shieldDirRightIdx = shieldDirIdx - 1 >= 0 ? shieldDirIdx - 1 : 7;
        shieldDirLeftIdx = shieldDirIdx + 1 <= 7 ? shieldDirIdx + 1 : 0;
        enableDir(direction[shieldDirIdx]);
        //Debug.Log(direction[shieldDirLeftIdx > 7 ? 0 : shieldDirLeftIdx]);
    }

    private void deployShield()
    {
        bool finishDeploy = true;
        if (deployDelay > 0)
        {
            deployDelay -= Time.deltaTime;
        }
        else
        {
            deployDelay = 0;
        }

        foreach (Transform shieldDir in transform)
        {
            if (!shieldDir.gameObject.activeSelf)
                finishDeploy = false;
        }

        if (!finishDeploy && deployDelay == 0)
        {
            if (!transform.GetChild(shieldDirLeftIdx).gameObject.activeSelf)
            {
                //Debug.Log(shieldDirLeftIdx);
                transform.GetChild(shieldDirLeftIdx).gameObject.SetActive(true);
                shieldDirLeftIdx++;
                shieldDirLeftIdx = shieldDirLeftIdx <= 7 ? shieldDirLeftIdx : 0;
            }
            if (!transform.GetChild(shieldDirRightIdx).gameObject.activeSelf)
            {
                //Debug.Log(shieldDirRightIdx);
                transform.GetChild(shieldDirRightIdx).gameObject.SetActive(true);
                shieldDirRightIdx--;
                shieldDirRightIdx = shieldDirRightIdx >= 0 ? shieldDirRightIdx : 7; 
            }
            deployDelay = delay;
        }
    }

    private void enableDir(string dirName)
    {
        foreach(Transform shieldDir in transform)
        {
            if (shieldDir.name != dirName)
                shieldDir.gameObject.SetActive(false);
            else
                shieldDir.gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float damage = GrowthRate * player.Currentlevel + BaseDamage;
        float currDmg = damage;

        if (collision.gameObject.CompareTag("EnemyMelee"))
        {
            collision.gameObject.GetComponent<Enemy2>().takeDamage(currDmg, collision.transform, 10);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy1>() != null)
                collision.gameObject.GetComponent<Enemy1>().takeDamage(currDmg, collision.transform, 10);
            else
                collision.gameObject.GetComponent<Enemy3>().takeDamage(currDmg, collision.transform, 10);
        }
        if (collision.gameObject.CompareTag("Colony")) { collision.gameObject.GetComponent<EnemyColony>().takeDamage(currDmg, collision.transform, 10); }
        if (collision.gameObject.CompareTag("Globin")) { collision.gameObject.GetComponent<Globin>().takeDamage(currDmg, collision.transform, 10); }
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(player.Stats.Angle * Mathf.Deg2Rad), Mathf.Sin(player.Stats.Angle * Mathf.Deg2Rad)) * knockBackForce, ForceMode2D.Impulse);
        }
        if (collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("EnemyBullet2"))
        {
            if (collision.gameObject.CompareTag("EnemyBullet"))
            {
                collision.gameObject.GetComponent<EnemyProj>().DestroyEnemyProj();

            }
            else
            {
                collision.gameObject.GetComponent<EnemyProj2>().DestroyEnemyProj();
            }
        }
    }
}
