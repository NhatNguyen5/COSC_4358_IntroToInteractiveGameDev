using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TakeDamage : MonoBehaviour
{
    public GameObject DamageText;
    private SpriteRenderer sprite;
    private float maxHP;
    private Image HealthBar;
    [SerializeField]
    StatusIndicator statusIndicator = null;



    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        HealthBar = GameObject.Find("HP").GetComponent<Image>();
        maxHP = GetComponent<Player>().Stats.Health;
        //rb = GetComponent<Rigidbody2D>();
        //sprite = GetComponent<SpriteRenderer>();
        sprite = transform.Find("PlayerSprite").GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
        
        //Debug.Log(HP);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            float contactDamage = collision.gameObject.GetComponent<Enemy1>().contactDamage;
            float speed = collision.gameObject.GetComponent<Enemy1>().speed;
            takeDamage(contactDamage, collision.transform, speed);
        }
        if (collision.gameObject.tag == "EnemyMelee")
        {
            float contactDamage = collision.gameObject.GetComponent<Enemy2>().contactDamage;
            float speed = collision.gameObject.GetComponent<Enemy2>().speed;
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
    }

    void takeDamage(float damage, Transform impact, float speed)
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
        GetComponent<Player>().Stats.Health -= damage;
        float HP = GetComponent<Player>().Stats.Health;
        showDamage(damage, impact, speed, iscrit);
        StartCoroutine(FlashRed());
        statusIndicator.StartFlash(0.25f, ((maxHP - HP)/maxHP)/2, Color.red, 3);
        statusIndicator.ChangeTransparency(((maxHP - HP) / maxHP));

        HealthBar.fillAmount = HP / maxHP;

        if (HP <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("Title");
            //GO TO TITLE OR GAME OVER SCREEN;
        }
    }


    void showDamage(float damage, Transform impact, float speed, bool crit)
    {

        Vector3 direction = (transform.position - impact.transform.position).normalized;

        //might add to impact to make it go past enemy
        var go = Instantiate(DamageText, impact.position, Quaternion.identity);
        if (crit == false)
        {
            go.GetComponent<TextMesh>().text = damage.ToString();
        }
        else
        {
            //Debug.Log("CRIT");
            go.GetComponent<TextMesh>().text = damage.ToString();
            go.GetComponent<TextMesh>().color = Color.red;
            go.GetComponent<TextMesh>().fontSize *= 3;
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
