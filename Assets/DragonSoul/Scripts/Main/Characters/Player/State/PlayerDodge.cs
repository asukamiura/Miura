using UnityEngine;

namespace Player
{
    public class PlayerDodge : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Dodge;
        InputReceiver Input => InputReceiver.Instance;
        readonly PlayerCore core;
        readonly AnimationController animationController;
        bool isNextAttack = false;  // 特殊攻撃1を行う場合true,行わない場合false

        const string AnimationStateName = "Dodge";
        const float NormalizedTimeOffset = 0.3f;                 // アニメーションを開始する時間
        const float TransitionAttackSpecial1Threshold = 0.75f;   // 特殊攻撃1へ遷移を開始するアニメーションの進捗率
        const float TranstionLocomotionThreshold = 0.75f;        // ロコモーションへ遷移を開始するアニメーションの進捗率
        const float KnockBackPower = 50;
        const float DecelerationRate = 0.9f; // 減速率

        public PlayerDodge(PlayerCore core, AnimationController animationController)
        {
            this.core = core;
            this.animationController = animationController;
        }

        public void Enter()
        {
            // プレイヤーを無敵状態にする
            core.IsInvincible = true;

            animationController.PlayAniamtion(AnimationStateName, timeOffset: NormalizedTimeOffset);

            // プレイヤーをノックバックさせる
            Vector3 knockbackDir = -core.transform.forward;
            core.Rb.velocity = knockbackDir * KnockBackPower;
        }

        public void Update()
        {
            if (Input.AttackNormal)
            {
                isNextAttack = true;
            }

            if (animationController.IsTimeElapsed(AnimationStateName, TransitionAttackSpecial1Threshold) && isNextAttack)
            {
                core.StateMachine.ChangeState(PlayerStateID.AttackSpecial1);
            }
            else if (animationController.IsTimeElapsed(AnimationStateName, TranstionLocomotionThreshold) && !isNextAttack)
            {
                core.StateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }

        public void FixedUpdate()
        {
            core.Rb.velocity *= DecelerationRate;
        }

        public void Exit()
        {
            core.IsInvincible = false;
            isNextAttack = false;
        }
    }
}
