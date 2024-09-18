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
            return;
        }

        FindAudioSourcesInScene();
        LoadVolumeSettings();
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

    private void LoadVolumeSettings()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f); 
        SetMusicVolume(savedMusicVolume);

        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f); 
        SetSFXVolume(savedSFXVolume);
    }

    GameObject[] FindObjectsByLayer(LayerMask layerMask)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<GameObject> objectsInLayer = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            int objLayer = obj.layer;
            if ((1 << objLayer) != 0)
            {
                objectsInLayer.Add(obj);
            }
        }

        if (objectsInLayer.Count == 0)
        {
            Debug.LogWarning("No objects found on the specified layer.");
        }

        return objectsInLayer.ToArray();
    }

    void FindAudioSourcesInScene()
    {
        GameObject[] musicObjects = FindObjectsByLayer(_musicLayer);
        if (musicObjects.Length > 0)
        {
            _musicSources = new AudioSource[musicObjects.Length];
            for (int i = 0; i < musicObjects.Length; i++)
            {
                _musicSources[i] = musicObjects[i].GetComponent<AudioSource>();
                Debug.Log("SFX source found: " + musicObjects[i].name);
            }
        }
        else
        {
            Debug.LogWarning("No SFX sources found in the scene on the specified layer.");
        }


        GameObject[] sfxObjects = FindObjectsByLayer(_sfxLayer);
        if (sfxObjects.Length > 0)
        {
            _soundEffectSources = new AudioSource[sfxObjects.Length];
            for (int i = 0; i < sfxObjects.Length; i++)
            {
                _soundEffectSources[i] = sfxObjects[i].GetComponent<AudioSource>();
                Debug.Log("SFX source found: " + sfxObjects[i].name);
            }
        }
        else
        {
            Debug.LogWarning("No SFX sources found in the scene on the specified layer.");
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (_soundEffectSources != null && _soundEffectSources.Length > 0)
        {
            foreach (AudioSource musicSource in _musicSources)
            {
                if (musicSource != null)
                {
                    musicSource.volume = volume;
                }
            }
            PlayerPrefs.SetFloat("MusicVolume", volume); 
            PlayerPrefs.Save();  
        }
        else
        {
            Debug.LogWarning("MusicSource is not assigned or not found in the scene!");
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (_soundEffectSources != null && _soundEffectSources.Length > 0)
        {
            foreach (AudioSource sfxSource in _soundEffectSources)
            {
                if (sfxSource != null)
                {
                    sfxSource.volume = volume;
                }
            }
            PlayerPrefs.SetFloat("SFXVolume", volume);  
            PlayerPrefs.Save();  
        }
        else
        {
            Debug.LogWarning("No sound effect sources found in the scene or soundEffectSources array is empty.");
        }
    }

    public void RefreshAudioSources()
    {
        FindAudioSourcesInScene();
    }
}
