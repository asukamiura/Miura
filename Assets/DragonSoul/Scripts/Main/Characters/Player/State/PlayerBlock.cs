using UnityEngine;

namespace Player
{
    public class PlayerBlock : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Block;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;
        bool isNextAttack = false;  // 特殊攻撃2を行う場合true,行わない場合false

        const float TranstionAttackSpecialNormalizedTime = 0.75f;
        const float TranstionIdleNormalizedTime = 1;
        const float KnockBackPower = 20;
        const float DecelerationRate = 0.95f; // 減速率

        public PlayerBlock(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // プレイヤーを無敵状態にする
            core.IsInvincible = true;

            core.Animator.CrossFade("Block", 0);

            // プレイヤーをノックバックさせる
            core.Rb.velocity = -core.transform.forward * KnockBackPower;
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("Block"))
            {
                // 次攻撃の入力があった場合、特殊攻撃2に遷移
                if (core.CurrentStateInfo.normalizedTime >= TranstionAttackSpecialNormalizedTime && isNextAttack)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackSpecial2);
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
            core.Rb.velocity = Vector3.zero;
            core.IsInvincible = false;
            isNextAttack = false;
        }
    }
}
