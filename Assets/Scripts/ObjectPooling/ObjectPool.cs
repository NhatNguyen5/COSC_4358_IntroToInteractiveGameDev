using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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


    [SerializeField] private GameObject HeartBeatObj;
    private Queue<GameObject> HeartBeatPool = new Queue<GameObject>();
    [SerializeField] private int HeartBeatPoolStartSize = 10;

    private GameObject EnemySpawnRate;

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
        EnemySpawnRate = GameObject.Find("EnemySpawnRateDisplay");
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

        Transform HMBG = EnemySpawnRate.transform.Find("HeartMonitorBG").transform.Find("HeartMonitorLine");
        for (int i = 0; i < HeartBeatPoolStartSize; i++)
        {
            //GameObject hrtBeat;

            GameObject hrtBeat = Instantiate(HeartBeatObj, HMBG);
            //GameObject hrtBeat = Instantiate(HeartBeatObj);
            HeartBeatPool.Enqueue(hrtBeat);
            //hrtBeat.transform.SetParent(gameObject.transform);
            hrtBeat.SetActive(false);
        }



    }

    public GameObject GetBulletFromPool()
    {
        if (EnemyBulletPool.Count > 0)
        {
            GameObject Bullet = EnemyBulletPool.Dequeue();
            Bullet.GetComponent<TrailRenderer>().Clear();
            Bullet.GetComponent<EnemyProj>().isDeflected = false;
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
            //Effect.GetComponent<EnemyProj>().hit = false;
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


    public Color topGrad;
    public Color bottomGrad;

    public GameObject GetDamagePopUpFromPool()
    {
        if (DamagePopUpPool.Count > 0)
        {
            GameObject popUp = DamagePopUpPool.Dequeue();
            popUp.SetActive(true);
            popUp.GetComponent<TextMeshPro>().colorGradient = DamagePopUp.GetComponent<TextMeshPro>().colorGradient;
            popUp.GetComponent<TextMeshPro>().fontSize = DamagePopUp.GetComponent<TextMeshPro>().fontSize;
            return popUp;
        }
        else
        {
            GameObject popUp = Instantiate(DamagePopUp);
            popUp.transform.SetParent(gameObject.transform);
            return popUp;
        }
    }

    public GameObject GetHeartBeatFromPool()
    {
        if (HeartBeatPool.Count > 0)
        {
            GameObject hrtbeat = HeartBeatPool.Dequeue();
            hrtbeat.SetActive(true);
            return hrtbeat;
        }
        else
        {
            GameObject gendBeat;
            Transform HMBG = EnemySpawnRate.transform.Find("HeartMonitorBG").transform.Find("HeartMonitorLine");
            gendBeat = Instantiate(HeartBeatObj, HMBG, false);
            //GameObject hrtBeat = Instantiate(HeartBeatObj);
            //DamagePopUpPool.Enqueue(HeartBeatObj);
            //Transform HMBG = EnemySpawnRate.transform.Find("HeartMonitorBG").transform.Find("HeartMonitorLine");
            //GameObject hrtbeat = Instantiate(HeartBeatObj);
            //hrtbeat.transform.SetParent(gameObject.transform);
            return gendBeat;
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

    public void ReturnHeartBeatToPool(GameObject hrtBeat)
    {
        HeartBeatPool.Enqueue(hrtBeat);
        hrtBeat.SetActive(false);
    }


}
