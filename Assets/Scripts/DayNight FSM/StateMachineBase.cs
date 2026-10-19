using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class StateMachineBase<TState, TMachine> : MonoBehaviour
    where TState : IState
{
    public TState CurrentState { get; protected set; }
    // lets you pass in a type directly and get an IState (or child of IState)
    public Dictionary<Type, TState> states;

    protected virtual void Update()
    {
        CurrentState?.UpdateState();
    }

    public void ChangeState(System.Type type)
    {
        try
        {
            TState newState = states[type];

            CurrentState?.OnExit();
            CurrentState = newState;
            CurrentState.OnEnter();
        }
        catch
        {
            Debug.LogError("Error changing state, likely invalid state type.");
        }
    }

    protected void RegisterState(Type type, TState state)
    {
        states[type] = state;
    }
}