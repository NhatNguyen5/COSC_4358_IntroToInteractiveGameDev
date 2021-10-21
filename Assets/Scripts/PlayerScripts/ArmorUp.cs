using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorUp : MonoBehaviour
{
    private Player player;
    ParticleSystem ArmorUpPS;
    ParticleSystem.MainModule ArmorUpPSMain;
    ParticleSystem.EmissionModule ArmorUpPSEmission;
    ParticleSystem.ColorOverLifetimeModule ArmorUpCOL;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        ArmorUpPS = transform.GetComponent<ParticleSystem>();
        ArmorUpPSMain = ArmorUpPS.main;
        ArmorUpPSEmission = ArmorUpPS.emission;
        ArmorUpPSEmission.rateOverTime = player.Stats.ArmorLevel * 40;
        ArmorUpCOL = ArmorUpPS.colorOverLifetime;
        switch (player.Stats.ArmorLevel)
        {
            case 1:
                ArmorUpPSMain.startColor = Color.white;
                break;
            case 2:
                ArmorUpCOL.color = new ParticleSystem.MinMaxGradient(Color.white, new Color(55 / 255f, 195 / 255f, 255 / 255f));
                break;
            case 3:
                ArmorUpCOL.color = new ParticleSystem.MinMaxGradient(new Color(55 / 255f, 195 / 255f, 255 / 255f), new Color(166 / 255f, 50 / 255f, 168 / 255f));
                break;
            case 4:
                ArmorUpCOL.color = new ParticleSystem.MinMaxGradient(new Color(166 / 255f, 50 / 255f, 168 / 255f), Color.yellow);
                break;
                /*
            case 5:
                ARBar.color = Color.green;
                break;*/
        }
        ArmorUpPS.Play();
        StartCoroutine(StopEff(ArmorUpPS.main.startLifetime.constant));
        Destroy(gameObject, ArmorUpPS.main.startLifetime.constant * 2);
    }

    // Update is called once per frame
    private void Update()
    {
        if(transform != null)
        {
            transform.position = player.Stats.Position - new Vector2(0, 0.6f);
        }
    }

    private IEnumerator StopEff(float dur)
    {
        yield return new WaitForSeconds(dur);
        ArmorUpPS.Stop();
    }

}
