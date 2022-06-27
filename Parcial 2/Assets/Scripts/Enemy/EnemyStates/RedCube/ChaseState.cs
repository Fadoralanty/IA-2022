using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState<T> : State<T>
{

    INode _root;
    RedCubeModel _redCube;
    Transform _target;
    ISteering _seek;
    ISteering _obsAvoidance;
    
    public ChaseState(INode root, RedCubeModel redCube, Transform target, ISteering seek, ISteering obsAvoidance)
    {
        _root = root;
        _redCube = redCube;
        _target = target;
        _seek = seek;
        _obsAvoidance = obsAvoidance;
    }

    public override void Init()
    {
        base.Init();
        //Debug.Log("chase");
    }

    public override void Execute()
    {
        base.Execute();
        Vector3 dir = _seek.GetDir() + _obsAvoidance.GetDir();
        _redCube.LookDir(dir.normalized);
        _redCube.Move(_redCube.transform.forward);
        if (!_redCube.LineOfSight(_target))
        {
            EnemyManager.instance.PlayerlastSeenPosition = _target.position;
            _root.Execute();
        }
        
    }
    
    
}
