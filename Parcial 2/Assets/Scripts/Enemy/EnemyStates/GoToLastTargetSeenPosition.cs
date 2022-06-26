using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLastTargetSeenPosition<T> : State<T>
{
    EnemyModel _enemyModel;
    Transform _target;
    INode _root;
    Vector3 _lastSeenPos;
    ISteering _obsAvoidance;
    LayerMask _maskObs;
    Astar<Vector3> _ast;
    List<Vector3> _path;
    int _index;
    float _minDistance = 0.5f;
    
    public GoToLastTargetSeenPosition(EnemyModel enemyModel, Transform target, INode root, Vector3 lastSeenPos, 
        ISteering obsAvoidance,LayerMask maskObs, Astar<Vector3> ast, List<Vector3> path, int index, float minDistance)
    {
        _enemyModel = enemyModel;
        _target = target;
        _root = root;
        _lastSeenPos = lastSeenPos;
        _obsAvoidance = obsAvoidance;
        _maskObs = maskObs;
        _ast = ast;
        _path = path;
        _index = index;
        _minDistance = minDistance;
    }

    public override void Init()
    {
        base.Init();
        SetPath();
    }
    public override void Execute()
    {
        base.Execute();
        if (_path == null || _path.Count < 2)
        {
            _root.Execute();
            return;
        }
        if (_enemyModel.LineOfSight(_target))
        {
            _root.Execute();
            return;
        }
        RunList();
    }
    public override void Exit()
    {
        base.Exit();
        _path = null;
    }
    void RunList()
    {
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
            _root.Execute();
            return;
        }
        _enemyModel.LookDir(dir);
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
        heuristic += Vector3.Distance(curr, _lastSeenPos);
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
                if (Physics.Raycast(curr, dir, distance, _maskObs)) continue;
                neighbours.Add(newPos);
            }
        }
        return neighbours;
    }
    private bool IsSatisfied(Vector3 curr)
    {
        float distance = Vector3.Distance(curr, _lastSeenPos);
        return distance <= _minDistance;
    }
}
