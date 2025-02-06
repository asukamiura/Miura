using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierDamage : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.Damage;
        CoachingSoldierCore core;

        public CoachingSoldierDamage(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Damage", 0, 0, 0, 0);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Damage"))
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

