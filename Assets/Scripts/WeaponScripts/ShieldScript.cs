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
    public float deploySpeed;
    [HideInInspector]
    public bool deploy = false;

    private int numDep = 1;

    private float deployDelay;
    private bool doneDeploy = false;
    private bool doneUndeploy = true;

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
            if (firingDelay > 0)
            {
                firingDelay -= Time.deltaTime;
            }
            else
            {
                firingDelay = 0;
            }
            //Debug.Log(doneDeploy);
            animCtrl.SetFloat("DeploySpeed", deploySpeed);
            if (Input.GetKeyDown(KeyCode.Alpha4) && firingDelay == 0)
            {
                if (!deploy)
                {
                    deploy = true;
                    doneDeploy = false;
                }
                else
                {
                    deploy = false;
                    doneUndeploy = false;
                }
                firingDelay = delay * 8;
            }

            if (deploy)
            {
                transform.position = player.transform.position;
                GlobalPlayerVariables.weaponWeight = 1;
                if (doneDeploy)
                {
                    if(player.Stats.Armorz < 780)
                        player.Stats.Armorz += 20*Time.deltaTime;
                    else
                        player.Stats.Armorz = 800;
                    getOppoDir();
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
                    deployShield();
                }
            }
            else
            {
                if (doneUndeploy)
                {
                    getDir();
                    animCtrl.SetBool("ShieldDeploy", false);
                    animateThis();
                    GlobalPlayerVariables.weaponWeight = weaponWeight;
                }
                else
                {
                    undeployShield();
                }
            }
        }
        
        //Debug.Log(animCtrl.GetCurrentAnimatorStateInfo(0).IsName("1stSwingAnim"));
        //Debug.Log(swingCount);
    }

    private void animateThis()
    {   
        enableDir(direction[shieldDirIdx]);
        //Debug.Log(direction[shieldDirLeftIdx > 7 ? 0 : shieldDirLeftIdx]);
    }

    private void getDir()
    {
        float relaMouseAngle = player.Stats.Angle;

        if (relaMouseAngle < 0)
            relaMouseAngle = relaMouseAngle + 360;
        shieldDirIdx = Mathf.FloorToInt((relaMouseAngle + 22.5f) / 45);
        shieldDirIdx = shieldDirIdx > 7 ? 0 : shieldDirIdx;
        shieldDirRightIdx = shieldDirIdx - 1 >= 0 ? shieldDirIdx - 1 : 7;
        shieldDirLeftIdx = shieldDirIdx + 1 <= 7 ? shieldDirIdx + 1 : 0;
    }

    private void getOppoDir()
    {
        float relaMouseAngle = player.Stats.Angle;

        if (relaMouseAngle < 0)
            relaMouseAngle = relaMouseAngle + 360;

        shieldDirIdx = Mathf.FloorToInt((relaMouseAngle + 22.5f) / 45);
        shieldDirIdx = shieldDirIdx > 7 ? 0 : shieldDirIdx;
        shieldDirIdx = shieldDirIdx < 4 ? shieldDirIdx + 4 : shieldDirIdx - 4;
        shieldDirRightIdx = shieldDirIdx - 1 >= 0 ? shieldDirIdx - 1 : 7;
        shieldDirLeftIdx = shieldDirIdx + 1 <= 7 ? shieldDirIdx + 1 : 0;
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
            {
                finishDeploy = false;
                break;
            }
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
        else if(finishDeploy && deployDelay == 0)
        {
            doneDeploy = true;
        }
    }

    private void undeployShield()
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
        //Debug.Log(deployDelay);
        int noOfActive = 0;
        foreach (Transform shieldDir in transform)
        {
            if (shieldDir.gameObject.activeSelf)
            {
                noOfActive++;
            }
        }
        if(noOfActive > 1)
        {
            finishDeploy = false;
        }
        Debug.Log(finishDeploy);

        if (!finishDeploy && deployDelay == 0)
        {
            if (transform.GetChild(shieldDirIdx).gameObject.activeSelf)
            {
                transform.GetChild(shieldDirIdx).gameObject.SetActive(false);
            }
            if (transform.GetChild(shieldDirLeftIdx).gameObject.activeSelf)
            {
                //Debug.Log(shieldDirLeftIdx);
                transform.GetChild(shieldDirLeftIdx).gameObject.SetActive(false);
                shieldDirLeftIdx++;
                shieldDirLeftIdx = shieldDirLeftIdx <= 7 ? shieldDirLeftIdx : 0;
            }
            if (transform.GetChild(shieldDirRightIdx).gameObject.activeSelf)
            {
                //Debug.Log(shieldDirRightIdx);
                transform.GetChild(shieldDirRightIdx).gameObject.SetActive(false);
                shieldDirRightIdx--;
                shieldDirRightIdx = shieldDirRightIdx >= 0 ? shieldDirRightIdx : 7;
            }
            deployDelay = delay;
        }
        else if (finishDeploy && deployDelay == 0)
        {
            doneUndeploy = true;
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
        if (collision.gameObject.CompareTag("Colony")) {
            if (collision.gameObject.GetComponent<EnemyColony>() != null)
                collision.gameObject.GetComponent<EnemyColony>().takeDamage(currDmg, collision.transform, 10);
            else
                collision.gameObject.GetComponent<EnemyColony2>().takeDamage(currDmg, collision.transform, 10);
            //collision.gameObject.GetComponent<EnemyColony>().takeDamage(currDmg, collision.transform, 10); 
        }
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
                collision.gameObject.GetComponent<EnemyProj>().isDeflected = false;
            }
            else
            {
                collision.gameObject.GetComponent<EnemyProj2>().DestroyEnemyProj();
                collision.gameObject.GetComponent<EnemyProj2>().isDeflected = false;
            }
        }
    }
}
