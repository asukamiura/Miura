using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour 
{
    public IState CurrentState {  get; private set; }
    private PlayerAnimation playerAnimation;
    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    public PlayerAttackNormal attackNormal;

    void Awake()
    {
        Initialize(idleState);
    }

    public void Initialize(IState startState)
    {
        CurrentState = startState;
        startState.Enter();
    }

    public void Transition(IState nextState)
    {
        CurrentState.Exit();
        CurrentState= nextState;
        nextState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }

}
