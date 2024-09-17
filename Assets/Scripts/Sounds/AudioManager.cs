using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;

    public Slider _musicVolumeSlider;      
    public Slider _sfxVolumeSlider;

    public LayerMask _musicLayer;
    public LayerMask _sfxLayer;

    private AudioSource[] _musicSources;      
    private AudioSource[] _soundEffectSources; 

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (_musicVolumeSlider != null)
        {
            _musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (_sfxVolumeSlider != null)
        {
            _sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        FindAudioSourcesInScene(); 
    }

    void FindAudioSourcesInScene()
    {
        GameObject[] musicObjects = FindObjectsByLayer(_sfxLayer);
        _musicSources = new AudioSource[musicObjects.Length];
        for (int i = 0; i < musicObjects.Length; i++)
        {
            _musicSources[i] = musicObjects[i].GetComponent<AudioSource>();
        }

        GameObject[] sfxObjects = FindObjectsByLayer(_sfxLayer);
        _soundEffectSources = new AudioSource[sfxObjects.Length];
        for (int i = 0; i < sfxObjects.Length; i++)
        {
            _soundEffectSources[i] = sfxObjects[i].GetComponent<AudioSource>();
        }
    }

    GameObject[] FindObjectsByLayer(LayerMask layerMask)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> objectsInLayer = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (((1 << obj.layer) & layerMask) != 0)
            {
                objectsInLayer.Add(obj);
            }
        }

        return objectsInLayer.ToArray();
    }

    public void SetMusicVolume(float volume)
    {
        foreach (AudioSource sfxSource in _musicSources)
        {
            if (sfxSource != null)
            {
                sfxSource.volume = volume;
            }
        }
    }

    public void SetSFXVolume(float volume)
    {
        foreach (AudioSource sfxSource in _soundEffectSources)
        {
            if (sfxSource != null)
            {
                sfxSource.volume = volume;
            }
        }
    }
}
