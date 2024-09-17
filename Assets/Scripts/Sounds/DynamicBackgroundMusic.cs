using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBackgroundMusic : MonoBehaviour
{
    public float _volume = 0.5f;

    public AudioClip _normalMusic;    
    public AudioClip _dangerMusic;    

    private AudioSource _audioSource;
    private Coroutine _musicTransitionCoroutine;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_normalMusic != null)
        {
            _audioSource.clip = _normalMusic;
            _audioSource.loop = true;
            _audioSource.volume = _volume;
            _audioSource.Play();
        }
    }

    public void SwitchToDangerMusic()
    {
        if (_audioSource.clip != _dangerMusic && _dangerMusic != null)
        {
            if (_musicTransitionCoroutine != null) StopCoroutine(_musicTransitionCoroutine);
            _musicTransitionCoroutine = StartCoroutine(FadeToNewTrack(_dangerMusic, 1f));
        }
    }

    public void SwitchToNormalMusic()
    {
        if (_audioSource.clip != _normalMusic && _normalMusic != null)
        {
            if (_musicTransitionCoroutine != null) StopCoroutine(_musicTransitionCoroutine);
            _musicTransitionCoroutine = StartCoroutine(FadeToNewTrack(_normalMusic, 1f));
        }
    }

    private IEnumerator FadeToNewTrack(AudioClip newClip, float fadeDuration)
    {
        float startVolume = _audioSource.volume;
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        _audioSource.Stop();
        _audioSource.clip = newClip;
        _audioSource.Play();

        while (_audioSource.volume < startVolume)
        {
            _audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
