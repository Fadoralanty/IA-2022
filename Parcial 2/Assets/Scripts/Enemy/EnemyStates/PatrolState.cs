using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState<T> : State<T>
{
    private EnemyModel _enemyModel;
    private Transform[] _patrolSpots;
    private int _index;
    private int _lastIndex = 0;
    private bool _isGoingBackwards;
    private INode _root;
    private Transform _target;
    public override void Init()
    {
        //Debug.Log("patrol");
        Vector3 dir = _patrolSpots[_index].position - _enemyModel.transform.position;
        _enemyModel.LookDir(dir.normalized);
        _index = _lastIndex;
        _isGoingBackwards = false;
    }
    public override void Execute()
    {
        if (_enemyModel.LineOfSight(_target))
        {
            _root.Execute();
            return;
        }

        if (_enemyModel._damageable.WasDamaged)
        {
            EnemyManager.instance.PlayerWasSeen = true;
            EnemyManager.instance.PlayerlastSeenPosition = _target.position;
            _root.Execute();
            _enemyModel._damageable.WasDamaged = false;
            return;
        }
        
        if (EnemyManager.instance.PlayerWasSeen)
        {
            _root.Execute();
            return;
        }
        
        if (Vector3.Distance(_patrolSpots[_index].position, _enemyModel.transform.position) < 1f) //si llego al waypoint
        {
            if(_index == _patrolSpots.Length-1) //si llego al ultimo waypoint que recorra usando- 
                //-este flag el array al reves hasta el primer waypoint
            {
                _isGoingBackwards = true;
            }
            if (_index == 0)
            {
                _isGoingBackwards = false;
            }
            if (_isGoingBackwards) 
            { 
                _index--;
            }
            else
            {
                _index++;
            }
            _root.Execute();
        }
        else // si no llego al waypoint que se mueva hacia el waypoint numero index
        {
            Vector3 dir = _patrolSpots[_index].position - _enemyModel.transform.position;
            //Debug.DrawLine(_enemy.transform.position, _enemy.transform.position+dir, Color.red);
            _enemyModel.Move(dir.normalized);
            _enemyModel.LookDir(dir.normalized);
        }
    }
    public override void Exit()
    {
        _lastIndex = _index;
    }
}
