using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IMove _move;
    FSM<StateEnum> _fsm;
    public Transform _cameraTransform;
    private float _timeSincePosUpdate;
    private float _updateTimer = 5f;
    GameManager _gameManager;
    

    void Start()
    {
        _move = GetComponent<IMove>();
        InitializedFSM();
        _gameManager = GameManager.Instance;
    }

    void InitializedFSM()
    {
        _fsm = new FSM<StateEnum>();
        var idle = new PlayerStateIdle<StateEnum>(_fsm, StateEnum.Move, _move);
        var move = new PlayerStateMove(_fsm, _move, _cameraTransform);

        idle.AddTransition(StateEnum.Move, move);

        move.AddTransition(StateEnum.Idle, idle);

        _fsm.SetInitial(idle);
    }

    void Update()
    {
        _fsm.OnUpdate();
        if (_timeSincePosUpdate < _updateTimer)
        {
            _timeSincePosUpdate += Time.deltaTime;
        }
        else
        {
            _timeSincePosUpdate = 0f;
            _gameManager.UpdatePosition(transform.position.x, transform.position.z);
        }
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
