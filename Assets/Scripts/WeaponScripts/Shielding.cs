using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielding : MonoBehaviour
{
    private float GrowthRate;
    private Player player;
    private float BaseDamage;
    private float knockBackForce;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GrowthRate = transform.parent.GetComponent<ShieldScript>().GrowthRate;
        BaseDamage = transform.parent.GetComponent<ShieldScript>().BaseDamage;
        knockBackForce = transform.parent.GetComponent<ShieldScript>().knockBackForce;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float damage = GrowthRate * player.Currentlevel + BaseDamage;
        float currDmg = damage;

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
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(player.Stats.Angle * Mathf.Deg2Rad), Mathf.Sin(player.Stats.Angle * Mathf.Deg2Rad)) * knockBackForce, ForceMode2D.Impulse);
        }
        if (collision.CompareTag("EnemyBullet") || collision.CompareTag("EnemyBullet2"))
        {
            if (collision.CompareTag("EnemyBullet"))
            {
                collision.GetComponent<EnemyProj>().DestroyEnemyProj();

            }
            else
            {
                collision.GetComponent<EnemyProj2>().DestroyEnemyProj();
            }

            //Instantiate(collision.gameObject, collision.transform.position, newRot);
            //Destroy(collision.gameObject);
        }
    }
}
