using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFollowPoints<T> : State<T>
{
    protected List<Vector3> _waypoints;
    int _index;
    protected Transform _entity;
    float _distanceToPoint = 0.2f;
    bool _isFinishPath;
    IMove _move;
     
    public StateFollowPoints(Transform entity, float distanceToPoint = 0.2f, IMove move = null)
    {
        _entity = entity;
        _distanceToPoint = distanceToPoint;
        _isFinishPath = true;
        _move = move;
    }

    public override void Execute()
    {
        base.Execute();
        Run();
    }

    public void SetWaypoints(List<Vector3> newPoints)
    {
        if (newPoints.Count == 0) return;
        _waypoints = newPoints;
        _index = 1;
        _isFinishPath = false;
        OnStartPath();
    }

    void Run()
    {
        //Debug.Log("Entró en el run");
        if (_isFinishPath) return;
        //Debug.Log("El camino no está finalizado");
        //Debug.Log("Índice: " + _index + ". Total: " + _waypoints.Count);
        if (_index > _waypoints.Count - 1) _index = 0; 
        Vector3 point = _waypoints[_index];
        point.y = _entity.position.y; //Horizontal move
        Vector3 dir = point - _entity.position;
        
        if (dir.magnitude < _distanceToPoint)
        {
            Debug.Log(_index + 1 < _waypoints.Count);
            if (_index + 1 < _waypoints.Count)
                _index++;
            else
            {
                _isFinishPath = true;
                OnFinishPath();
                return;
            }
        }

        _move.Move(dir.normalized);

       // OnMove(dir.normalized);
    }

    protected virtual void OnMove(Vector3 dir)
    {

    }

    protected virtual void OnStartPath()
    {

    }

    protected virtual void OnFinishPath()
    {
        Debug.Log("Path finalizado");
    }

    public bool IsFinishPath => _isFinishPath;
}
