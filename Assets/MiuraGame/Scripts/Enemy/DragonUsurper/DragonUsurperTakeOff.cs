using UnityEngine;

namespace Enemy
{
    public class DragonUsurperTakeOff : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.TakeOff;
        private DragonUsurperCore core;

        public DragonUsurperTakeOff(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("TakeOff", 0.1f);
            core.isFlying = true;
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            // アニメーションが終わったらIdleStateに遷移
            if (stateInfo.IsName("TakeOff"))
            {
                if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.FlyAttack);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit() { }
    }
}


