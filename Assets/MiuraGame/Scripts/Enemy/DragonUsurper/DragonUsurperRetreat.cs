using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class DragonUsurperRetreat : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Retreat;
        private DragonUsurperCore core;

        private const string retreatAnimationName = "Jump";
        private const float animationTransitionTime = 1;
        private const float retreatSpeed = 10f;
        private const float retreatStartTime = 0.4f;
        private const float retreatEndTime = 0.9f;

        public DragonUsurperRetreat(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.ResetAttackCount();

            core.animator.CrossFade(retreatAnimationName, 0);
        }

        public void Update()
        {
            
        }

        public void FixedUpdate() 
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(retreatAnimationName))
            {
                if (stateInfo.normalizedTime >= animationTransitionTime)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Approach);
                }
                else if (stateInfo.normalizedTime >= retreatStartTime && stateInfo.normalizedTime <= retreatEndTime)
                {
                    core.transform.position -= core.transform.forward * retreatSpeed * Time.deltaTime;
                }
            }
        }

        public void Exit()
        {

        }
    }
}

