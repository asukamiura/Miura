using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : MonoBehaviour, IState
{
    private InputReciver input => InputReciver.Instance;
    public PlayerStateMachine stateMachine;

    public void Enter() { }
    public void Update() 
    {
        if (input.Move != Vector2.zero)
        {
            stateMachine.Transition(stateMachine.moveState);
        }

        if (input.AttackNormal)
        {
            stateMachine.Transition(stateMachine.attackNormal);
        }
    }
    public void Exit() { }
}
