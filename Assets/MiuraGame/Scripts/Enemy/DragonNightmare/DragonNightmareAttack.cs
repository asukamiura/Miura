using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class DragonNightmareAttack : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Attack;
        private DragonNightmareCore core;

        // それぞれの攻撃を開始するプレイヤーとの距離
        private const int Attack1Num = 0;    
        private const int Attack2Num = 1;
        private const int Attack3Num = 2;

        public DragonNightmareAttack(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.navMeshAgent.stoppingDistance = 0;
            switch (core.attackType)
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

            core.UpdateTotalWeight(core.attackType);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2") || stateInfo.IsName("Attack3"))
            {               
                if (stateInfo.normalizedTime >= 1 && core.isJustGuarded)
                {
                    core.stateMachine.ChangeState(DragonNightmareStateID.Idle);
                }
                else if (stateInfo.normalizedTime >= 1 && !core.isJustGuarded)
                {
                    core.stateMachine.ChangeState(DragonNightmareStateID.Move);
                }

                if (core.isJustGuarded)
                {
                    core.navMeshAgent.speed = 0;
                    core.navMeshAgent.acceleration = 0;
                    core.navMeshAgent.velocity = Vector3.zero;
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.ResetAttackCollider();
            core.isJustGuarded = false;
        }
    }
}

