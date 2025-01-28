using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierAttack1 : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.Attack1;
        CoachingSoldierCore core;

        public CoachingSoldierAttack1(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Attack1", 0);
            core.IncreaseAttackCount(1);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack1"))
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

