using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasserbyView : MonoBehaviour
{
    [SerializeField]
    Animator _anim;
    Rigidbody _rb;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 horizontalVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _anim.SetFloat("Vel", horizontalVelocity.magnitude);
    }

}
