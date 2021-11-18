using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{

    public string HitNoise;
    public string ContactNoise;
    public bool isObjectEnemy;
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if(hitInfo.tag == "Bullet" && isObjectEnemy == true)
            AudioManager.instance.PlayEffect(HitNoise);
        if(hitInfo.tag == "EnemyBullet" && isObjectEnemy == false)
            AudioManager.instance.PlayEffect(HitNoise);
    }


    private void OnCollisionEnter2D(Collision2D hitInfo)
    {
        //Debug.Log(hitInfo.gameObject.tag);
        if (hitInfo.gameObject.tag == "Player" && isObjectEnemy == true)
            AudioManager.instance.PlayEffect(ContactNoise);
        if (hitInfo.gameObject.tag == "Enemy" && isObjectEnemy == false)
            AudioManager.instance.PlayEffect(ContactNoise);

    }

}
