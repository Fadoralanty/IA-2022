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
        Debug.Log("idle");
        base.Init();
        //Debug.Log("idle");
        _enemyModel.Move(Vector3.zero);
    }
    public override void Execute()
    {
        base.Execute();
        if (_enemyModel.LineOfSight(_target))
        {
            EnemyManager.instance.PlayerWasSeen = true;
            EnemyManager.instance.PlayerlastSeenPosition = _target.position;
            _root.Execute();
            return;
        }

        if (_enemyModel._damageable.WasDamaged)
        {
            _root.Execute();
            EnemyManager.instance.PlayerWasSeen = true;
            EnemyManager.instance.PlayerlastSeenPosition = _target.position;
            _enemyModel._damageable.WasDamaged = false;
            return;
        }

        if (EnemyManager.instance.PlayerWasSeen)
        {
            _root.Execute();
            return;
        }
    }
}
