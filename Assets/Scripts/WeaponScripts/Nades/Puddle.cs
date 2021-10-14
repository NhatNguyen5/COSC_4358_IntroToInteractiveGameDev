using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    public string PuddleType;
    public float puddleSize;
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
    void Start()
    {
        PuddleShadow = transform.Find("Sprite");
        PudShaOriSca = PuddleShadow.localScale;
        OriShaTrans = PuddleShadow.GetComponent<SpriteRenderer>().color.a;
        PuddleShadow.localScale *= 0;
        cc2d = transform.GetComponent<CircleCollider2D>();
        cc2d.radius = 0;
        PSShapeModule = transform.GetComponent<ParticleSystem>().shape;
        PSShapeModule.radius = 0;
        PSEmissionModule = transform.GetComponent<ParticleSystem>().emission;
        PSEmissionModule.rateOverTime = 0;
        currDamage = initialExplodeDamage;
        scaleDown = fullDamageDuration + puddleFadeDuration;
        StartCoroutine(clearPuddle(fullDamageDuration + puddleFadeDuration));
    }

    // Update is called once per frame
    void Update()
    {
        if (scaleUp < puddleSize && firstExplotion)
        {
            PSEmissionModule.rateOverTime = puddleSize * 5000 * scaleUp;
            scaleUp += Time.deltaTime * spreadSpeed;
            //cc2d.radius = scaleUp;
            PSShapeModule.radius = (0.75f * puddleSize) * scaleUp;
            PuddleShadow.localScale = PudShaOriSca * scaleUp;
        }
        else if(scaleDown <= puddleFadeDuration)
        {
            scaleDown -= Time.deltaTime;
            // Uncomment this if you want fade effect
            Color tempColor = PuddleShadow.GetComponent<SpriteRenderer>().color;
            tempColor.a = OriShaTrans * scaleDown / puddleFadeDuration;
            PuddleShadow.GetComponent<SpriteRenderer>().color = tempColor;
            PSEmissionModule.rateOverTime = 300 * puddleSize * scaleDown / puddleFadeDuration;
            currDamage = (int)(damageOverTime * scaleDown / puddleFadeDuration);
        }
        else
        {
            scaleDown -= Time.deltaTime;
        }
        //Debug.Log(300 * puddleSize * scaleDown / puddleFadeDuration);

        if(scaleUp >= puddleSize && firstExplotion)
        {
            currDamage = damageOverTime;
            firstExplotion = false;
            PSShapeModule.radius = scaleUp;
            PSEmissionModule.rateOverTime = 300 * scaleUp;
        }


        if (!isWaiting)
        {
            burn();
            StartCoroutine(wait(durationBetweenBurn));
        }
    }

    private void burn()
    {
        Collider2D[] objectsToBurn = Physics2D.OverlapCircleAll(transform.position, puddleSize);
        foreach (var objectToBurn in objectsToBurn)
        {
            if (objectToBurn.tag == "EnemyMelee") { objectToBurn.GetComponent<Enemy2>().takeDamage(currDamage, objectToBurn.transform, 10); }
            if (objectToBurn.tag == "Enemy") { objectToBurn.GetComponent<Enemy1>().takeDamage(currDamage, objectToBurn.transform, 10); }
            if (objectToBurn.tag == "Colony") { objectToBurn.GetComponent<EnemyColony>().takeDamage(currDamage, objectToBurn.transform, 10); }
            if (objectToBurn.tag == "Player") { objectToBurn.GetComponent<TakeDamage>().takeDamage(currDamage, objectToBurn.transform, 10); }
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
        Destroy(gameObject);
    }

}
