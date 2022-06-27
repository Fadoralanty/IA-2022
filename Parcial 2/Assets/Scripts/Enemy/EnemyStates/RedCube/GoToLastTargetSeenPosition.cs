using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLastTargetSeenPosition<T> : State<T>
{
    RedCubeModel _redCubeModel;
    Transform _target;
    INode _root;
    Vector3 _lastSeenPos;
    ISteering _obsAvoidance;
    LayerMask _maskObs;
    Astar<Vector3> _ast;
    List<Vector3> _path;
    int _index;
    float _minDistance = 0.5f;
    private float TargetRange = 2f;
    
    public GoToLastTargetSeenPosition(RedCubeModel redCubeModel, Transform target, INode root, ISteering obsAvoidance,
        LayerMask maskObs)
    {
        _ast = new Astar<Vector3>();
        _redCubeModel = redCubeModel;
        _target = target;
        _root = root;
        _obsAvoidance = obsAvoidance;
        _maskObs = maskObs;
    }

    public override void Init()
    {
        _lastSeenPos = EnemyManager.instance.PlayerlastSeenPosition;
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
        if (_redCubeModel.LineOfSight(_target))
        {
            EnemyManager.instance.PlayerlastSeenPosition = _target.position;
            EnemyManager.instance.PlayerWasSeen = true;
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
        currPoint.y = _redCubeModel.transform.position.y;
        Vector3 diff = currPoint - _redCubeModel.transform.position;
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
        _redCubeModel.LookDir((dir + _obsAvoidance.GetDir()).normalized );
        _redCubeModel.Move(_redCubeModel.GetFoward);
    }
    
    private void SetPath()
    {
        Vector3 startPos = _redCubeModel.transform.position;
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
        return distance <= TargetRange;
    }
}
