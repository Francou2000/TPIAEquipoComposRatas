using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasserbyController : MonoBehaviour
{
    public Rigidbody target;
    public LineOfSight los;
    public float timePrediction;
    FSM<StateEnum> _fsm;
    ITreeNode _root;
    ISteering _steering;

    private void Start()
    {
        InitializedSteering();
        InitializedFSM();
        InitializedTree();
    }

    void InitializedSteering()
    {
        var seek = new Seek(transform, target.transform);
        var flee = new Flee(transform, target.transform);
        var pursuit = new Pursuit(transform, target, timePrediction);
        var evade = new Evade(transform, target, timePrediction);
        _steering = evade;
    }

    public void ChangeSteering(ISteering steering)
    {
        _steering = steering;
    }

    void InitializedFSM()
    {
        IMove entityMove = GetComponent<IMove>();

        var idle = new EnemyIdleState();
        var chase = new EnemySteeringState(entityMove, _steering);

        idle.AddTransition(StateEnum.Chase, chase);

        chase.AddTransition(StateEnum.Idle, idle);

        _fsm = new FSM<StateEnum>(idle);
    }

    void InitializedTree()
    {
        var idle = new ActionTree(() => _fsm.Transition(StateEnum.Idle));
        var chase = new ActionTree(() => _fsm.Transition(StateEnum.Chase));

        
        var qInView = new QuestionTree(InView, chase, idle);
        var qIsExist = new QuestionTree(() => target != null, qInView, idle);

        _root = qIsExist;
    }

    bool InView()
    {
        return true;
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
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
