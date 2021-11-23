using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colony2WPHitBox : MonoBehaviour
{
    public PolygonCollider2D weaponHurtBox;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        //weaponHurtBox.GetComponent<PolygonCollider2D>();
        damage = transform.parent.transform.parent.gameObject.GetComponent<EnemyColony2>().meleeDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Globin")) { collision.GetComponent<Globin>().takeDamage(damage, collision.transform, 10); }
        if (collision.CompareTag("Player")) { collision.GetComponent<TakeDamage>().takeDamage(damage, collision.transform, 10); }
    }
}
