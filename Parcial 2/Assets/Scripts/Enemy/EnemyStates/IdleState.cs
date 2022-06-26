using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : CooldownState<T>
{
    EnemyModel _enemyModel;
    Transform _target;
    public IdleState(EnemyModel enemyModel, Transform target, float time, INode root) : base(time, root)
    {
        _enemyModel = enemyModel;
        _target = target;
    }
    public override void Init()
    {
        base.Init();
        //Debug.Log("idle");
        _enemyModel.Move(Vector3.zero);
    }
    public override void Execute()
    {
        base.Execute();
        if (_enemyModel.LineOfSight(_target))
        {
            _root.Execute();
        }
    }
}
