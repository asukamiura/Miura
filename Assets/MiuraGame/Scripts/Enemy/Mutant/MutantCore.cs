using UnityEngine;

public class MutantCore : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    public Collider attack1Collider;
    public Collider attack2Collider;
    public HealthManager healthManager;
    public StateMachine<MutantStateID> stateMachine;
    public GameObject player;

    private void Awake()
    {
        stateMachine = new StateMachine<MutantStateID>();

        stateMachine.RegisterState(new MutantIdle(this));
        stateMachine.RegisterState(new MutantMove(this));
        stateMachine.RegisterState(new MutantAttack1(this));
        stateMachine.RegisterState(new MutantAttack2(this));
        stateMachine.RegisterState(new MutantDamage(this));
        stateMachine.RegisterState(new MutantDie(this));
    }

    private void Start()
    {
        stateMachine.Initialize(MutantStateID.Idle);
        attack1Collider.enabled = false;
    }

    private void Update()
    {
        stateMachine.Update();

        if (healthManager.isDead && stateMachine.StateID != MutantStateID.Dead)
        {
            stateMachine.ChangeState(MutantStateID.Dead);
        }
    }

    public void AttackStart()
    {
        switch (stateMachine.StateID)
        {
            case MutantStateID.Attack1:
                attack1Collider.enabled = true;
                break;
            case MutantStateID.Attack2:
                attack2Collider.enabled = true;
                break;
        }
    }

    public void AttackEnd()
    {
        switch (stateMachine.StateID)
        {
            case MutantStateID.Attack1:
                attack1Collider.enabled = false;
                break;
            case MutantStateID.Attack2:
                attack2Collider.enabled = false;
                break;
        }
    }
}
