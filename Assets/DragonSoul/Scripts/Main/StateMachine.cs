using System.Collections.Generic;

public class StateMachine<TStateID>
{
    IState<TStateID> currentState;
    Dictionary<TStateID, IState<TStateID>> states = new Dictionary<TStateID, IState<TStateID>>();

    public TStateID CurrentState => currentState.StateID;

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
