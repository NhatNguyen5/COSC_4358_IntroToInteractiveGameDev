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
    public float BaseDamage;
    public float GrowthRate = 1;
    public float BaseSwingSpeed = 1;
    public float delay = 0.2f;
    float firingDelay = 0.0f;
    public float ADSRange;
    public float ADSSpeed;
    public Color trailColor;
    

    public float weaponWeight = 1;

    [HideInInspector]
    public bool hitMelee = false;

    private float damage;
    private float currDmg;
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
        hitBox = transform.GetComponent<BoxCollider2D>();
        if (player.Stats.Angle < 0)
        {
            player.transform.Find("LeftArm").transform.localPosition = new Vector2(0.05f, -0.15f);
            player.transform.Find("RightArm").transform.localPosition = new Vector2(-0.05f, -0.15f);
        }
        else
        {
            player.transform.Find("LeftArm").transform.localPosition = new Vector2(-0.05f, -0.15f);
            player.transform.Find("RightArm").transform.localPosition = new Vector2(0.05f, -0.15f);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        GlobalPlayerVariables.weaponWeight = weaponWeight;
        damage = GrowthRate * player.Currentlevel + BaseDamage;
        animCtrl.SetFloat("SwingSpeed", BaseSwingSpeed + GrowthRate/50 * player.Currentlevel);

        if (firingDelay > 0)
        {
            firingDelay -= Time.deltaTime;
        }
        else
        {
            firingDelay = 0;
        }
        
        if(!animCtrl.GetBool("Blocking") && animCtrl.GetBool("StopSwing") && animCtrl.GetCurrentAnimatorStateInfo(0).IsName("Standby"))
        {
            currDmg = damage / 5;
            for (int i = 0; i < 10; i++)
            {
                transform.Find("BladeTrail (" + i + ")").GetComponent<TrailRenderer>().enabled = false;
            }
        }

        if (OptionSettings.GameisPaused == false && firingDelay == 0)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
            {
                currDmg = damage;
                animCtrl.SetBool("StopSwing", false);
                for (int i = 0; i < 10; i++)
                {
                    transform.Find("BladeTrail (" + i + ")").GetComponent<TrailRenderer>().enabled = true;
                }
            }
            else if(!Input.GetKey(KeyCode.Mouse0))
            {
                animCtrl.SetBool("StopSwing", true);
            }

            if (Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.Mouse0))
            {
                GlobalPlayerVariables.weaponWeight = weaponWeight * 5;
                currDmg = damage / 5;
                hitBox.edgeRadius = 0.3f;
                animCtrl.SetBool("Blocking", true);
                for (int i = 0; i < 10; i++)
                {
                    transform.Find("BladeTrail (" + i + ")").GetComponent<TrailRenderer>().enabled = true;
                }
            }
            else if(!Input.GetKey(KeyCode.Mouse1))
            {
                GlobalPlayerVariables.weaponWeight = weaponWeight;
                hitBox.edgeRadius = 0.2f;
                animCtrl.SetBool("Blocking", false);
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
            collision.GetComponent<Enemy2>().takeDamage(currDmg, collision.transform, 10); 
        }
        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<Enemy1>() != null)
                collision.GetComponent<Enemy1>().takeDamage(currDmg, collision.transform, 10);
            else
                collision.GetComponent<Enemy3>().takeDamage(currDmg, collision.transform, 10);
        }
        if (collision.CompareTag("Colony")) { collision.GetComponent<EnemyColony>().takeDamage(currDmg, collision.transform, 10); }
        if (collision.CompareTag("Globin")) { collision.GetComponent<Globin>().takeDamage(currDmg, collision.transform, 10); }
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
