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
            core.playerEventManager.TriggerAttack();

            core.powerManager.SetAttackPower("Normal2");
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("AttackNormal2"))
            {
                if (Input.AttackNormal)
                {
                    isNextAttack = true;
                }

                if (isNextAttack && core.CurrentStateInfo.normalizedTime >= 0.6f)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackNormal3);
                }
                else if (core.CurrentStateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Idle);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.Animator.applyRootMotion = false;
            isNextAttack = false;
        }
    }
}

