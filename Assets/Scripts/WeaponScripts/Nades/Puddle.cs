using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    public string PuddleType;
    public float explodeRadius;
    public float spreadSize;
    public float fullDamageDuration;
    public float puddleFadeDuration;
    public float spreadSpeed;
    public float initialExplodeDamage;
    public float damageOverTime;

    private float currDamage;
    
    public float durationBetweenBurn;
    private bool isWaiting = false;
    private CircleCollider2D cc2d;
    private ParticleSystem.ShapeModule PSShapeModule;
    private ParticleSystem.EmissionModule PSEmissionModule;

    private float scaleUp = 0.1f;
    private float scaleDown;

    private Transform PuddleShadow;
    private float OriShaTrans;

    private Vector2 PudShaOriSca;

    private bool firstExplotion = true;
    // Start is called before the first frame update
    private void Start()
    {
        PuddleShadow = transform.Find("Sprite");
        PudShaOriSca = PuddleShadow.localScale;
        OriShaTrans = PuddleShadow.GetComponent<SpriteRenderer>().color.a;
        PuddleShadow.localScale *= 0;
        cc2d = transform.GetComponent<CircleCollider2D>();
        cc2d.radius = 0;
        PSShapeModule = transform.Find("Particle").GetComponent<ParticleSystem>().shape;
        PSShapeModule.radius = 0;
        PSEmissionModule = transform.Find("Particle").GetComponent<ParticleSystem>().emission;
        PSEmissionModule.rateOverTime = 0;
        currDamage = initialExplodeDamage;
        scaleDown = fullDamageDuration + puddleFadeDuration;
        StartCoroutine(clearPuddle(fullDamageDuration + puddleFadeDuration));
    }

    // Update is called once per frame
    private void Update()
    {
        if (scaleUp < explodeRadius && firstExplotion)
        {
            PSEmissionModule.rateOverTime = explodeRadius * 5000 * scaleUp;
            scaleUp += Time.deltaTime * 10;
            //cc2d.radius = scaleUp;
            PSShapeModule.radius = (0.75f * explodeRadius) * scaleUp;
            PuddleShadow.localScale = PudShaOriSca * scaleUp;
        }
        else if(scaleDown <= puddleFadeDuration)
        {
            scaleDown -= Time.deltaTime;
            // Uncomment this if you want fade effect
            Color tempColor = PuddleShadow.GetComponent<SpriteRenderer>().color;
            tempColor.a = OriShaTrans * scaleDown / puddleFadeDuration;
            PuddleShadow.GetComponent<SpriteRenderer>().color = tempColor;
            PSEmissionModule.rateOverTime = 300 * explodeRadius * scaleDown / puddleFadeDuration;
            currDamage = (int)(damageOverTime * scaleDown / puddleFadeDuration);
        }
        else
        {
            scaleDown -= Time.deltaTime;
        }
        //Debug.Log(300 * puddleSize * scaleDown / puddleFadeDuration);

        if(scaleUp < spreadSize && scaleUp > explodeRadius)
        {
            scaleUp += Time.deltaTime * spreadSpeed/5;
            //cc2d.radius = scaleUp;
            PSShapeModule.radius = scaleUp;
            PuddleShadow.localScale = PudShaOriSca * scaleUp;
            //Debug.Log("Spreading");
        }

        if(scaleUp >= explodeRadius && firstExplotion)
        {
            currDamage = damageOverTime;
            firstExplotion = false;
            PSShapeModule.radius = scaleUp;
            PSEmissionModule.rateOverTime = 300 * scaleUp;
        }


        if (!isWaiting)
        {
            if (scaleUp < explodeRadius)
                burn(explodeRadius);
            else
                burn(scaleUp);
            StartCoroutine(wait(durationBetweenBurn));
        }
    }

    private void burn(float size)
    {
        Collider2D[] objectsToBurn = Physics2D.OverlapCircleAll(transform.position, size);
        foreach (var objectToBurn in objectsToBurn)
        {
            if (objectToBurn.tag == "EnemyMelee") { objectToBurn.GetComponent<Enemy2>().takeDamage(currDamage, objectToBurn.transform, 10); }
            if (objectToBurn.tag == "Enemy") {
                if (objectToBurn.GetComponent<Enemy1>() != null)
                    objectToBurn.GetComponent<Enemy1>().takeDamage(currDamage, objectToBurn.transform, 10); 
                else
                    objectToBurn.GetComponent<Enemy3>().takeDamage(currDamage, objectToBurn.transform, 10);
            }
            if (objectToBurn.tag == "Colony") {

                if (objectToBurn.GetComponent<EnemyColony>() != null)
                    objectToBurn.GetComponent<EnemyColony>().takeDamage(currDamage, objectToBurn.transform, 10);
                else
                    objectToBurn.GetComponent<EnemyColony2>().takeDamage(currDamage, objectToBurn.transform, 10);
            }
            if (objectToBurn.tag == "Player") { objectToBurn.GetComponent<TakeDamage>().takeDamage(currDamage, objectToBurn.transform, 10); }
            if (objectToBurn.tag == "Globin") { objectToBurn.GetComponent<Globin>().takeDamage(currDamage, objectToBurn.transform, 10); }
        }
    }

    private IEnumerator wait(float duration)
    {
        isWaiting = true;
        yield return new WaitForSeconds(duration);
        isWaiting = false;
    }

    private IEnumerator clearPuddle(float AfterDuration)
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
