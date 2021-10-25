using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpEff : MonoBehaviour
{
    private Player player;
    ParticleSystem LevelUpEffPS;
    ParticleSystem.MainModule LevelUpEffPSMain;
    private float scale = 1;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //Debug.Log("Spawn Armor down at " + player.Stats.ArmorLevel);
        LevelUpEffPS = transform.GetComponent<ParticleSystem>();
        LevelUpEffPSMain = LevelUpEffPS.main;

        LevelUpEffPS.Play();
        StartCoroutine(StopEff(LevelUpEffPS.main.startLifetime.constant));
        Destroy(gameObject, LevelUpEffPS.main.startLifetime.constant * 2);
    }

    // Update is called once per frame
    private void Update()
    {
        float angle = player.Stats.Angle;
        if (angle < 0)
            angle += 360;
        //Debug.Log(scale);
        if (transform != null)
        {
            transform.position = player.Stats.Position - new Vector2(0, 0.3f);
            if (angle <= 22.5 || angle > 337.5 || (angle > 157.5 && angle <= 202.5)) //Right, Left
            {
                transform.localScale = new Vector3(1, 0.6f, 1);
            }
            else if ((angle > 22.5 && angle <= 67.5) || (angle > 112.5 && angle <= 157.5) || (angle > 202.5 && angle <= 247.5) || (angle > 292.5 && angle <= 337.5)) //other
            {
                transform.localScale = new Vector3(2f, 0.6f, 1);
            }
            else if ((angle > 67.5 && angle <= 112.5) || (angle > 247.5 && angle <= 292.5)) //Up, Down
            {
                transform.localScale = new Vector3(3f, 0.6f, 1);
            }
        }
    }

    private IEnumerator StopEff(float dur)
    {
        yield return new WaitForSeconds(dur);
        LevelUpEffPS.Stop();
    }
}
