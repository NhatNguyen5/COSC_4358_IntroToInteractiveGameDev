using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private GameObject EnemyBullets;
    private Queue<GameObject> EnemyBulletPool = new Queue<GameObject>();
    [SerializeField] private int BulletPoolSizeStart = 10;


    [SerializeField] private GameObject EnemyBulletsEffect;
    private Queue<GameObject> EnemyBulletEffectPool = new Queue<GameObject>();
    [SerializeField] private int BulletPoolEffectSizeStart = 10;

    
    [SerializeField] private GameObject DamagePopUp;
    private Queue<GameObject> DamagePopUpPool = new Queue<GameObject>();
    [SerializeField] private int DamagePopUpSizeStart = 10;
    


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < BulletPoolSizeStart; i++)
        {
            GameObject mosaicBullet = Instantiate(EnemyBullets);
            EnemyBulletPool.Enqueue(mosaicBullet);
            mosaicBullet.transform.parent = gameObject.transform;
            mosaicBullet.SetActive(false);
        }

        for (int i = 0; i < BulletPoolEffectSizeStart; i++)
        {
            GameObject bulletParticle = Instantiate(EnemyBulletsEffect);
            EnemyBulletEffectPool.Enqueue(bulletParticle);
            bulletParticle.transform.parent = gameObject.transform;
            bulletParticle.SetActive(false);
        }

        for (int i = 0; i < DamagePopUpSizeStart; i++)
        {
            GameObject dmgPopUp = Instantiate(DamagePopUp);
            DamagePopUpPool.Enqueue(dmgPopUp);
            dmgPopUp.transform.SetParent(gameObject.transform);
            dmgPopUp.SetActive(false);
        }


    }

    public GameObject GetBulletFromPool()
    {
        if (EnemyBulletPool.Count > 0)
        {
            GameObject Bullet = EnemyBulletPool.Dequeue();
            Bullet.GetComponent<TrailRenderer>().Clear();
            Bullet.SetActive(true);
            return Bullet;
        }
        else
        {
            GameObject bullet = Instantiate(EnemyBullets);
            bullet.transform.parent = gameObject.transform;
            return null;
        }
    }

    public GameObject GetBulletEffectFromPool()
    {
        if (EnemyBulletEffectPool.Count > 0)
        {
            GameObject Effect = EnemyBulletEffectPool.Dequeue();
            Effect.SetActive(true);
            return Effect;
        }
        else
        {
            GameObject Effect = Instantiate(EnemyBulletsEffect);
            Effect.transform.SetParent(gameObject.transform);
            return null;
        }
    }

    public GameObject GetDamagePopUpFromPool()
    {
        if (DamagePopUpPool.Count > 0)
        {
            GameObject popUp = DamagePopUpPool.Dequeue();
            popUp.SetActive(true);
            return popUp;
        }
        else
        {
            GameObject popUp = Instantiate(DamagePopUp);
            popUp.transform.SetParent(gameObject.transform);
            return null;
        }
    }







    public void ReturnBulletToPool(GameObject bullet)
    {
        EnemyBulletPool.Enqueue(bullet);
        bullet.SetActive(false);
    }
    public void ReturnBulletEffectToPool(GameObject effect)
    {
        EnemyBulletEffectPool.Enqueue(effect);
        effect.SetActive(false);
    }

    public void ReturnDamagePopUpToPool(GameObject popUp)
    {
        DamagePopUpPool.Enqueue(popUp);
        popUp.SetActive(false);
    }


}
