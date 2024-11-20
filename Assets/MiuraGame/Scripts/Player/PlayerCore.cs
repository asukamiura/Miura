using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerCore : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    public Collider swordCollider;
    [SerializeField] GameObject playerCM;
    [SerializeField] GameObject justGuardCM;
    [SerializeField] private TextMeshProUGUI timingText;
    private HealthManager healthManager;
    private JustParryJudgement justParryJudgement;
    public StateMachine<PlayerStateID> stateMachine;
    public bool isBlock = false;
    InputReciver input => InputReciver.Instance;

    private void Awake()
    {
        stateMachine = new StateMachine<PlayerStateID>();
        stateMachine.RegisterState(new PlayerIdle(this, stateMachine));
        stateMachine.RegisterState(new PlayerMove(this, stateMachine));
        stateMachine.RegisterState(new PlayerDodge(this, stateMachine));
        stateMachine.RegisterState(new PlayerGuard(this, stateMachine));
        stateMachine.RegisterState(new PlayerBlock(this, stateMachine));
        //stateMachine.RegisterState(new PlayerParry(this, stateMachine));
        //stateMachine.RegisterState(new PlayerParrySuccess(this, stateMachine));
        stateMachine.RegisterState(new PlayerAttackNormal1(this, stateMachine));
        stateMachine.RegisterState(new PlayerAttackNormal2(this, stateMachine));
        stateMachine.RegisterState(new PlayerAttackNormal3(this, stateMachine));
        stateMachine.RegisterState(new PlayerAttackSpecial2(this, stateMachine));
        stateMachine.RegisterState(new PlayerDamage(this, stateMachine));
        stateMachine.RegisterState(new PlayerDead(this, stateMachine));
    }

    private void Start()
    {
        stateMachine.Initialize(PlayerStateID.Idle);
        healthManager = GetComponent<HealthManager>();
        justParryJudgement = GetComponent<JustParryJudgement>();
        timingText.enabled = false;
    }

    private void Update()
    {
        stateMachine.Update();
        
        animator.SetFloat("Speed", rb.velocity.magnitude, 0.1f, Time.deltaTime);

        if (healthManager.isDead && stateMachine.StateID != PlayerStateID.Dead )
        {
            stateMachine.ChangeState(PlayerStateID.Dead);
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(input.Guard && stateMachine.StateID != PlayerStateID.Guard && stateMachine.StateID != PlayerStateID.Damage)
        {
            stateMachine.ChangeState(PlayerStateID.Guard);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            if (stateMachine.StateID == PlayerStateID.Dead) { return; }
            if (stateMachine.StateID == PlayerStateID.Guard)
            {
                isBlock = true;
                return;
            }
            // 攻撃を受けたらダメージステートへ遷移
            stateMachine.ChangeState(PlayerStateID.Damage);
        }
    }

    public void AttackStart()
    {
        swordCollider.enabled = true;
    }

    public void AttackEnd()
    {
        swordCollider.enabled = false;
    }

    //public void ChangeCamera(bool restore)
    //{
    //    justGuardCM.SetActive(!restore);
    //    playerCM.SetActive(restore);
    //}

    public void TimingUIShow(string timing)
    {
        StartCoroutine(TimingUIChange(timing));
    }

    IEnumerator TimingUIChange(string timing)
    {
        timingText.text = timing;
        timingText.enabled = true;
        yield return new WaitForSeconds(1);
        timingText.enabled = false;
    }
}
