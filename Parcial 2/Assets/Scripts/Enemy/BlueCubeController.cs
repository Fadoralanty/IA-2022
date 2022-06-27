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
        Flee
    }
    
    private FSM<States> _fsm;
    private INode _root;
    private IState<States> _idleState;
    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _blueCubeModel = GetComponent<BlueCubeModel>();
        _damageable.OnDie.AddListener(OnDieListener);
    }
    private void OnDieListener()
    {
        GameManager.instance.enemies.Remove(gameObject);
        gameObject.SetActive(false);
    }
    private void Start()
    {
        GameManager.instance.enemies.Add(gameObject);
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
        _idleState = new BlueIdle<States>(_blueCubeModel, target, waitTime, _root);
        FleeState<States> fleeState = new FleeState<States>(_root, _blueCubeModel, target, flee, obsAvoidance);
        
        _idleState.AddTransition(States.Flee,fleeState);
        

        fleeState.AddTransition(States.idle,_idleState);
        
        _fsm.SetInit(_idleState);
    }
    private void InitializedTree()
    {
        ActionNode idle = new ActionNode(() => _fsm.Transition(States.idle));
        ActionNode flee = new ActionNode(() => _fsm.Transition(States.Flee));

        QuestionNode inSight = new QuestionNode(InSight, flee, idle);

        _root = inSight;
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
