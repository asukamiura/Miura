using UnityEngine;

namespace Enemy
{
    public class DragonUsurperAttack2 : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Attack2;
        private DragonUsurperCore core;

        public DragonUsurperAttack2(DragonUsurperCore core)
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
                    core.stateMachine.ChangeState(DragonUsurperStateID.Idle);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
        }
    }
}

