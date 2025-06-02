using UnityEngine;

namespace Player
{
    public class PlayerDamage : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Damage;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;

        const float KnockBackSpeed = 11;     // ノックバックスピード
        const float KnockBackDeceleration = 0.94f; // ノックバック減速率

        public PlayerDamage(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.attackAssist.CorrectionAttack();
            core.Animator.CrossFade("Damage", 0, 0, 0);
            core.Rb.velocity = -core.transform.forward * KnockBackSpeed;
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("Damage"))
            {
                if (core.CurrentStateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Locomotion);
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
