using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierAttack : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.Attack;
        CoachingSoldierCore core;

        public CoachingSoldierAttack(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            if (core.CanAttack1)
            {
                core.animator.CrossFade("Attack1", 0);
            }
            else if (core.CanAttack2)
            {
                core.animator.CrossFade("Attack2", 0);
            }
            else
            {
                core.stateMachine.ChangeState(CoachingSoldierStateID.TakeWarning);
            }
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
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

