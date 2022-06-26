using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState<T> : State<T>
{

    INode _root;
    EnemyModel _enemy;
    Transform _target;
    ISteering _seek;
    ISteering _obsAvoidance;
    
    public ChaseState(INode root, EnemyModel enemy, Transform target, ISteering seek, ISteering obsAvoidance)
    {
        _root = root;
        _enemy = enemy;
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
        _enemy.LookDir(dir.normalized);
        _enemy.Move(_enemy.transform.forward);
        if (!_enemy.LineOfSight(_target))
        {
            EnemyManager.instance.PlayerlastSeenPosition = _target.position;
            _root.Execute();
        }
        
    }
    
    
}
