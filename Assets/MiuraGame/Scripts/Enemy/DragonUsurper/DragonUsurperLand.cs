using System.Globalization;
using UnityEngine;

namespace Enemy
{
    public class DragonUsurperLand : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Land;
        private DragonUsurperCore core;

        public DragonUsurperLand(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Land", 0.1f);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            // アニメーションが終わったらIdleStateに遷移
            if (stateInfo.IsName("Land"))
            {
                if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.TakeWarning);
                }
            }            
        }

        public void FixedUpdate() { }

        public void Exit() 
        {
            core.isFlying = false;
        }    
    }
}


