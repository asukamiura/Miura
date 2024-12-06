using UnityEngine;

namespace Enemy
{
    public class DragonUsurperAttack1 : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Attack1;
        private DragonUsurperCore core;

        public DragonUsurperAttack1(DragonUsurperCore core)
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

