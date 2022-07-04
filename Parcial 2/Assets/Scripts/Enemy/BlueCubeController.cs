using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCubeController : MonoBehaviour
{
    [SerializeField] private Damageable _damageable;
    [SerializeField] private BlueCubeModel _blueCubeModel;
    
    public Transform target;
    
    [Header("Obs Avoidance")]
    public LayerMask obsMask;
    public float obsRange = 2;
    public float obsAngle = 190;
    public float waitTime = 2;
    public float randomAngle = 90;
    
    enum States
    {
        idle,
        Flee,
        Wander,
        GoToSafeSpot
    }
    
    private FSM<States> _fsm;
    private INode _root;
    private IState<States> _idleState;
    private IState<States> _fleeState;

    private IFlockingBehaviour _separation;
    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _blueCubeModel = GetComponent<BlueCubeModel>();
        _separation = GetComponent<IFlockingBehaviour>();
        _damageable.OnDie.AddListener(OnDieListener);
    }
    private void OnDieListener()
    {
        GameManager.instance.enemies.Remove(gameObject);
        EnemyManager.instance.enemies.Remove(gameObject.transform);

        // gameObject.SetActive(false);
        Destroy(gameObject);
    }
    private void Start()
    {
        GameManager.instance.enemies.Add(gameObject);
        EnemyManager.instance.enemies.Add(gameObject.transform);
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
        ISteering obsAvoidance = new ObstacleAvoidance(_blueCubeModel.transform, obsMask, obsRange, obsAngle);
        ISteering flee = new Flee(_blueCubeModel.transform, target);

        _fsm = new FSM<States>();
        _idleState = new IdleState<States>(_blueCubeModel, target, waitTime, _root);
        _fleeState = new FleeState<States>(_root, _blueCubeModel, target, flee, obsAvoidance, _separation);
        WanderingState<States> wanderState = new WanderingState<States>(_blueCubeModel, target, obsAvoidance, randomAngle, waitTime, _root,_separation);
        GoToSafeSpot<States> goToSafeSpotState = new GoToSafeSpot<States>(_blueCubeModel, target, _root, obsAvoidance, obsMask);

        goToSafeSpotState.AddTransition(States.Flee,_fleeState);
        goToSafeSpotState.AddTransition(States.idle,_idleState);
        
        _idleState.AddTransition(States.Flee,_fleeState);
        _idleState.AddTransition(States.Wander,wanderState);
        
        wanderState.AddTransition(States.idle,_idleState);
        wanderState.AddTransition(States.Flee,_fleeState);

        _fleeState.AddTransition(States.idle,_idleState);
        _fleeState.AddTransition(States.Wander,wanderState);
        _fleeState.AddTransition(States.GoToSafeSpot, goToSafeSpotState);
        
        _fsm.SetInit(wanderState);
    }
    private void InitializedTree()
    {
        ActionNode idle = new ActionNode(() => _fsm.Transition(States.idle));
        ActionNode flee = new ActionNode(() => _fsm.Transition(States.Flee));
        ActionNode wander = new ActionNode(() => _fsm.Transition(States.Wander));
        ActionNode goToSafeSpot = new ActionNode(() => _fsm.Transition(States.GoToSafeSpot));

        QuestionNode isIdle = new QuestionNode(IsIdle, wander, idle);
        QuestionNode isFleeing = new QuestionNode(IsFleeing, goToSafeSpot, isIdle);
        QuestionNode inSight = new QuestionNode(InSight, flee, isFleeing);
        

        _root = inSight;
    }

    bool IsFleeing()
    {
        return _fsm.GetCurrentState == _fleeState ;
    }
    bool IsIdle()
    {
        return _fsm.GetCurrentState == _idleState;
    }
    bool InSight()
    {
        return _blueCubeModel.LineOfSight(target);
    }  
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, obsRange);

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, obsAngle / 2, 0) * transform.forward * obsRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -obsAngle / 2, 0) * transform.forward * obsRange);
    }
}
