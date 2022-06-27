using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueIdle<T> : CooldownState<T>
{
    private BlueCubeModel _blueCubeModel;
    private Transform _target;
    public BlueIdle(BlueCubeModel blueCubeModel, Transform target, float time, INode root) : base(time, root)
    {
        _blueCubeModel = blueCubeModel;
        _target = target;
    }
    public override void Init()
    {
        base.Init();
        _blueCubeModel.Move(Vector3.zero);
    }
    public override void Execute()
    {
        base.Execute();
        if (_blueCubeModel.LineOfSight(_target))
        {
            EnemyManager.instance.PlayerWasSeen = true;
            EnemyManager.instance.PlayerlastSeenPosition = _target.position;
            _root.Execute();
            return;
        }

        if (EnemyManager.instance.PlayerWasSeen)
        {
            _root.Execute();
            return;
        }
    }
}
