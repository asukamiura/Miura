using UnityEngine;

namespace Player
{
    public class PlayerDamage : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Damage;
        private InputReciver input => InputReciver.Instance;
        private PlayerCore core;

        public PlayerDamage(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.AttackEnd();
            core.Animator.applyRootMotion = true;
            core.Animator.CrossFade("Damage", 0, 0, 0);
            //rb.AddForce(new Vector3(0,0,-50), ForceMode.Impulse);
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
