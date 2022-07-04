using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToSafeSpot<T> : State<T>
{
    EnemyModel _enemyModel;
    
    INode _root;
    ISteering _obsAvoidance;
    LayerMask _maskObs;
    
    private Transform _safeSpot;
    private Transform _target;
    Astar<Vector3> _ast;
    List<Vector3> _path;
    int _index;
    float _minDistance = 0.5f;
    private float TargetRange = 2f;
    private float _currentTime = 0;
    private float _timer = 30;
    
    private Roulette<Transform> _rouletteSafeSpot;
    private Dictionary<Transform, int> _safeSpotsDictionary;
    public GoToSafeSpot(EnemyModel enemyModel, Transform target, INode root, ISteering obsAvoidance, LayerMask maskObs)
    {
        _ast = new Astar<Vector3>();
        _safeSpotsDictionary = new Dictionary<Transform, int>();
        foreach (var item in EnemyManager.instance.SafeSpots)
        {
            _safeSpotsDictionary.Add(item,Random.Range(1,100));
        }
        _rouletteSafeSpot = new Roulette<Transform>();
        _enemyModel = enemyModel;
        _target = target;
        _root = root;
        _obsAvoidance = obsAvoidance;
        _maskObs = maskObs;
    }

    public override void Init()
    {
        //Debug.Log("gotosafe");
        base.Init();
        _safeSpot = _rouletteSafeSpot.Run(_safeSpotsDictionary);
        _currentTime = 0f;
        SetPath();
    }
    public override void Exit()
    {
        base.Exit();
        _path = null;
    }
    public override void Execute()
    {
        base.Execute();
        if (_path == null || _path.Count < 2)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _timer)
            {
                SetPath();
                _currentTime = 0;
            }
        }
        else if (_enemyModel.LineOfSight(_target))
        {
            EnemyManager.instance.PlayerlastSeenPosition = _target.position;
            EnemyManager.instance.PlayerWasSeen = true;
            _root.Execute();
        }
        else
        {
            RunList();
        }
    }
     void RunList()
    {
        //if (_path== null) return;

        var currPoint = _path[_index];
        currPoint.y = _enemyModel.transform.position.y;
        Vector3 diff = currPoint - _enemyModel.transform.position;
        Vector3 dir = diff.normalized;
        float distance = diff.magnitude;
        if (distance < _minDistance)
        {
            _index++;
        }
        if (_index >= _path.Count)
        {
            EnemyManager.instance.PlayerWasSeen = false;
            _root.Execute();
            return;
        }
        _enemyModel.LookDir((dir + _obsAvoidance.GetDir()).normalized );
        _enemyModel.Move(_enemyModel.GetFoward);
    }
    
    private void SetPath()
    {
        Vector3 startPos = _enemyModel.transform.position;
        _path = _ast.GetPath(startPos, IsSatisfied, GetNeighbours, GetCost, Heuristic);
        _index = 0;
    }
    private float GetCost(Vector3 from, Vector3 to)
    {
        float distanceMultiplier = 1;

        float cost = 0;
        cost += Vector3.Distance(from, to) * distanceMultiplier;
        return cost;
    }
    private float Heuristic(Vector3 curr)
    {
        float heuristic = 0;
        heuristic += Vector3.Distance(curr, _safeSpot.position);
        //
        return heuristic;
    }
    private List<Vector3> GetNeighbours(Vector3 curr)
    {
        List<Vector3> neighbours = new List<Vector3>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                var newPos = new Vector3(curr.x + x, curr.y, curr.z + z);
                Vector3 diff = newPos - curr;
                Vector3 dir = diff.normalized;
                float distance = diff.magnitude;
                RaycastHit hit;
                bool obstacleHit = Physics.SphereCast(curr, 2, dir, out hit, distance,_maskObs);
                if (obstacleHit) continue;
                neighbours.Add(newPos);
            }
        }
        return neighbours;
    }
    private bool IsSatisfied(Vector3 curr)
    {
        var targetPos = _safeSpot.position;
        targetPos.y = curr.y;
        
        float distance = Vector3.Distance(curr, targetPos);
        return distance <= TargetRange;
    }
}
