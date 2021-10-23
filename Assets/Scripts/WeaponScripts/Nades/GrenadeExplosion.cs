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
    private ParticleSystem.MainModule PSMain;
    private ParticleSystem.ShapeModule PSShape;
    private ParticleSystem.EmissionModule PSEmission;


    private void Start()
    {
        cc2d = GetComponent<CircleCollider2D>();
        cc2d.isTrigger = true;
        cc2d.radius = explodeRadius;
        PS = transform.Find("Particle").GetComponent<ParticleSystem>();
        transform.Find("Sprite").transform.localScale *= explodeRadius;
        PSShape = PS.shape;
        PSShape.radius = explodeRadius;
        PSEmission = PS.emission;
        PSEmission.rateOverTimeMultiplier *= 100*explodeRadius;
        PSMain = PS.main;
        PSMain.startSpeed = 1;

        Collider2D[] CaughtObjects = Physics2D.OverlapCircleAll(transform.position, explodeRadius);
        foreach (var CaughtObject in CaughtObjects)
        {
            if (CaughtObject.tag == "EnemyMelee") { CaughtObject.GetComponent<Enemy2>().takeDamage(ExplodeDamage, CaughtObject.transform, 10); }
            if (CaughtObject.tag == "Enemy") { CaughtObject.GetComponent<Enemy1>().takeDamage(ExplodeDamage, CaughtObject.transform, 10); }
            if (CaughtObject.tag == "Colony") { CaughtObject.GetComponent<EnemyColony>().takeDamage(ExplodeDamage, CaughtObject.transform, 10); }
            if (CaughtObject.tag == "Player") { CaughtObject.GetComponent<TakeDamage>().takeDamage(ExplodeDamage, CaughtObject.transform, 10); }
        }
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
        tempPS = transform.Find("Flaring");
        tempPS.GetComponent<ParticleSystem>().Stop();
        tempPS.parent = null;
        Destroy(tempPS.gameObject, tempPS.GetComponent<ParticleSystem>().main.duration);

        Destroy(gameObject);
    }

}
