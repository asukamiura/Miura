using System;
using System.Collections.Generic;

public class GameFlowStateMachine<TStateID>
{
    IState<TStateID> currentState;
    readonly Dictionary<TStateID, IState<TStateID>> states = new Dictionary<TStateID, IState<TStateID>>();
    readonly Dictionary<TStateID, Action> onEnterActions = new Dictionary<TStateID, Action>();
    readonly Dictionary<TStateID, Action> onExitActions = new Dictionary<TStateID, Action>();

    public TStateID CurrentState => currentState.StateID;

    // ステートを登録
    public void RegisterState(IState<TStateID> state, Action enterAction, Action exitAction)
    {
        if (!states.ContainsKey(state.StateID))
        {
            states.Add(state.StateID, state);
            onEnterActions.Add(state.StateID, enterAction);
            onExitActions.Add(state.StateID, exitAction);
        }
    }

    //初期ステートの設定
    public void Initialize(TStateID stateID)
    {
        if (states.TryGetValue(stateID, out IState<TStateID> startState))
        {
            currentState = startState;
            currentState?.Enter();
        }
    }

    // ステートの変更
    public void ChangeState(TStateID stateID)
    {
        if (states.TryGetValue(stateID, out IState<TStateID> newState))
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }
    }

    public void StateUpdate()
    {
        currentState?.Update();
    }

    public void StateFixedUpdate()
    {
        currentState?.FixedUpdate();
    }
}
