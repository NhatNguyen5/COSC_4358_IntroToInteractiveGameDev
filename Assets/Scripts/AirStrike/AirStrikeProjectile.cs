using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrikeProjectile : MonoBehaviour
{
    public float distanceToTarget;
    public Transform landingPad;
    public GameObject explosion;
    //public float projectileSpeed;
    //public float secondsToland;
    float t;
    Vector3 startPosition;
    Vector3 target;
    float timeToReachTarget;

    [Header("airStrikeRandomness")]
    public float startTimerRangeForStrikes;
    public float endTimerRangeForStrikes;
    public float strikeTimer;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = target = transform.position;


        strikeTimer = Random.Range(startTimerRangeForStrikes, endTimerRangeForStrikes);
        SetDestination(landingPad.position, strikeTimer);
        float angle = 0;
        float AimDir = 0;
        Vector3 relativePos = landingPad.position - transform.position;
        angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
        AimDir = angle;
        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            //transform.position.y *= -1;
            aimLocalScale.y = -1f;
            transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
            //aimLocalScale.y = -1f * scaleX;
        }
        else
        {
            aimLocalScale.y = +1f;
            //aimLocalScale.y = -1f * scaleX;
            transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        }
        transform.localScale = aimLocalScale;


    }

    public void SetDestination(Vector3 destination, float time)
    {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = time;
        target = destination;
    }

    // Update is called once per frame
    void Update()
    {
        strikeTimer -= Time.deltaTime;
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);


        distanceToTarget = Vector2.Distance(transform.position, landingPad.position);
        if (distanceToTarget <= 0)
        {
            //spawn proj

            Vector3 offset = new Vector3(transform.position.x, transform.position.y, 0);
            Instantiate(explosion, offset, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
