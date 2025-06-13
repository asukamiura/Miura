using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class DragonUsurperMove : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Move;

        DragonUsurperCore core;
        float targetDistance;
        Vector3 targetPos;
        string animationName;
        DragonUsurperStateID selectedAttackState;

        readonly Dictionary<DragonUsurperStateID, float> attackStartDistances = new Dictionary<DragonUsurperStateID, float>()
        {
            { DragonUsurperStateID.AttackBite, 6},
            { DragonUsurperStateID.AttackClaw, 8},
            { DragonUsurperStateID.AttackBreath, 10},
        };

        const float MoveSpeed = 8;
        const float Acceleration = 16;

        public DragonUsurperMove(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // 攻撃タイプを抽選
            selectedAttackState = core.attackSelector.ChooseAttack();

            foreach (var attackRange in attackStartDistances)
            {
                if (selectedAttackState == attackRange.Key)
                {
                    targetDistance = attackRange.Value;
                }
            }

            // 移動速度、加速度、止まる距離を設定
            core.navMeshAgent.speed = MoveSpeed;
            core.navMeshAgent.acceleration = Acceleration;
            ////core.navMeshAgent.stoppingDistance = targetDistance;

            if (core.DistanceToPlayer() > targetDistance)
            {
                core.navMeshAgent.stoppingDistance = targetDistance;
                animationName = "RunFront";
                targetPos = core.playerTransform.position;
            }
            else
            {
                core.navMeshAgent.stoppingDistance = 2;
                animationName = "RunBack";

                //Vector3 direction = (core.transform.position - core.playerTransform.position).normalized;

                targetPos = core.playerTransform.position - core.transform.forward * targetDistance;

                if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, targetDistance, NavMesh.AllAreas))
                {
                    targetPos = hit.position;
                }
            }

            if (core.navMeshAgent.enabled)
            {
                core.navMeshAgent.SetDestination(targetPos);
            }
            core.Animator.CrossFade(animationName, 0.1f);
        }

        public void Update()
        {
            core.LookAtPlayer();

            if (Vector3.Distance(targetPos, core.transform.position) <= core.navMeshAgent.stoppingDistance)
            {
                core.navMeshAgent.ResetPath();
                core.stateMachine.ChangeState(selectedAttackState);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.navMeshAgent.speed = 0;
            core.navMeshAgent.acceleration = 0;
            core.navMeshAgent.velocity = Vector3.zero;
        }
    }
}


