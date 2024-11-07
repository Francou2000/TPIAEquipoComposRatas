using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
    Transform _entity;
    Transform _target;
    public Seek(Transform entity, Transform target)
    {
        _entity = entity;
        _target = target;
    }

    public virtual Vector3 GetDir()
    {
        if (!_target) return Vector3.zero;
        return (_target.position - _entity.position).normalized;
    }

    public virtual Vector3 GetPoint()
    {
        if (!_target) return Vector3.zero;
        return (_target.position - _entity.position);
    }
    
    public Transform Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
        }
    }
}
