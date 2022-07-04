using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState<T> : State<T>
{
    INode _root;
    BlueCubeModel _blueCubeModel;
    Transform _target;
    ISteering _flee;
    ISteering _obsAvoidance;
    private IFlockingBehaviour _separation;
    
    public FleeState(INode root, BlueCubeModel blueCubeModel, Transform target, ISteering flee, ISteering obsAvoidance, IFlockingBehaviour separation)
    {
        _root = root;
        _blueCubeModel = blueCubeModel;
        _target = target;
        _flee = flee;
        _obsAvoidance = obsAvoidance;
        _separation = separation;
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Execute()
    {
        base.Execute();
        Vector3 dir = _flee.GetDir() + _obsAvoidance.GetDir() + _separation.GetDir(EnemyManager.instance.enemies, _blueCubeModel.transform);
        _blueCubeModel.LookDir(dir.normalized);
        _blueCubeModel.Move(_blueCubeModel.transform.forward);
        float dist = Vector3.Distance(_blueCubeModel.transform.position, _target.position);
        if (dist > _blueCubeModel.rangeSight)
        {
            _root.Execute();
        }
    }
}
