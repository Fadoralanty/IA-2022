using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Damageable _damageable;
    [SerializeField] private EnemyModel _enemyModel;

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
        Chase
    }

    private FSM<States> _fsm;
    private INode _root;
    private IState<States> _idleState;

    private void Awake()
    {
        GameManager.instance.enemies.Add(gameObject);
        _damageable = GetComponent<Damageable>();
        _enemyModel = GetComponent<EnemyModel>();
        _damageable.OnDie.AddListener(OnDieListener);
    }

    private void Start()
    {
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
        ISteering obsAvoidance = new ObstacleAvoidance(_enemyModel.transform, obsMask, obsRange, obsAngle);
        ISteering seek = new Seek(_enemyModel.transform, target);

        _fsm = new FSM<States>();
        _idleState = new IdleState<States>(_enemyModel, target, waitTime, _root);
        ChaseState<States> chaseState = new ChaseState<States>(_root, _enemyModel, target, seek, obsAvoidance);
        
        _idleState.AddTransition(States.Chase,chaseState);
        
        chaseState.AddTransition(States.idle,_idleState);
        
        _fsm.SetInit(_idleState);
    }

    private void InitializedTree()
    {
        ActionNode idle = new ActionNode(() => _fsm.Transition(States.idle));
        ActionNode chase = new ActionNode(() => _fsm.Transition(States.Chase));

        QuestionNode inSight = new QuestionNode(InSight, chase, idle);

        _root = inSight;
    }
    bool InSight()
    {
        return _enemyModel.LineOfSight(target);
    }
    private void OnDieListener()
    {
        GameManager.instance.enemies.Remove(gameObject);
        gameObject.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, obsRange);

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, obsAngle / 2, 0) * transform.forward * obsRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -obsAngle / 2, 0) * transform.forward * obsRange);
    }
}
