using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TStateID>
{
    private IState<TStateID> currentState;
    public TStateID StateID;
    private Dictionary<TStateID,IState<TStateID>> states = new Dictionary<TStateID,IState<TStateID>>();

    public void RegisterState(IState<TStateID> state)
    {
        Debug.Log(state.ToString());
        if (!states.ContainsKey(state.StateID))
        { 
            states.Add(state.StateID, state);
        }
    }

    //初期ステートの設定
    public void Initialize(TStateID stateID)
    {
        if (states.TryGetValue(stateID, out IState<TStateID> startState))
        {
            currentState?.Exit();
            currentState = startState;
            currentState.Enter();
        }
    }

    public void ChangeState(TStateID stateID)
    {
        if (states.TryGetValue(stateID, out IState<TStateID> newState))
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
            StateID = stateID;
            Debug.Log(currentState.ToString());
        }
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }
}
