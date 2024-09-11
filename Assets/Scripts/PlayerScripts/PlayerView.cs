using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    Animator _anim;

    private void Awake()
    {

    }

    private void Update()
    {
        _anim.SetFloat("Vel", 0);
    }
}
