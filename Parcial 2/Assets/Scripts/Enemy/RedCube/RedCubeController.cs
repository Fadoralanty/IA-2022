using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCubeController : MonoBehaviour
{
    [SerializeField] private Damageable _damageable;
    [SerializeField] private RedCubeModel redCubeModel;

    public Transform target;
    [Header("Obs Avoidance")]
    public LayerMask obsMask;
    public float obsRange = 2;
    public float obsAngle = 190;
    public float waitTime = 2;
    public float randomAngle = 90;

    [Header("Idle State")]
    public float WaitTime = 3f;
    enum States
    {
        idle,
        Chase,
        GoToLastSeenPos
    }

    private FSM<States> _fsm;
    private INode _root;
    private IState<States> _idleState;
    private IFlockingBehaviour _separation;

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
        redCubeModel = GetComponent<RedCubeModel>();
        _separation = GetComponent<IFlockingBehaviour>();
        _damageable.OnDie.AddListener(OnDieListener);
    }

    private void Start()
    {
        GameManager.instance.enemies.Add(gameObject);
        EnemyManager.instance.enemies.Add(gameObject.transform);
        if(!target)
            target = GameManager.instance.Player.transform;
        InitializedTree();
        InitializedFSM();
    }

    private void Update()
    {
        _fsm.OnUpdate();
    }

    private void InitializedFSM()
    {
        ISteering obsAvoidance = new ObstacleAvoidance(redCubeModel.transform, obsMask, obsRange, obsAngle);
        ISteering seek = new Seek(redCubeModel.transform, target);

        _fsm = new FSM<States>();
        _idleState = new IdleState<States>(redCubeModel, target, waitTime, _root);
        ChaseState<States> chaseState = new ChaseState<States>(_root, redCubeModel, target, seek, obsAvoidance, _separation);
        GoToLastTargetSeenPosition<States> goToLastSeenPos = new GoToLastTargetSeenPosition<States>(redCubeModel, target, _root, obsAvoidance, obsMask);
        
        _idleState.AddTransition(States.Chase,chaseState);
        _idleState.AddTransition(States.GoToLastSeenPos,goToLastSeenPos);

        goToLastSeenPos.AddTransition(States.idle, _idleState);
        goToLastSeenPos.AddTransition(States.Chase, chaseState);

        chaseState.AddTransition(States.idle,_idleState);
        chaseState.AddTransition(States.GoToLastSeenPos,goToLastSeenPos);
        
        _fsm.SetInit(_idleState);
    }

    private void InitializedTree()
    {
        ActionNode idle = new ActionNode(() => _fsm.Transition(States.idle));
        ActionNode chase = new ActionNode(() => _fsm.Transition(States.Chase));
        ActionNode goToLastSeen = new ActionNode(() => _fsm.Transition(States.GoToLastSeenPos));

        QuestionNode wasSeen = new QuestionNode(WasSeen, goToLastSeen, idle);
        QuestionNode inSight = new QuestionNode(InSight, chase, wasSeen);
        QuestionNode wasDamaged = new QuestionNode(WasDamaged, chase, inSight);
        

        _root = wasDamaged;
    }
    bool InSight()
    {
        return redCubeModel.LineOfSight(target);
    }    
    bool WasSeen()
    {
        return EnemyManager.instance.PlayerWasSeen;
    }

    bool WasDamaged()
    {
        return _damageable.WasDamaged;
    }
    private void OnDieListener()
    {
        GameManager.instance.enemies.Remove(gameObject);
        EnemyManager.instance.enemies.Remove(gameObject.transform);

        // gameObject.SetActive(false);
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, obsRange);

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, obsAngle / 2, 0) * transform.forward * obsRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -obsAngle / 2, 0) * transform.forward * obsRange);
    }
}
