using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateInteract : State<StateEnum>
{
    IInteract _interact;
    FSM<StateEnum> _fsm;
    public PlayerStateInteract(FSM<StateEnum> fsm, IInteract interact)
    {
        _interact = interact;
        _fsm = fsm;
    }

    public override void Enter()
    {
        base.Enter();
        _interact.Interact();
    }

    public override void Execute()
    {
        base.Execute();
        if (Input.GetKeyDown(KeyCode.E))
        {
            _fsm.Transition(StateEnum.Idle);
        }
    }
    public override void Exit()
    {
        base.Exit();
        _interact.Interact();
    }
}
