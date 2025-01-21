using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class DragonUsurperAttack : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Attack;
        private DragonUsurperCore core;
        private int attackType;
        private Vector3 playerPos;

        // それぞれの攻撃のナンバー
        private const int Attack1Num = 0;
        private const int Attack2Num = 1;
        private const int Attack3Num = 2;
        private const float AttackRange = 2;    //攻撃開始範囲
        private const float MoveTime = 0.13f;

        public DragonUsurperAttack(DragonUsurperCore core)
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
                    playerPos = core.playerTransform.position - core.transform.forward * AttackRange;
                    core.navMeshAgent.acceleration = 100;
                    core.navMeshAgent.speed = Vector3.Distance(core.playerTransform.position, core.transform.position) / MoveTime;
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
                if (stateInfo.IsName("Attack2") && stateInfo.normalizedTime >= 0.4f && stateInfo.normalizedTime <= 0.54f && !core.justGuard.isJustGuard)
                {
                    core.navMeshAgent.SetDestination(playerPos);
                }
                
                if (core.isJustGuarded) 
                {
                    core.navMeshAgent.speed = 0;
                    core.navMeshAgent.acceleration = 0;
                    core.navMeshAgent.velocity = Vector3.zero;
                    playerPos = core.transform.position;
                }

                if (stateInfo.normalizedTime >= 1 && !core.isJustGuarded)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Move);
                }
                else if (stateInfo.normalizedTime >= 0.6 && core.isJustGuarded)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Idle);
                }

                if (stateInfo.normalizedTime < 0.2f)
                {
                    core.LookAtPlayer();
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

