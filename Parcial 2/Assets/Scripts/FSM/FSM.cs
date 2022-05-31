using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    IState<T> _current;
    public FSM(IState<T> init)
    {
        SetInit(init);
    }
    public FSM()
    {
    }
    public void SetInit(IState<T> init)
    {
        _current = init;
        _current.Init();
    }
    public void OnUpdate()
    {
        if (_current != null)
            _current.Execute();
    }
    public void Transition(T input)
    {
        var newState = _current.GetTransition(input);
        if (newState != null)
        {
            //Debug.Log(newState);
            _current.Exit();
            SetInit(newState);
        }
    }
    public IState<T> GetCurrentState => _current;
}
