using UnityEngine;

namespace Enemy
{
    public class DragonUsurperAttack3 : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Attack3;
        private DragonUsurperCore core;

        public DragonUsurperAttack3(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            if (core.isFlying)
            {
                core.animator.CrossFade("FlyAttack", 0);
            }
            else
            {
                core.animator.CrossFade("Attack3", 0);
            }

            core.IncreaseAttackCount(3);
        }

        public void Update()
        {        
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack3"))
            {
                if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Idle);
                }
            }
            if (stateInfo.IsName("FlyAttack"))
            {
                if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Land);
                }
            }
        }

        public void FixedUpdate() { }
       
        public void Exit() 
        {
        }
    }
}
