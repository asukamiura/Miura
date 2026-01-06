using UnityEngine;

namespace Player
{
    public class PlayerDamage : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Damage;
        readonly PlayerCore core;
        readonly AnimationController animationController;
        readonly AttackAssist attackAssist;

        const float KnockBackSpeed = 10;     // ノックバックスピード
        const float KnockBackDeceleration = 0.94f; // ノックバック減速率
        const string AnimationStateName = "Damage";
        const float TransitionThreshold = 1.0f;     // 遷移を開始するアニメーションの進捗率

        public PlayerDamage(PlayerCore core, AnimationController animationController, AttackAssist attackAssist)
        {
            this.core = core;
            this.animationController = animationController;
            this.attackAssist = attackAssist;
        }

        public void Enter()
        {
            attackAssist.CorrectionAttack();
            animationController.SetRootMotion(false);
            animationController.PlayAniamtion(AnimationStateName);
            core.Rb.velocity = -core.transform.forward * KnockBackSpeed;
        }

        public void Update()
        {         
            if (animationController.IsTimeElapsed(AnimationStateName, TransitionThreshold))
            {
                core.StateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }

        public void FixedUpdate()
        {
            core.Rb.velocity *= KnockBackDeceleration;
        }

        public void Exit()
        {
            core.Rb.velocity = Vector3.zero;
        }
    }
}
