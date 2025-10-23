using UnityEngine;

namespace Player
{
    public class PlayerDodge : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Dodge;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;
        bool isNextAttack = false;  // 特殊攻撃1を行う場合true,行わない場合false

        const float NormalizedTimeOffset = 0.3f;
        const float TranstionAttackSpecialNormalizedTime = 0.75f;
        const float TranstionIdleNormalizedTime = 0.75f;
        const float KnockBackPower = 50;
        const float DecelerationRate = 0.9f; // 減速率

        public PlayerDodge(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // プレイヤーを無敵状態にする
            core.IsInvincible = true;

            core.Animator.CrossFade("Dodge", 0, 0, NormalizedTimeOffset);

            // プレイヤーをノックバックさせる
            Vector3 knockbackDir = (-core.transform.forward + -core.transform.right).normalized;
            core.Rb.velocity = knockbackDir * KnockBackPower;
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("Dodge"))
            {
                // 次攻撃の入力があった場合、特殊攻撃2に遷移
                if (core.CurrentStateInfo.normalizedTime >= TranstionAttackSpecialNormalizedTime && isNextAttack)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackSpecial1);
                }
                else if (core.CurrentStateInfo.normalizedTime >= TranstionIdleNormalizedTime && !isNextAttack)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Locomotion);
                }

                if (Input.AttackNormal)
                {
                    isNextAttack = true;
                }
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
