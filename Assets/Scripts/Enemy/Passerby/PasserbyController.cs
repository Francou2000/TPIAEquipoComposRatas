using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PasserbyController : MonoBehaviour, IWaitTimer, IPatrol
{
    public Rigidbody target;
    public LineOfSight los;
    public float idleLos;
    public float alertedLos;
    public float idleLosAngle;
    public float alertedLosAngle;
    public float timePrediction;
    [field: SerializeField] public Transform PatrolPointA { get; set; }
    [field: SerializeField] public Transform PatrolPointB { get; set; }
    public Transform CurrentTarget { get; set; }
    public bool HasArrived { get; set; }
    FSM<StateEnum> _fsm;
    ITreeNode _root;
    ISteering _steering;
    public float WaitTimer { get; set; }
    [field: SerializeField] public float WaitTime { get; set; }
    public bool DoneWaiting { get; set; }

    private void Start()
    {
        InitializedSteering();
        InitializedFSM();
        InitializedTree();
        CurrentTarget = PatrolPointB;
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

        var idle = new EnemyIdleState(this);
        var chase = new EnemySteeringState(entityMove, _steering, los, alertedLos, alertedLosAngle);
        var patrol = new EnemyPatrolState(PatrolPointA, PatrolPointB, entityMove, transform, this, los, idleLos, idleLosAngle);

        idle.AddTransition(StateEnum.Chase, chase);
        idle.AddTransition(StateEnum.Patrol, patrol);

        chase.AddTransition(StateEnum.Idle, idle);
        chase.AddTransition(StateEnum.Patrol, patrol);
        
        patrol.AddTransition(StateEnum.Idle, idle);
        patrol.AddTransition(StateEnum.Chase, chase);

        _fsm = new FSM<StateEnum>(idle);
    }

    void InitializedTree()
    {
        var idle = new ActionTree(() => _fsm.Transition(StateEnum.Idle));
        var chase = new ActionTree(() => _fsm.Transition(StateEnum.Chase));
        var patrol = new ActionTree(() => _fsm.Transition(StateEnum.Patrol));
        
        var qHasArrived = new QuestionTree(_HasArrived, idle, patrol);
        var qDoneWaiting = new QuestionTree(() => DoneWaiting, qHasArrived, idle);
        var qInView = new QuestionTree(InView, chase, qDoneWaiting);
        var qIsExist = new QuestionTree(() => target != null, qInView, qDoneWaiting);

        _root = qIsExist;
    }

    bool InView()
    {
        return (los.CheckRange(target.transform) && los.CheckAngle(target.transform) && los.CheckView(target.transform));
    }
    
    bool _HasArrived()
    {
        
        //Debug.Log(AlmostEqual(transform.position, CurrentTarget.position, 0.05f));
        return AlmostEqual(transform.position, CurrentTarget.position, 0.05f);
    }
    
    public bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;
		
        if (Mathf.Abs (v1.x - v2.x) > precision) equal = false;
        //if (Mathf.Abs (v1.y - v2.y) > precision) equal = false;
        if (Mathf.Abs (v1.z - v2.z) > precision) equal = false;
		
        return equal;
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
