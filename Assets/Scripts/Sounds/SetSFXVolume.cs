using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSFXVolume : MonoBehaviour
{
    private Slider _mySlider;

    void Start()
    {
        _mySlider = GetComponent<Slider>();
        _mySlider.value = VolumeController.Instance._SFXVolume;
        _mySlider.onValueChanged.AddListener((v) => {
            VolumeController.Instance.SetSFXVolumeTo(v);
            VolumeController.Instance._volumeUpdate.Invoke();
        });

    }
}
