using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMusicVolume : MonoBehaviour
{
    private Slider _mySlider;

    void Start()
    {
        _mySlider = GetComponent<Slider>();
        _mySlider.value = VolumeController.Instance._musicVolume;
        _mySlider.onValueChanged.AddListener((v) => {
            VolumeController.Instance.SetMusicVolumeTo(v);
            VolumeController.Instance._volumeUpdate.Invoke();
        });

    }
}
