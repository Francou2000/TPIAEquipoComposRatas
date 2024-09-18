using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : State<StateEnum>
{
    IMove _move;
    Transform _entity;
    Transform _target;
    AudioSource _audioSource;
    DynamicBackgroundMusic _music;

    public EnemyChaseState(IMove move, Transform entity, Transform target, AudioSource audioSource, DynamicBackgroundMusic music)
    {
        _move = move;
        _entity = entity;
        _target = target;
        _audioSource = audioSource;
        _music = music;
    }

    public override void Enter()
    {
        base.Enter();

        _audioSource.Play();

        if (_music != null)
        {
            _music.SwitchToDangerMusic();
        }
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

    public override void Exit() 
    { 
        base.Exit();

        if (_music != null)
        {
            _music.SwitchToNormalMusic();
        }
    }
}
