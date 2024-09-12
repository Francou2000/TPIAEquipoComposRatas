using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IMove _move;

    FSM<StateEnum> _fsm;

    void Start()
    {
        _move = GetComponent<IMove>();
        InitializedFSM();
    }
    void InitializedFSM()
    {
        _fsm = new FSM<StateEnum>();
        var idle = new PlayerStateIdle<StateEnum>(_fsm, StateEnum.Move, _move);
        var move = new PlayerStateMove(_fsm, _move);

        idle.AddTransition(StateEnum.Move, move);

        move.AddTransition(StateEnum.Idle, idle);

        _fsm.SetInitial(idle);
    }
    void Update()
    {
        _fsm.OnUpdate();
    }
    private void FixedUpdate()
    {
        _fsm.OnFixedUpdate();
    }
    private void LateUpdate()
    {
        _fsm.OnLateUpdate();
    }
}
