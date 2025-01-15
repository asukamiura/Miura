using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class DragonUsurperAttack : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Attack;
        private DragonUsurperCore core;

        // それぞれの攻撃のナンバー
        private const int Attack1Num = 1;
        private const int Attack2Num = 2;
        private const int Attack3Num = 3;

        private const float AttackRange = 3;    //攻撃開始範囲

        public DragonUsurperAttack(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // 攻撃をランダムに選択
            int attackType = Random.Range(Attack1Num, Attack3Num + 1);
            switch (attackType)
            {
                case Attack1Num:
                    core.animator.CrossFade("Attack1", 0);
                    break;
                case Attack2Num:
                    core.animator.CrossFade("Attack2", 0);
                    break;
                case Attack3Num:
                    core.animator.CrossFade("Attack3", 0);
                    break;
            }
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2") || stateInfo.IsName("Attack3"))
            {
                if (stateInfo.normalizedTime >= 1 && core.DistanceToPlayer <= AttackRange && core.IsPlayerInSight)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Attack);
                }
                else if (stateInfo.normalizedTime >= 1 && core.DistanceToPlayer > AttackRange)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.TakeWarning);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            //core.ResetAttackCollider();
        }
    }
}

