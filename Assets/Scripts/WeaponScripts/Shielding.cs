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

        if (collision.gameObject.CompareTag("EnemyMelee"))
        {
            collision.gameObject.GetComponent<Enemy2>().takeDamage(currDmg, collision.transform, 10);
            AudioManager.instance.PlayEffect("ShieldHit");
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy1>() != null)
                collision.gameObject.GetComponent<Enemy1>().takeDamage(currDmg, collision.transform, 10);
            else
                collision.gameObject.GetComponent<Enemy3>().takeDamage(currDmg, collision.transform, 10);
            AudioManager.instance.PlayEffect("ShieldHit");
        }
        if (collision.gameObject.CompareTag("Colony"))
        {
            if (collision.gameObject.GetComponent<EnemyColony>() != null)
                collision.gameObject.GetComponent<EnemyColony>().takeDamage(currDmg, collision.transform, 10);
            else
                collision.gameObject.GetComponent<EnemyColony2>().takeDamage(currDmg, collision.transform, 10);
            AudioManager.instance.PlayEffect("ShieldHit");
            //collision.gameObject.GetComponent<EnemyColony>().takeDamage(currDmg, collision.transform, 10); 
        }
        if (collision.gameObject.CompareTag("Globin")) { 
            collision.gameObject.GetComponent<Globin>().takeDamage(currDmg, collision.transform, 10);
            AudioManager.instance.PlayEffect("ShieldHit");
        }

        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(player.Stats.Angle * Mathf.Deg2Rad), Mathf.Sin(player.Stats.Angle * Mathf.Deg2Rad)) * knockBackForce, ForceMode2D.Impulse);
        }
        if (collision.CompareTag("EnemyBullet") || collision.CompareTag("EnemyBullet2"))
        {
            if (collision.CompareTag("EnemyBullet"))
            {
                collision.GetComponent<EnemyProj>().DestroyEnemyProj();
                collision.GetComponent<EnemyProj>().isDeflected = false;
            }
            else
            {
                collision.GetComponent<EnemyProj2>().DestroyEnemyProj();
                collision.GetComponent<EnemyProj2>().isDeflected = false;
            }
            AudioManager.instance.PlayEffect("ShieldHit");
        }
    }
}
