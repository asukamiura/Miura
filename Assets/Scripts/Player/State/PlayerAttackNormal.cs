using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackNormal : MonoBehaviour, IState
{
    public PlayerStateMachine playerStateMachine;
    private InputReciver input => InputReciver.Instance;

    public void Enter()
    {
        Debug.Log("AttackNormal");
    }

    public void Update()
    {
        
    }

    public void Exit()
    {

    }

}
