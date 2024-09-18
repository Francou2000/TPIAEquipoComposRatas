using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolume : MonoBehaviour
{
    AudioSource myAudioSource;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        SetMusicVolume();
        VolumeController.Instance._volumeUpdate.AddListener(SetMusicVolume);
    }

    private void SetMusicVolume()
    {
        myAudioSource.volume = VolumeController.Instance._musicVolume;
    }
}
