using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticleCleanUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(sploosh(GetComponent<ParticleSystem>().main.duration));
    }

    private IEnumerator sploosh(float duration)
    {
        yield return new WaitForSeconds(duration);
        GetComponent<ParticleSystem>().Stop();
    }
}
