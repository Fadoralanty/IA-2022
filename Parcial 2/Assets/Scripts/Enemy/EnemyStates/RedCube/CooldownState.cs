using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownState<T> : State<T>
{
    float _time;
    float _counter;
    protected INode _root;
    public CooldownState(float time, INode root)
    {
        _root = root;
        _time = time;
    }
    public override void Init()
    {
        _counter = _time;
    }
    public override void Execute()
    {
        _counter -= Time.deltaTime;

        if (_counter <= 0)
        {
            _root.Execute();
            _counter = _time;
        }
    }
}
