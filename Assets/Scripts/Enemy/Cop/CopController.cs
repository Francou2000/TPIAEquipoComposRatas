using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CopController : MonoBehaviour, IWaitTimer, IPatrol, IAlert
{
    public Rigidbody target;
    
    [Header("Obstacle Avoidance")]
    public float timePrediction;
    
    [Header("Line of Sight")]
    public LineOfSight los;
    public float idleLos;
    public float alertedLos;
    public float idleLosAngle;
    public float alertedLosAngle;

    [field: Header("Alert")] 
    [field: SerializeField] public float AlertedTime { get; set; }
    public float AlertedTimer { get; set; }
    public bool IsAlerted { get; set; }
    
    [field: Header("Patrol")]
    [field: SerializeField] public Transform PatrolPointA { get; set; }
    [field: SerializeField] public Transform PatrolPointB { get; set; }
    public Transform CurrentTarget { get; set; }
    public bool HasArrived { get; set; }
    
    [field: Header("Idle Wait")]
    [field: SerializeField] public float WaitTime { get; set; }
    public float WaitTimer { get; set; }
    public bool DoneWaiting { get; set; }
    
    FSM<StateEnum> _fsm;
    IAttack _entityAttack;
    ITreeNode _root;
    ISteering _steering;
    GameObject[] _passersby;
    List<Rigidbody> _passersbyrb = new List<Rigidbody>();
    List<IAlert> _passersbyAlerts = new List<IAlert>();

    public AudioSource _audioSource;
    public DynamicBackgroundMusic _backgroundMusic;

    private void Start()
    {
        InitializedSteering();
        InitializedFSM();
        InitializedTree();
        CurrentTarget = PatrolPointB;
        _passersby = GameObject.FindGameObjectsWithTag("Passerby");
        for (int i = 0; i < _passersby.Length; i++)
        {
            _passersbyrb.Add(_passersby[i].GetComponent<Rigidbody>());
            _passersbyAlerts.Add(_passersby[i].GetComponent<IAlert>());
        }
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
        var chase = new EnemySteeringState(entityMove, _steering, this, los, alertedLos, alertedLosAngle);
        var patrol = new EnemyPatrolState(entityMove, transform, this, this, los, idleLos, idleLosAngle);
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
        var qAlertPasserbyInView = new QuestionTree(AlertPasserbyInView, chase, qDoneWaiting);
        var qAlreadyAlert = new QuestionTree(IsAlreadyAlert, qDistance, qAlertPasserbyInView);
        var qInView = new QuestionTree(InView, qDistance, qAlreadyAlert);
        var qIsExist = new QuestionTree(() => target != null, qInView, qDoneWaiting);

        _root = qIsExist;
    }
    bool InView()
    {
        if (los.CheckRange(target.transform) && los.CheckAngle(target.transform) && los.CheckView(target.transform))
        {
            AlertedTimer = 0;
            return true;
        }
        return false;
    }

    bool IsAlreadyAlert()
    {
        return IsAlerted && AlertedTimer < AlertedTime;
    }
    
    bool AlertPasserbyInView()
    {
        for (int i = 0; i < _passersbyrb.Count; i++)
        {
            if (_passersbyAlerts[i].IsAlerted &&
                los.CheckRange(_passersbyrb[i].transform) &&
                los.CheckAngle(_passersbyrb[i].transform) &&
                los.CheckView(_passersbyrb[i].transform))
            {
                AlertedTimer = 0;
                return true;
            };
        }
        return false;
    }
    bool InAttackRange()
    {
        return Vector3.Distance(target.transform.position, transform.position) <= _entityAttack.GetAttackRange;
    }

    bool _HasArrived()
    {
        //Chequeo haber llegado al punto de patrulla de forma aproximada
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
