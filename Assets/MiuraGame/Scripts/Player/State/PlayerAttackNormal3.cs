using UnityEngine;

namespace Player
{
    public class PlayerAttackNormal3 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackNormal3;
        private PlayerCore core;
        private InputReciver input => InputReciver.Instance;

        public PlayerAttackNormal3(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // アニメーションの遷移
            core.Animator.CrossFade("AttackNormal3", 0.1f, 0, 0.1f);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            // アニメーションが終わったらIdleStateに遷移
            if (stateInfo.normalizedTime >= 1)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {

        }
    }
}
