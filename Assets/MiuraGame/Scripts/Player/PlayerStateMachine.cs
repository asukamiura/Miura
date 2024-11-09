using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public IState currentState;
    private Dictionary<PlayerStateID,IState> states = new Dictionary<PlayerStateID,IState>();

    public void RegisterState(IState state)
    {
        if (!states.ContainsKey(state.StateID))
        {
            states.Add(state.StateID, state);
            Debug.Log(state.ToString());
        }
    }

    //初期ステートの設定
    public void Initialize(PlayerStateID stateID)
    {
        if (states.TryGetValue(stateID, out IState startState))
        {
            currentState?.Exit();
            currentState = startState;
            currentState.Enter();
        }
    }

    public void ChangeState(PlayerStateID stateID)
    {
        if (states.TryGetValue(stateID, out IState newState))
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }
}
