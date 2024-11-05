using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : MonoBehaviour, IState
{
    public PlayerStateMachine stateMachine;
    private InputReciver input => InputReciver.Instance;
    [SerializeField]Rigidbody rb;
    [SerializeField] float speed = 1f;
    private Quaternion targetRotation;

    void Awake()
    {
        targetRotation = transform.rotation;
    }
    public void Enter() { }
    public void Update() 
    {
        Vector3 velocity = new Vector3(input.Move.x, 0, input.Move.y).normalized;
        var rotationSpeed = 600 * Time.deltaTime;
        rb.velocity = velocity * speed;
        if (velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
        if (input.Move != Vector2.zero)
        {
            stateMachine.Transition(stateMachine.idleState);
        }
        if (input.AttackNormal)
        {
            stateMachine.Transition(stateMachine.attackNormal);
        }
    }
    public void Exit() { }
}
