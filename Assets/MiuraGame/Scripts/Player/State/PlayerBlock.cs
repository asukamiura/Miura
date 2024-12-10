using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerBlock : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Block;
        private InputReciver input => InputReciver.Instance;
        private PlayerCore core;
        private Vector3 moveTargetPos;
        private float targetDistance = 4;
        private float moveSpeed = 10;

        public PlayerBlock(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            //anim.applyRootMotion = true;
            core.isInvincible = true;
            core.Animator.CrossFade("Block", 0, 0, 0);
            //core.Rb.AddForce(-core.transform.forward * 20, ForceMode.Impulse);
            moveTargetPos = core.transform.position - core.transform.forward * targetDistance;
        }

        public void Update()
        {
            moveSpeed *= 0.99f;
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

        public void FixedUpdate() 
        {
            core.transform.position = Vector3.MoveTowards(core.transform.position, moveTargetPos, moveSpeed * Time.deltaTime);
        }

        public void Exit()
        {
            core.Rb.velocity = Vector3.zero;
            core.isJustGuard = false;
            core.isInvincible = false;
            moveSpeed = 10;
        }
    }
}
