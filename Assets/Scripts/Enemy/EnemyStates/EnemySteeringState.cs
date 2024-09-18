using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySteeringState : State<StateEnum>
{
    private LineOfSight _los;
    private float _alertedLos;
    private float _alertedLosAngle;
    IMove _move;
    ISteering _steering;
    private IAlert _alert;

    AudioSource _audioSource;
    DynamicBackgroundMusic _music;

    public EnemySteeringState(IMove move, ISteering steering, IAlert alert, LineOfSight los, float alertedLos, float alertedLosAngle, AudioSource audioSource, DynamicBackgroundMusic music)
    {
        _move = move;
        _steering = steering;
        _los = los;
        _alertedLos = alertedLos;
        _alertedLosAngle = alertedLosAngle;
        _alert = alert;
        _audioSource = audioSource;
        _music = music;
    }

    public override void Enter()
    {
        _los.range = _alertedLos;
        _los.angle = _alertedLosAngle;
        _alert.IsAlerted = true;
        //actualizo el LoS

        _audioSource.Play();

        if (_music != null)
        {
            _music.SwitchToDangerMusic();
        }
    }
    
    public override void Execute()
    {
        base.Execute();
        Vector3 dir = _steering.GetDir();
        _move.Move(dir.normalized);
        _alert.AlertedTimer += Time.deltaTime;
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