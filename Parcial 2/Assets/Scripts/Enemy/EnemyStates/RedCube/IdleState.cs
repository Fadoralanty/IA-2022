using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : CooldownState<T>// TODO add obstacle avoidance
{
    RedCubeModel _redCubeModel;
    Transform _target;
    public IdleState(RedCubeModel redCubeModel, Transform target, float time, INode root) : base(time, root)
    {
        _redCubeModel = redCubeModel;
        _target = target;
    }
    public override void Init()
    {
        base.Init();
        //Debug.Log("idle");
        _redCubeModel.Move(Vector3.zero);
    }
    public override void Execute()
    {
        base.Execute();
        if (_redCubeModel.LineOfSight(_target))
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
