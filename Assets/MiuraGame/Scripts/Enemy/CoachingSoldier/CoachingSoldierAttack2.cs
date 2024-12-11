using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierAttack2 : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.Attack2;
        private CoachingSoldierCore core;

        public CoachingSoldierAttack2(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Attack2", 0.2f);
            core.IncreaseAttackCount(2);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack2"))
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
        }
    }
}

