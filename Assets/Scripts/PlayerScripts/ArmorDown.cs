using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorDown : MonoBehaviour
{
    private Player player;
    ParticleSystem ArmorDownPS;
    ParticleSystem.MainModule ArmorDownPSMain;
    ParticleSystem.EmissionModule ArmorDownPSEmission;
    ParticleSystem.ColorOverLifetimeModule ArmorDownCOL;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Debug.Log("Spawn Armor down at " + player.Stats.ArmorLevel);
        ArmorDownPS = transform.GetComponent<ParticleSystem>();
        ArmorDownPSMain = ArmorDownPS.main;
        ArmorDownPSEmission = ArmorDownPS.emission;
        ArmorDownPSEmission.rateOverTime = (player.Stats.ArmorLevel + 1) * 10;
        ArmorDownCOL = ArmorDownPS.colorOverLifetime;
        switch (player.Stats.ArmorLevel)
        {
            case 0:
                ArmorDownPSMain.startColor = Color.white;
                break;
            case 1:
                ArmorDownCOL.color = new ParticleSystem.MinMaxGradient(new Color(55 / 255f, 195 / 255f, 255 / 255f), Color.white);
                break;
            case 2:
                ArmorDownCOL.color = new ParticleSystem.MinMaxGradient(new Color(166 / 255f, 50 / 255f, 168 / 255f), new Color(55 / 255f, 195 / 255f, 255 / 255f));
                break;
            case 3:
                ArmorDownCOL.color = new ParticleSystem.MinMaxGradient(Color.yellow, new Color(166 / 255f, 50 / 255f, 168 / 255f));
                break;
                /*
            case 5:
                ARBar.color = Color.green;
                break;*/
        }
        ArmorDownPS.Play();
        StartCoroutine(StopEff(ArmorDownPS.main.startLifetime.constant));
        Destroy(gameObject, ArmorDownPS.main.startLifetime.constant * 2);
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform != null)
        {
            transform.position = player.Stats.Position;
        }
    }

    private IEnumerator StopEff(float dur)
    {
        yield return new WaitForSeconds(dur);
        ArmorDownPS.Stop();
    }
}
