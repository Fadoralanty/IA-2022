using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingState<T> : CooldownState<T>
{
    BlueCubeModel _blueCubeModel;
    Transform _target;
    float _randomAngle;
    Vector3 _dir;
    ISteering _steering;

    public WanderingState(BlueCubeModel blueCubeModel, Transform target, ISteering steering, float randomAngle, float time, INode root) : base(time, root)
    {
        _blueCubeModel = blueCubeModel;
        _randomAngle = randomAngle;
        _target = target;
        _steering = steering;
    }
    public override void Init()
    {
        base.Init();
        _dir = Quaternion.Euler(0, Random.Range(-_randomAngle, _randomAngle), 0) * _blueCubeModel.transform.forward;
    }
    public override void Execute()
    {
        base.Execute();
        _blueCubeModel.LookDir(_dir + _steering.GetDir());
        _blueCubeModel.Move(_blueCubeModel.transform.forward);
        if (_blueCubeModel.LineOfSight(_target))
        {
            EnemyManager.instance.PlayerWasSeen = true;
            EnemyManager.instance.PlayerlastSeenPosition = _target.position;
            _root.Execute();
        }
    }
}
