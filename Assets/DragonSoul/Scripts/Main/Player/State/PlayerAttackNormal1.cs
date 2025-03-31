using UnityEngine;

namespace Player
{
    public class PlayerAttackNormal1 : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.AttackNormal1;

        PlayerCore core;
        InputReciver Input => InputReciver.Instance;
        bool isNextAttack = false;

        public PlayerAttackNormal1(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.applyRootMotion = true;
            core.attackAssist.CorrectionAttack();
            core.Animator.CrossFade("AttackNormal1", 0.1f);
            core.playerEventManager.TriggerAttack();

            // 攻撃力を設定
            core.powerManager.SetAttackPower("Normal1");
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("AttackNormal1"))
            {
                if (Input.AttackNormal)
                {
                    isNextAttack = true;
                }

                if (isNextAttack && core.CurrentStateInfo.normalizedTime >= 0.6f)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackNormal2);
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
