using UnityEngine;

namespace Player
{
    public class PlayerAttackUltimate : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackUltimate;
        private PlayerCore core;
        private InputReciver Input => InputReciver.Instance;

        public PlayerAttackUltimate(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.applyRootMotion = true;
            core.isInvincible = true;
            // アニメーションの遷移
            core.Animator.CrossFade("AttackUltimate",0);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            // アニメーションが終わったらIdleStateに遷移
            if (stateInfo.normalizedTime >= 0.8f)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.Animator.applyRootMotion = false;
            core.isInvincible = false;
        }
    }
}
