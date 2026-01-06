using UnityEngine;

namespace Player
{
    public class PlayerBlock : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Block;
        InputReciver Input => InputReciver.Instance;
        readonly PlayerCore core;
        readonly AnimationController animationController;
        bool isNextAttack = false;  // 特殊攻撃2を行う場合true,行わない場合false

        const string AnimationStateName = "Block";
        const float TransitionAttackSpecial2Threshold = 0.75f;  // 特殊攻撃2へ遷移を開始するアニメーションの進捗率
        const float TransitionLocomotionThreshold = 1;          // ロコモーションへ遷移を開始するアニメーションの進捗率
        const float KnockBackPower = 20;
        const float DecelerationRate = 0.95f; // 減速率

        public PlayerBlock(PlayerCore core, AnimationController animationController)
        {
            this.core = core;
            this.animationController = animationController;
        }

        public void Enter()
        {
            // プレイヤーを無敵状態にする
            core.IsInvincible = true;

            animationController.PlayAniamtion(AnimationStateName);

            // プレイヤーをノックバックさせる
            core.Rb.velocity = -core.transform.forward * KnockBackPower;
        }

        public void Update()
        {
            if (Input.AttackNormal)
            {
                isNextAttack = true;
            }

            if (animationController.IsTimeElapsed(AnimationStateName, TransitionAttackSpecial2Threshold) && isNextAttack)
            {
                core.StateMachine.ChangeState(PlayerStateID.AttackSpecial2);
            }
            else if (animationController.IsTimeElapsed(AnimationStateName, TransitionLocomotionThreshold) && !isNextAttack)
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
            core.Rb.velocity = Vector3.zero;
            core.IsInvincible = false;
            isNextAttack = false;
        }
    }
}
