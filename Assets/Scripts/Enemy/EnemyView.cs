using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField]
    Animator _anim;
    Rigidbody _rb;
    IAttack _attack;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<IAttack>();
    }
    private void Start()
    {
        _attack.OnAttack += OnAttackAnim;
    }
    private void Update()
    {
        Vector3 horizontalVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _anim.SetFloat("Vel", horizontalVelocity.magnitude);
    }
    void OnAttackAnim()
    {
        _anim.SetTrigger("Attack");
    }
}
