using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VolumeController : MonoBehaviour
{
    public static VolumeController Instance;

    public float _musicVolume;
    public float _SFXVolume;
    public UnityEvent _volumeUpdate = new UnityEvent();

    private void Awake()
    {
        if (Instance == null)
        {
            VolumeController.Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMusicVolumeTo(float volume)
    {
        _musicVolume = volume;
    }

    public void SetSFXVolumeTo(float volume)
    {
        _SFXVolume = volume;
    }
}
