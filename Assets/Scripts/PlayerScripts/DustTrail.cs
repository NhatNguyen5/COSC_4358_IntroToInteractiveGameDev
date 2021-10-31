using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustTrail : MonoBehaviour
{
    [SerializeField]
    private Player player;
    private ParticleSystem trailPS;
    private Quaternion keepRot;

    // Start is called before the first frame update
    private void Start()
    {
        trailPS = transform.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (player != null)
        {
            transform.position = player.Stats.Position - new Vector2(0, 0.4f);
            if(player.Stats.Direction.magnitude != 0)
                keepRot = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(player.Stats.Direction.y, player.Stats.Direction.x) * Mathf.Rad2Deg - 180));
            transform.rotation = keepRot;
            //Debug.Log(player.easeRef);
            //Debug.Log(Mathf.Atan2(player.Stats.Direction.y, player.Stats.Direction.x) * Mathf.Rad2Deg);
            //player.Components.PlayerRidgitBody.velocity.magnitude
            if (player.easeRef > 0)
            {
                if (trailPS.isStopped)
                    trailPS.Play();

                ParticleSystem.MainModule trailPSMain = trailPS.main;
                ParticleSystem.EmissionModule trailPSEmission = trailPS.emission;
                trailPSMain.startSpeed = player.Stats.Speed / player.Stats.WalkSpeed;
                trailPSEmission.rateOverTime = 30 * player.Stats.Speed / player.Stats.WalkSpeed;
                if(!GlobalPlayerVariables.EnablePlayerControl)
                    if (trailPS.isPlaying)
                        trailPS.Stop();
            }
            else
            {
                if (trailPS.isPlaying)
                    trailPS.Stop();
            }
        }
    }
}
