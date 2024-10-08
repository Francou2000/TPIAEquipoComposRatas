using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.SocialPlatforms;
using static UnityEditor.Progress;

public class CopModel : Entity, IAttack
{
    public LayerMask attackMask;
    [SerializeField]
    LineOfSight _attackOfSight;
    Action _onAttack;
    public float attackCooldownTime;
    Cooldown _attackCooldown;
    public AudioSource _audioSource;
    public AudioClip _attackSound;

    public ScenesManagement _scenesManagament;

    [Header("Obstacle Avoidance")]
    public float radius;
    public float angle;
    public float personalArea;
    public LayerMask obsMask;
    ObstacleAvoidance _obs;

    protected override void Awake()
    {
        base.Awake();
        _attackCooldown = new Cooldown(attackCooldownTime);
        _obs = new ObstacleAvoidance(transform, radius, angle, personalArea, obsMask);
    }

    public float GetAttackRange => _attackOfSight.range;

    public Action OnAttack { get => _onAttack; set => _onAttack = value; }

    public Cooldown Cooldown { get => _attackCooldown; }

    public void Attack()
    {
        _audioSource.PlayOneShot(_attackSound);

        Collider[] colls = Physics.OverlapSphere(transform.position, _attackOfSight.range, attackMask);
        
        foreach (var item in colls)
        {
            var currTarget = item.transform;
            if (!_attackOfSight.CheckAngle(currTarget)) continue;
            if (!_attackOfSight.CheckView(currTarget)) continue;

            StartCoroutine(AttackRoutine(item));

            break;
        }
        _attackCooldown.ResetCooldown();
        _onAttack();
    }

    public override void Move(Vector3 dir)
    {
        dir = _obs.GetDir(dir, false);
        dir.y = 0;
        Look(dir);
        base.Move(dir);
    }

    private void OnDrawGizmosSelected()
    {
        Color myColor = Color.cyan;
        myColor.a = 0.5f;
        Gizmos.color = myColor;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, personalArea);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * radius);
    }

    private IEnumerator AttackRoutine(Collider item)
    {
        Destroy(item.gameObject);
        yield return new WaitForSeconds(.5f);
        _scenesManagament.LoadScene("Defeat");
    }
}
