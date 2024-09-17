using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] _musicSource;       
    public AudioSource[] _soundEffectSources; 
    public Slider _musicVolumeSlider;      
    public Slider _sfxVolumeSlider;        

    void Start()
    {
        SetMusicVolume(_musicVolumeSlider.value);
        SetSFXVolume(_sfxVolumeSlider.value);

        _musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        foreach (AudioSource sfxSource in _musicSource)
        {
            sfxSource.volume = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        foreach (AudioSource sfxSource in _soundEffectSources)
        {
            sfxSource.volume = volume;
        }
    }
}
