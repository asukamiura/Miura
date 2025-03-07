using UnityEngine;

namespace Player
{
    public class PlayerAttackNormal2 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackNormal2;
        PlayerCore core;
        InputReciver Input => InputReciver.Instance;
        bool isNextAttack = false;  // コンボ攻撃を継続フラグ

        public PlayerAttackNormal2(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.applyRootMotion = true;
            core.attackAssist.CorrectionAttack();
            // アニメーションの遷移
            core.Animator.CrossFade("AttackNormal2", 0.1f, 0, 0);
        }

        public void StateUpdate()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("AttackNormal2"))
            {
                if (Input.AttackNormal)
                {
                    isNextAttack = true;
                }

                if (isNextAttack && stateInfo.normalizedTime >= 0.6f)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackNormal3);
                }
                else if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Idle);
                }
            }
        }

        public void StateFixedUpdate() { }

        public void Exit()
        {
            core.Animator.applyRootMotion = false;
            isNextAttack = false;
        }
    }
}

