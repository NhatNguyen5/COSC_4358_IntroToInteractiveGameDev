using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpEff : MonoBehaviour
{
    private Player player;
    ParticleSystem LevelUpEffPS;
    //ParticleSystem.MainModule LevelUpEffPSMain;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //Debug.Log("Spawn Armor down at " + player.Stats.ArmorLevel);
        LevelUpEffPS = transform.GetComponent<ParticleSystem>();
        //LevelUpEffPSMain = LevelUpEffPS.main;

        LevelUpEffPS.Play();
        StartCoroutine(StopEff(LevelUpEffPS.main.startLifetime.constant));
        Destroy(gameObject, LevelUpEffPS.main.startLifetime.constant * 2);
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
        LevelUpEffPS.Stop();
    }
}
