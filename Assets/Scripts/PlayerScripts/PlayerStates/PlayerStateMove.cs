using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMove : State<StateEnum>
{
    IMove _move;
    FSM<StateEnum> _fsm;
    Transform _cameraTransform;

    public PlayerStateMove(FSM<StateEnum> fsm, IMove move, Transform cameraTransform)
    {
        _move = move;
        _fsm = fsm;
        _cameraTransform = cameraTransform;
    }

    public override void FixedExecute()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 dir = (cameraForward * v + cameraRight * h).normalized;

        if (h == 0 && v == 0)
        {
            _fsm.Transition(StateEnum.Idle);
        }
        else
        {
            _move.Move(dir);  
            _move.Look(dir);  
        }
    }
}
