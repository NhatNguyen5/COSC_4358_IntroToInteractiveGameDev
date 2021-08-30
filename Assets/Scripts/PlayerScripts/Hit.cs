using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{

    public string HitNoise;
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        FindObjectOfType<AudioManager>().PlayEffect(HitNoise);
    }


}
