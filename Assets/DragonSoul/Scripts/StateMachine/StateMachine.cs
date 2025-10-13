using System;
using System.Collections.Generic;

public class StateMachine<TStateID>
{
    IState<TStateID> currentState;
    IState<TStateID> previousState;
    Dictionary<TStateID, IState<TStateID>> states = new Dictionary<TStateID, IState<TStateID>>();

    public TStateID CurrentState => currentState.StateID;
    public TStateID PreviousState => previousState.StateID;

    public Action<TStateID> OnStateChanged { get; set; }

    // ステートを登録
    public void RegisterState(IState<TStateID> state)
    {
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
            previousState = startState;
            currentState = startState;
            currentState?.Enter();

            OnStateChanged?.Invoke(stateID);
        }
    }

    // ステートの変更
    public void ChangeState(TStateID stateID)
    {
        if (states.TryGetValue(stateID, out IState<TStateID> newState))
        {
            currentState?.Exit();
            previousState = currentState;
            currentState = newState;
            currentState?.Enter();

            OnStateChanged?.Invoke(stateID);
        }
    }

    public void UpdateState()
    {
        currentState?.Update();
    }

    public void FixedUpdateState()
    {
        currentState?.FixedUpdate();
    }
}
