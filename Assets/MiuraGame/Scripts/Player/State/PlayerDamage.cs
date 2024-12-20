using UnityEngine;

namespace Player
{
    public class PlayerDamage : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Damage;
        private InputReciver input => InputReciver.Instance;
        private PlayerCore core;

        private const float knockBackPower = 5;

        public PlayerDamage(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.AttackEnd();
            //core.Animator.applyRootMotion = true;
            core.Animator.CrossFade("Damage", 0, 0, 0);
            core.Rb.AddForce(-core.transform.forward * knockBackPower, ForceMode.Impulse);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Damage"))
            {
                if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Idle);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit() { }
    }
}
