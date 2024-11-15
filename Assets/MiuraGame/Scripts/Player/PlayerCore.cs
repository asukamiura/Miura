using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    public new Collider collider;
    public Collider attackCollider;
    private HealthManager healthManager;
    private PlayerStateID state => PlayerStateID.Idle;
    private StateMachine<PlayerStateID> stateMachine;

    private void Awake()
    {
        stateMachine = new StateMachine<PlayerStateID>();
        stateMachine.RegisterState(new PlayerIdle(this, stateMachine));
        stateMachine.RegisterState(new PlayerMove(this, stateMachine));
        stateMachine.RegisterState(new PlayerDodge(this, stateMachine));
        stateMachine.RegisterState(new PlayerParry(this, stateMachine));
        stateMachine.RegisterState(new PlayerAttackNormal1(this, stateMachine));
        stateMachine.RegisterState(new PlayerAttackNormal2(this, stateMachine));
        stateMachine.RegisterState(new PlayerAttackNormal3(this, stateMachine));
        stateMachine.RegisterState(new PlayerDamage(this, stateMachine));
    }

    private void Start()
    {
        stateMachine.Initialize(PlayerStateID.Idle);
        healthManager = GetComponent<HealthManager>();
    }

    private void Update()
    {
        stateMachine.Update();

        animator.SetFloat("Speed", rb.velocity.magnitude, 0.1f, Time.deltaTime);

        //if (healthManager.isDead)
        //{
        //    stateMachine.ChangeState(PlayerStateID.Dead);
        //}

        if (InputReciver.Instance.AttackCharge)
        {
            Debug.Log("AttackCharge");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 攻撃を受けたらダメージステートへ遷移
        stateMachine.ChangeState(PlayerStateID.Damage);
        attackCollider.enabled = false;
    }

    public void AttackStart()
    {
        attackCollider.enabled = true;
    }

    public void AttackEnd()
    {
        attackCollider.enabled = false;
    }
}
