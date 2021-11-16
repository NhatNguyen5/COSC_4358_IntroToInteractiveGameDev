using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public string SoundClip;
    public bool SoundEffect;
    public bool PlayOnArrival;
    public bool ResetSoundForNewScene;
    private AudioSource[] allAudioSources;
    public void playSound() //used for button pressing
    {
        if(SoundEffect == true)
            AudioManager.instance.PlayEffect(SoundClip);
        else
            AudioManager.instance.PlayMusic(SoundClip);
    }

    void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }


    private void Start()
    {
        if (ResetSoundForNewScene == true)
        {
            StopAllAudio();
        }
        if (PlayOnArrival == true)
        {
            if(SoundEffect == true)
                AudioManager.instance.PlayEffect(SoundClip);
            else
                AudioManager.instance.PlayMusic(SoundClip);
        }
    }

}
