using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyPatrolState : State<StateEnum>
{
    private LineOfSight _los;
    private float _idleLos;
    private float _idleLosAngle;
    IMove _move;
    private IPatrol _patrol;
    private Transform _pointA;
    private Transform _pointB;
    private Transform _unitTransform;
    private bool pointAtoB;
    private ISteering _steering;
    
    public EnemyPatrolState(Transform pointA, Transform pointB, IMove move, Transform unitTransform, IPatrol patrol, LineOfSight los, float idleLos, float idleLosAngle)
    {
        _pointA = pointA;
        _pointB = pointB;
        _move = move;
        _unitTransform = unitTransform;
        _patrol = patrol;
        _los = los;
        _idleLos = idleLos;
        _idleLosAngle = idleLosAngle;
    }
    public override void Enter()
    {
        base.Enter();
        if (_patrol.CurrentTarget == null)
        {
            _patrol.CurrentTarget = _pointB;
            pointAtoB = true;
        }
        _patrol.HasArrived = false;
        InitializedSteering();
        _los.range = _idleLos;
        _los.angle = _idleLosAngle;
    }
    public override void Execute()
    {
        base.Execute();
        //Debug.Log("Patrolling to B:" + pointAtoB);
        Vector3 dir = _steering.GetDir();
        _move.Move(dir.normalized);

        if (_unitTransform.position.x == _patrol.CurrentTarget.position.x && _unitTransform.position.y == _patrol.CurrentTarget.position.y)
        {
            _patrol.HasArrived = true;
            Debug.Log("llegó");
        }
    }

    public override void Exit()
    {
        if (pointAtoB)
        {
            pointAtoB = false;
            _patrol.CurrentTarget = _pointA;
        }
        else
        {
            pointAtoB = true;
            _patrol.CurrentTarget = _pointB;
        }
        
    } 
    
    void InitializedSteering()
    {
        var seek = new Seek(_unitTransform, _patrol.CurrentTarget);
        _steering = seek;
    }
}
