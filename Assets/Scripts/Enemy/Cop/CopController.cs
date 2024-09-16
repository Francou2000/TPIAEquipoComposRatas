using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopController : MonoBehaviour, IWaitTimer, IPatrol
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
    public float WaitTimer { get; set; }
    [field: SerializeField] public float WaitTime { get; set; }
    public bool DoneWaiting { get; set; }
    FSM<StateEnum> _fsm;
    IAttack _entityAttack;
    ITreeNode _root;
    ISteering _steering;
    //ISteering _steeringPatrol;
    

    private void Start()
    {
        InitializedSteering();
        InitializedFSM();
        InitializedTree();
        CurrentTarget = PatrolPointB;
    }

    void InitializedSteering()
    {
        var pursuit = new Pursuit(transform, target, timePrediction);
        _steering = pursuit;
    }

    public void ChangeSteering(ISteering steering)
    {
        _steering = steering;
    }

    void InitializedFSM()
    {
        IMove entityMove = GetComponent<IMove>();
        _entityAttack = GetComponent<IAttack>();

        var idle = new EnemyIdleState(this);
        var chase = new EnemySteeringState(entityMove, _steering, los, alertedLos, alertedLosAngle);
        var patrol = new EnemyPatrolState(PatrolPointA, PatrolPointB, entityMove, transform, this, los, idleLos, idleLosAngle);
        var attack = new EnemyAttackState(_entityAttack);

        idle.AddTransition(StateEnum.Attack, attack);
        idle.AddTransition(StateEnum.Chase, chase);
        idle.AddTransition(StateEnum.Patrol, patrol);

        chase.AddTransition(StateEnum.Attack, attack);
        chase.AddTransition(StateEnum.Idle, idle);
        chase.AddTransition(StateEnum.Patrol, patrol);

        attack.AddTransition(StateEnum.Chase, chase);
        attack.AddTransition(StateEnum.Idle, idle);
        attack.AddTransition(StateEnum.Patrol, patrol);
        
        patrol.AddTransition(StateEnum.Attack, attack);
        patrol.AddTransition(StateEnum.Chase, chase);
        patrol.AddTransition(StateEnum.Idle, idle);

        _fsm = new FSM<StateEnum>(idle);
    }

    void InitializedTree()
    {
        var idle = new ActionTree(() => _fsm.Transition(StateEnum.Idle));
        var chase = new ActionTree(() => _fsm.Transition(StateEnum.Chase));
        var attack = new ActionTree(() => _fsm.Transition(StateEnum.Attack));
        var patrol = new ActionTree(() => _fsm.Transition(StateEnum.Patrol));

        var qDistance = new QuestionTree(InAttackRange, attack, chase);
        var qHasArrived = new QuestionTree(_HasArrived, idle, patrol);
        var qDoneWaiting = new QuestionTree(() => DoneWaiting, qHasArrived, idle);
        var qInView = new QuestionTree(InView, qDistance, qDoneWaiting);
        var qIsExist = new QuestionTree(() => target != null, qInView, qDoneWaiting);

        _root = qIsExist;
    }
    bool InView()
    {
        return (los.CheckRange(target.transform) && los.CheckAngle(target.transform) && los.CheckView(target.transform));
    }
    bool InAttackRange()
    {
        return Vector3.Distance(target.transform.position, transform.position) <= _entityAttack.GetAttackRange;
    }

    bool _HasArrived()
    {
        
        Debug.Log(AlmostEqual(transform.position, CurrentTarget.position, 0.05f));
        return AlmostEqual(transform.position, CurrentTarget.position, 0.05f);
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
    
    public bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;
		
        if (Mathf.Abs (v1.x - v2.x) > precision) equal = false;
        //if (Mathf.Abs (v1.y - v2.y) > precision) equal = false;
        if (Mathf.Abs (v1.z - v2.z) > precision) equal = false;
		
        return equal;
    }
}
