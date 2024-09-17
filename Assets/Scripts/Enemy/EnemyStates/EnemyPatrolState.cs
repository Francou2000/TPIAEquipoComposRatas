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
    private IAlert _alert;
    
    public EnemyPatrolState(IMove move, Transform unitTransform, IPatrol patrol, IAlert alert, LineOfSight los, float idleLos, float idleLosAngle)
    {
        _pointA = patrol.PatrolPointA;
        _pointB = patrol.PatrolPointB;
        _move = move;
        _unitTransform = unitTransform;
        _patrol = patrol;
        _los = los;
        _idleLos = idleLos;
        _idleLosAngle = idleLosAngle;
        _alert = alert;
        //son muchas cosas pero prometo que son necesarias, es el estado donde pasa la mayoría del tiempo
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
        _alert.IsAlerted = false;
        //Actualizo el LoS y preparo el steering para el próximo punto de patrullaje
    }
    public override void Execute()
    {
        base.Execute();
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
        //Al salir del estado, preparo el próximo punto de patrullaje
    } 
    
    void InitializedSteering()
    {
        var seek = new Seek(_unitTransform, _patrol.CurrentTarget);
        _steering = seek;
    }
}
