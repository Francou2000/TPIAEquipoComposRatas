using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertState : State<StateEnum>
{
    IMove _move;
    Transform _entity;
    Transform _target;
    AudioSource _audioSource;

    public EnemyAlertState(IMove move, Transform entity, Transform target, AudioSource audioSource)
    {
        _move = move;
        _entity = entity;
        _target = target;
        _audioSource = audioSource;
    }

    public override void Enter()
    {
        base.Enter();

        _audioSource.Play();
    }

    public override void Execute()
    {
        base.Execute();
        //b-a
        //a:enemy
        //B: target
        Vector3 dirToTarget = _target.position - _entity.position;
        _move.Move(dirToTarget.normalized);
        dirToTarget.y = 0;
        _move.Look(dirToTarget);
    }
}
