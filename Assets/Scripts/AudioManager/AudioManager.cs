using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup MusicVolume;

	public AudioMixerGroup SoundEffectVolume;

	public Sound[] SoundEffects;

	public Sound[] Music;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in SoundEffects)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;

			s.source.outputAudioMixerGroup = SoundEffectVolume;
		}

		foreach (Sound s in Music)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;

			s.source.outputAudioMixerGroup = MusicVolume;
		}

	}

	public void PlayEffect(string sound)
	{
		Sound s = Array.Find(SoundEffects, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}


		s.source.Play();
	}

	public void PlayMusic(string sound)
	{
		Sound s = Array.Find(Music, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}


		s.source.Play();
	}

}
