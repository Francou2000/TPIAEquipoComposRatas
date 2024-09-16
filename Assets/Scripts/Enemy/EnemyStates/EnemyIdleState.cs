using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State<StateEnum>
{
    private IWaitTimer _waitTimer;
    
    public EnemyIdleState(IWaitTimer waitTimer)
    {
        _waitTimer = waitTimer;
    }

    public override void Enter()
    {
        _waitTimer.WaitTimer = 0;
        _waitTimer.DoneWaiting = false;
    }
    
    public override void Execute()
    {
        _waitTimer.WaitTimer += Time.deltaTime;
        if (_waitTimer.WaitTimer >= _waitTimer.WaitTime) _waitTimer.DoneWaiting = true;
    }

    public override void Exit()
    {
        _waitTimer.DoneWaiting = true;
    }
}
