using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MeleeWeapon : MonoBehaviour
{
    
    [Header("Gun Settings")]
    public float Slot;
    public Sprite WeapnIcon;
    public float IconScale = 1;
    public string WeaponLabel;
    public float damage;
    public float delay = 0.2f;
    float firingDelay = 0.0f;
    public float ADSRange;
    public float ADSSpeed;
    public Color trailColor;
    public float swingSpeed;


    public float weaponWeight = 1;

    [HideInInspector]
    public bool hitMelee = false;

    private List<GameObject> slashDetect = new List<GameObject>();
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
    public bool IsRightArm;

    private bool Pullout = false;

    private Player player;
    //public PlayerActions SwapWeapon;
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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

        for(int i = 0; i < 10; i++)
        {
            transform.Find("BladeTrail (" + i + ")").GetComponent<TrailRenderer>().startColor = trailColor;
            transform.Find("BladeTrail (" + i + ")").GetComponent<TrailRenderer>().enabled = false;
        }

        animCtrl = transform.GetComponent<Animator>();
        animCtrl.SetFloat("SwingSpeed", swingSpeed);
    }

    // Update is called once per frame
    private void Update()
    {
        GlobalPlayerVariables.weaponWeight = weaponWeight;

        if(firingDelay > 0)
        {
            firingDelay -= Time.deltaTime;
        }
        else
        {
            firingDelay = 0;
        }
        
        if(animCtrl.GetBool("StopSwing") && animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Standby"))
        {
            for (int i = 0; i < 10; i++)
            {
                transform.Find("BladeTrail (" + i + ")").GetComponent<TrailRenderer>().enabled = false;
            }
        }

        if (OptionSettings.GameisPaused == false && firingDelay == 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animCtrl.SetBool("StopSwing", false);
                for (int i = 0; i < 10; i++)
                {
                    transform.Find("BladeTrail (" + i + ")").GetComponent<TrailRenderer>().enabled = true;
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                animCtrl.SetBool("StopSwing", true);
            }
                /*
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    switch (swingCount)
                    {
                        case 1:
                            animCtrl.SetBool("firstSwing", true);
                            break;
                        case 2:
                            animCtrl.SetBool("secondSwing", true);
                            break;
                        case 3:
                            animCtrl.SetBool("thirdSwing", true);
                            break;
                    }
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    switch (swingCount)
                    {
                        case 1:
                            if (animCtrl.GetBool("firstSwing"))
                            {
                                animCtrl.SetBool("firstSwing", false);
                                firingDelay = delay;
                                swingCount++;
                            }
                            break;
                        case 2:
                            if (animCtrl.GetBool("secondSwing"))
                            {
                                animCtrl.SetBool("secondSwing", false);
                                firingDelay = delay;
                                swingCount++;
                            }
                            break;
                        case 3:
                            if (animCtrl.GetBool("thirdSwing"))
                            {
                                animCtrl.SetBool("thirdSwing", false);
                                firingDelay = delay;
                                swingCount = 1;
                            }
                            break;
                    }
                }
                */
        }

        //Debug.Log(animCtrl.GetCurrentAnimatorStateInfo(0).IsName("1stSwingAnim"));
        //Debug.Log(swingCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyMelee")) 
        { 
            collision.GetComponent<Enemy2>().takeDamage(damage, collision.transform, 10); 
        }
        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<Enemy1>() != null)
                collision.GetComponent<Enemy1>().takeDamage(damage, collision.transform, 10);
            else
                collision.GetComponent<Enemy3>().takeDamage(damage, collision.transform, 10);
        }
        if (collision.CompareTag("Colony")) { collision.GetComponent<EnemyColony>().takeDamage(damage, collision.transform, 10); }
        if (collision.CompareTag("Globin")) { collision.GetComponent<Globin>().takeDamage(damage, collision.transform, 10); }
        if(collision.GetComponent<Rigidbody2D>() != null)
        {
            collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(player.Stats.Angle * Mathf.Deg2Rad), Mathf.Sin(player.Stats.Angle * Mathf.Deg2Rad)) * knockBackForce, ForceMode2D.Impulse);
        }
        if(collision.CompareTag("EnemyBullet") || collision.CompareTag("EnemyBullet2"))
        {
            if (collision.CompareTag("EnemyBullet"))
            {
                collision.GetComponent<EnemyProj>().isDeflected = true;
            }
            else
            {
                collision.GetComponent<EnemyProj2>().isDeflected = true;
            }

            Quaternion newRot = Quaternion.Euler(0, 0, player.Stats.Angle);
            collision.transform.rotation = newRot;

            //Instantiate(collision.gameObject, collision.transform.position, newRot);
            //Destroy(collision.gameObject);
            
        }
    }
}
