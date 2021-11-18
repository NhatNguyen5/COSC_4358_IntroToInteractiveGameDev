using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectPool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf == true)
        {
            transform.parent = ObjectPool.instance.transform;
            StartCoroutine("DestroyEffect");
        }
    }


    IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(.2f);
        ObjectPool.instance.ReturnBulletEffectToPool(gameObject);
    }


}
