using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    public float explodeRadius;
    public float ExplodeDamage;

    private bool isWaiting = false;
    private CircleCollider2D cc2d;
    private ParticleSystem PS;
    private ParticleSystem.ShapeModule PSShapeModule;
    private ParticleSystem.EmissionModule PSEmissionModule;


    private void Start()
    {
        cc2d = GetComponent<CircleCollider2D>();
        cc2d.isTrigger = true;
        cc2d.radius = explodeRadius;
        PS = transform.Find("Particle").GetComponent<ParticleSystem>();
        PSShapeModule = PS.shape;
        PSShapeModule.radius = explodeRadius;
        PSEmissionModule = PS.emission;
        PSEmissionModule.rateOverTimeMultiplier *= 100*explodeRadius;
        StartCoroutine(clearSmoke(PS.main.duration));
    }

    private IEnumerator clearSmoke(float AfterDuration)
    {
        yield return new WaitForSeconds(AfterDuration);
        //detach particle effect so it won't disapear suddenly

        Transform tempPS;
        tempPS = transform.Find("Particle");
        tempPS.GetComponent<ParticleSystem>().Stop();
        tempPS.parent = null;
        Destroy(tempPS.gameObject, tempPS.GetComponent<ParticleSystem>().main.duration);

        Destroy(gameObject);
    }

}
