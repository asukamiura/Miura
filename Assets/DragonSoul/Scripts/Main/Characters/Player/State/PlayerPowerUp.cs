using UnityEngine;

namespace Player
{
    public class PlayerPowerUp : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.PowerUp;
        readonly PlayerCore core;
        readonly AnimationController animationController;
        const string AnimationStateName = "PowerUp";    
        const float TransitionThreshold = 1.0f;     // 遷移を開始するアニメーションの進捗率

        public PlayerPowerUp(PlayerCore core, AnimationController animationController)
        {
            this.core = core;
            this.animationController = animationController;
        }

        public void Enter()
        {
            core.IsInvincible = true;
            core.Rb.velocity = Vector3.zero;
            animationController.PlayAniamtion(AnimationStateName);
        }

        public void Update()
        {
            if (animationController.IsTimeElapsed(AnimationStateName, TransitionThreshold))
            {
                core.StateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }

        public void FixedUpdate() { }      

        public void Exit()
        {
            core.IsInvincible = false;
        }
    }
}
