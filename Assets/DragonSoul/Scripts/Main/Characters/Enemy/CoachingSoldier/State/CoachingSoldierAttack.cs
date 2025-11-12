using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierAttack : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.Attack;
        readonly CoachingSoldierCore core;

        public CoachingSoldierAttack(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            if (core.CanAttack1)
            {
                core.Animator.CrossFade("Attack1", 0);
                core.warningEffectManager.ShowWarningEffect(EnemyAttackType.Guardable);
            }
            else if (core.CanAttack2)
            {
                core.Animator.CrossFade("Attack2", 0);
                core.warningEffectManager.ShowWarningEffect(EnemyAttackType.Dodgeable);
            }
            else
            {
                core.stateMachine.ChangeState(CoachingSoldierStateID.TakeWarning);
            }
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2"))
            {
                if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(CoachingSoldierStateID.TakeWarning);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.ResetAttackCollider();
        }
    }
}

