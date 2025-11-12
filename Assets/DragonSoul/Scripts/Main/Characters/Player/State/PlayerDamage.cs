using UnityEngine;

namespace Player
{
    public class PlayerDamage : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Damage;
        readonly PlayerCore core;
        readonly AttackAssist attackAssist;

        const float KnockBackSpeed = 10;     // ノックバックスピード
        const float KnockBackDeceleration = 0.94f; // ノックバック減速率

        public PlayerDamage(PlayerCore core, AttackAssist attackAssist)
        {
            this.core = core;
            this.attackAssist = attackAssist;
        }

        public void Enter()
        {
            attackAssist.CorrectionAttack();
            core.Animator.CrossFade("Damage", 0);
            core.Rb.velocity = -core.transform.forward * KnockBackSpeed;
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("Damage"))
            {
                if (core.CurrentStateInfo.normalizedTime >= 1)
                {
                    core.StateMachine.ChangeState(PlayerStateID.Locomotion);
                }
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
