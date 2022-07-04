using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    /// <summary>
    /// AWAKE
    /// </summary>
    void Init();
    /// <summary>
    /// EXECUTE / UPDATE
    /// </summary>
    void Execute();
    /// <summary>
    /// SLEEP
    /// </summary>
    void Exit();
    void AddTransition(T input, IState<T> state);
    void RemoveTransition(T input);
    void RemoveTransition(IState<T> state);
    IState<T> GetTransition(T input);
}
