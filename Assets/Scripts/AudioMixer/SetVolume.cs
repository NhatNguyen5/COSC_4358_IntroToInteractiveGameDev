using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Text Textvalue;
    public Slider SlideValueToChange;
    public string NameofMixer;
    
    
    
    public bool MasterVol = false;
    public bool MusicVol = false;
    public bool EffectVol = false;
    
    

    private void Start()
    {
        if (OptionSettings.update == true)
        {
            //set value to what it needs to be through static value
            if (MasterVol)
            {
                Textvalue.text = OptionSettings.MasterVolValue;
                SlideValueToChange.value = OptionSettings.MasterV;
            }
            if (MusicVol)
            {
                Textvalue.text = OptionSettings.MusicVolValue;
                SlideValueToChange.value = OptionSettings.MusicV;
            }
            if (EffectVol)
            {
                Textvalue.text = OptionSettings.EffectVolValue;
                SlideValueToChange.value = OptionSettings.EffectV;
            }
        }
        else
        {
            OptionSettings.MasterVolValue = "100%";
            OptionSettings.MusicVolValue = "100%";
            OptionSettings.EffectVolValue = "100%";
        }



    }


    public void SetLevel(float sliderValue)
    {
        Textvalue.text =  (Mathf.RoundToInt(sliderValue*100)).ToString() + "%";
        mixer.SetFloat(NameofMixer, Mathf.Log10(sliderValue) * 20);

        //update static values to remember settings between scenes
        if (MasterVol)
        {
            OptionSettings.MasterVolValue = Textvalue.text;
            OptionSettings.MasterV = sliderValue;
        }
        if (MusicVol)
        {
            OptionSettings.MusicVolValue = Textvalue.text;
            OptionSettings.MusicV = sliderValue;
        }
        if (EffectVol)
        {
            OptionSettings.EffectVolValue = Textvalue.text;
            OptionSettings.EffectV = sliderValue;
        }

        OptionSettings.update = true;
    }
}
