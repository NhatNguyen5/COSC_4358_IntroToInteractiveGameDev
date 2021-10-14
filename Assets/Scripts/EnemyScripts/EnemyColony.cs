using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyColony : MonoBehaviour
{
    public GameObject DamageText;
    private SpriteRenderer sprite;
    public Text BossName;
    public Image HealthBar;
    public string NameOfEnemy;
    //DEATH VARIABLE
    [HideInInspector]
    public bool isDead = false;

    [Header("Enemy Stats")]

    public float contactDamage;
    private EnemyManager enemyColony;
    private float MaxHP = 0;
    public float speed = 0;

    [Header("Drops")]
    public GameObject[] Drops;
    public float DropPercentageTylenol;
    public int NumOfTylenolDrop;
    public float DropPercentageProtein;
    public int NumOfProteinDrop;
    public float DropPercentageAmmo;
    public int NumOfAmmoDrop;
    public float DropPercentagePhizer;
    public int NumOfPhizerDrop;

    // Start is called before the first frame update
    void Start()
    {
        BossName.text = NameOfEnemy;
        enemyColony = transform.parent.transform.parent.GetComponent<EnemyManager>();
        //Debug.Log(enemyColony.colonyHealth);
        MaxHP = enemyColony.colonyHealth;
        //HealthBar = GameObject.Find("EnemyHP").GetComponent<Image>();
        //BossName = GameObject.Find("BossName").GetComponent<Text>();
        sprite = transform.Find("BossSprite").GetComponent<SpriteRenderer>();
        HealthBar.fillAmount = enemyColony.colonyHealth / MaxHP;

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    





    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {




        if (collision.tag == "Bullet")
        {
            float damage = collision.gameObject.GetComponent<Bullet>().damage;
            float speed = collision.gameObject.GetComponent<Bullet>().speed;
            

            takeDamage(damage, collision.transform, speed);

            Destroy(collision.gameObject);

        }



    }

    public void takeDamage(float damage, Transform impact, float speed)
    {
        //Debug.Log(damage);
        bool iscrit = false;
        float chance2crit = Random.Range(0f, 1f);
        if (chance2crit <= GlobalPlayerVariables.critRate)
        {
            iscrit = true;
            damage *= GlobalPlayerVariables.critDmg;
        }

        enemyColony.colonyHealth -= damage;
        HealthBar.fillAmount = enemyColony.colonyHealth / MaxHP;
        showDamage(damage, impact, speed, iscrit);
        StartCoroutine(FlashRed());
        if (enemyColony.colonyHealth <= 0)
        {
            Die();
        }
    }


    void showDamage(float damage, Transform impact, float speed, bool crit)
    {
        damage = Mathf.Round(damage);
        if (damage > 1)
        {
            Vector3 direction = (transform.position - impact.transform.position).normalized;

            //might add to impact to make it go past enemy
            var go = Instantiate(DamageText, impact.position, Quaternion.identity);
            if (crit == false)
            {
                go.GetComponent<TextMeshPro>().text = damage.ToString();
            }
            else
            {
                //Debug.Log("CRIT");

                Color colorTop = new Color(0.83529f, 0.06667f, 0.06667f);
                Color colorBottom = new Color(0.98824f, 0.33725f, .90196f);

                go.GetComponent<TextMeshPro>().text = damage.ToString();
                go.GetComponent<TextMeshPro>().colorGradient = new VertexGradient(colorTop, colorTop, colorBottom, colorBottom);
                go.GetComponent<TextMeshPro>().fontSize *= 1.2f;
            }
            go.GetComponent<DestroyText>().spawnPos(direction.x, direction.y, speed / 5);
        }
    }


    public IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }




    void Die()
    {
        if (isDead == false)
        {
            isDead = true;
            transform.Find("BossSprite").GetComponent<Animator>().SetBool("IsDead", isDead);
            GetComponent<PolygonCollider2D>().enabled = false;
            StartCoroutine(Dying());
            //GameObject.Destroy(gameObject);
        }
        
    }

    IEnumerator Dying()
    {
        yield return new WaitForSecondsRealtime(2.75f);
        if (Random.Range(0, 100) <= DropPercentageTylenol)
        {
            for (int i = 0; i < NumOfTylenolDrop; i++)
                Instantiate(Drops[0], transform.position, Quaternion.Euler(0, 0, 0));
        }

        if (Random.Range(0, 100) <= DropPercentageProtein)
        {
            for (int i = 0; i < NumOfProteinDrop; i++)
                Instantiate(Drops[1], transform.position, Quaternion.Euler(0, 0, 0));
        }

        if (Random.Range(0, 100) <= DropPercentageAmmo)
        {
            for (int i = 0; i < NumOfAmmoDrop; i++)
                Instantiate(Drops[2], transform.position, Quaternion.Euler(0, 0, 0));
        }

        if (Random.Range(0, 100) <= DropPercentagePhizer)
        {
            for (int i = 0; i < NumOfPhizerDrop; i++)
                Instantiate(Drops[3], transform.position, Quaternion.Euler(0, 0, 0));
        }

        Destroy(transform.parent.gameObject);
    }




}
