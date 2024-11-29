using UnityEngine;

namespace Player
{
    public class PlayerBlock : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Block;
        private InputReciver input => InputReciver.Instance;
        private PlayerCore core;

        public PlayerBlock(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            //anim.applyRootMotion = true;
            core.Animator.CrossFade("Block", 0, 0, 0);
            core.Rb.AddForce(-core.transform.forward * 4, ForceMode.Impulse);
        }

        public void Update()
        {
            //rb.velocity *= 0.9f;
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1f)
            {
                core.stateMachine.ChangeState(PlayerStateID.Idle);
            }
            if (input.AttackNormal)
            {
                core.stateMachine.ChangeState(PlayerStateID.AttackSpecial2);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.Rb.velocity = Vector3.zero;
            core.isJustGuard = false;
        }
    }
}
