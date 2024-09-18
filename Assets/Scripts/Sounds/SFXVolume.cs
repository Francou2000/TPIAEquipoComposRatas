using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXVolume : MonoBehaviour
{
    AudioSource myAudioSource;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        SetSFXVolume();
        VolumeController.Instance._volumeUpdate.AddListener(SetSFXVolume);
    }

    private void SetSFXVolume()
    {
        myAudioSource.volume = VolumeController.Instance._SFXVolume;
    }
}
