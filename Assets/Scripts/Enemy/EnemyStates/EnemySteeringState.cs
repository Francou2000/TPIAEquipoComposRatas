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
    public EnemySteeringState(IMove move, ISteering steering, LineOfSight los, float alertedLos, float alertedLosAngle)
    {
        _move = move;
        _steering = steering;
        _los = los;
        _alertedLos = alertedLos;
        _alertedLosAngle = alertedLosAngle;
    }

    public override void Enter()
    {
        _los.range = _alertedLos;
        _los.angle = _alertedLosAngle;
        //actualizo el LoS
    }
    
    public override void Execute()
    {
        base.Execute();
        Vector3 dir = _steering.GetDir();
        _move.Move(dir.normalized);
    }
}