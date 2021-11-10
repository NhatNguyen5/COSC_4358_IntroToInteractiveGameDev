using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TakeDamage : MonoBehaviour
{
    public GameObject HealthDamageText;
    public GameObject ArmorDamageText;
    private SpriteRenderer sprite;
    private float maxHP;
    private Image HealthBar;
    private Player player;
    [SerializeField]
    private StatusIndicator statusIndicator = null;
    [SerializeField]
    private Camera Mcamera;
    [Header("CameraShake")]
    public bool CameraShake;
    public float ShakeDuration;
    public float ShakeIntensity;
    [Header("Death Sounds and Canvas")]
    public string onDeathSound;
    public GameObject gameOverScreen;


    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        GlobalPlayerVariables.GlobinsAndPlayerAlive += 1;
        player = GetComponent<Player>();
        HealthBar = GameObject.Find("HP").GetComponent<Image>();
        //rb = GetComponent<Rigidbody2D>();
        //sprite = GetComponent<SpriteRenderer>();
        sprite = transform.Find("PlayerSprite_HemoDefault").GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {

    }


    // Update is called once per frame
    void Update()
    {
        maxHP = player.Stats.MaxHealth;

        //Debug.Log(HP);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            float contactDamage = 0;
            float speed = 0;
            if (collision.gameObject.GetComponent<Enemy1>() != null)
            {
                contactDamage = collision.gameObject.GetComponent<Enemy1>().contactDamage;
                speed = collision.gameObject.GetComponent<Enemy1>().speed;
            }
            else
            {
                contactDamage = collision.gameObject.GetComponent<Enemy3>().contactDamage;
                speed = collision.gameObject.GetComponent<Enemy3>().speed;
            }
            
            takeDamage(contactDamage, collision.transform, speed);
        }
        if (collision.gameObject.tag == "EnemyMelee")
        {
            float contactDamage = collision.gameObject.GetComponent<Enemy2>().contactDamage;
            float speed = collision.gameObject.GetComponent<Enemy2>().speed;
            takeDamage(contactDamage, collision.transform, speed);
        }
        if (collision.gameObject.tag == "Colony")
        {
            float contactDamage = collision.gameObject.GetComponent<EnemyColony>().contactDamage;
            float speed = collision.gameObject.GetComponent<EnemyColony>().speed;
            takeDamage(contactDamage, collision.transform, speed);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "EnemyBullet")
        {
            float damage = collision.gameObject.GetComponent<EnemyProj>().damage;
            float speed = collision.gameObject.GetComponent<EnemyProj>().speed;


            takeDamage(damage, collision.transform, speed);
        }
        if (collision.tag == "EnemyBullet2")
        {
            float damage = collision.gameObject.GetComponent<EnemyProj2>().damage;
            float speed = collision.gameObject.GetComponent<EnemyProj2>().speed;


            takeDamage(damage, collision.transform, speed);
        }

    }

    public void takeDamage(float damage, Transform impact, float speed)
    {
        //Debug.Log(damage);
        
        bool iscrit = false;
        /*
        float chance2crit = Random.Range(0f, 1f);
        if (chance2crit <= GlobalPlayerVariables.critRate)
        {
            iscrit = true;
            damage *= GlobalPlayerVariables.critDmg;
        }
        */
        //Debug.Log(player.Stats.Armorz);
        if (player.Stats.Armorz > damage)
        {
            float ArmorMitigation = player.Stats.damagereduceperarmorlevel * player.Stats.ArmorLevel;
            float ArmorDamagePop = 0;
            float HealthDamagePop = 0;
            if (damage >= ArmorMitigation)
            {
                ArmorDamagePop = ArmorMitigation;
                HealthDamagePop = damage - ArmorMitigation;
                player.Stats.Armorz -= ArmorMitigation;
                player.Stats.Health -= damage - ArmorMitigation;
                showDamage(ArmorDamagePop, impact, speed, iscrit, ArmorDamageText);
                showDamage(HealthDamagePop, impact, speed, iscrit, HealthDamageText);
            }
            else
            {
                ArmorDamagePop = damage;
                player.Stats.Armorz -= damage;
                showDamage(ArmorDamagePop, impact, speed, iscrit, ArmorDamageText);
            }
            //Debug.Log("Armor: " + ArmorDamagePop + " " + "Health: " + HealthDamagePop);
        }
        else
        {
            player.Stats.Armorz = 0;
            player.Stats.Health -= damage;
            showDamage(damage, impact, speed, iscrit, HealthDamageText);
        }

        float HP = player.Stats.Health;
        StartCoroutine(FlashRed());
        //if ((maxHP - HP) / maxHP >= 0.5)
        //{
            
        statusIndicator.StartFlash(0.25f, ((maxHP - HP) / maxHP), Color.red, ((maxHP - HP) / maxHP)/2f, Color.red, 3);
        //} 
        //statusIndicator.ChangeTransparency((maxHP - HP) / maxHP);
        if(CameraShake)
            statusIndicator.StartShake(Mcamera, ShakeDuration, ShakeIntensity);

        HealthBar.fillAmount = HP / maxHP;

        if (HP <= 0)
        {

            FindObjectOfType<AudioManager>().PlayEffect(onDeathSound);
            GlobalPlayerVariables.GlobinsAndPlayerAlive -= 1;
            Destroy(gameObject);
            //SceneManager.LoadScene("Title");
            //GO TO TITLE OR GAME OVER SCREEN;
            OptionSettings.GameisPaused = true;
            GlobalPlayerVariables.GameOver = true;
            gameOverScreen.SetActive(true);
            //Time.timeScale = 0f;
        }
    }


    void showDamage(float damage, Transform impact, float speed, bool crit, GameObject typeOfDamage)
    {

        Vector3 direction = (transform.position - impact.transform.position).normalized;

        //might add to impact to make it go past enemy
        var go = Instantiate(typeOfDamage, impact.position, Quaternion.identity);
        if (crit == false)
        {
            go.GetComponent<TextMeshPro>().text = damage.ToString();
        }
        else
        {
            //Debug.Log("CRIT");
            go.GetComponent<TextMeshPro>().text = damage.ToString();
            go.GetComponent<TextMeshPro>().color = Color.red;
            go.GetComponent<TextMeshPro>().fontSize *= 1.6f;
        }
        go.GetComponent<DestroyText>().spawnPos(direction.x, direction.y, speed / 5);
    }


    public IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
