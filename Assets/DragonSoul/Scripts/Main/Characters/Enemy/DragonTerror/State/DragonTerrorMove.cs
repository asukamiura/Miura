using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class DragonTerrorMove : IState<DragonTerrorStateID>
    {
        public DragonTerrorStateID StateID => DragonTerrorStateID.Move;

        readonly DragonTerrorCore core;
        float targetDistance;
        Vector3 targetPos;
        string animationName;
        DragonTerrorStateID selectedAttackState;

        readonly Dictionary<DragonTerrorStateID, float> attackStartDistances = new Dictionary<DragonTerrorStateID, float>()
        {
            { DragonTerrorStateID.AttackBite, 4},
            { DragonTerrorStateID.AttackClaw, 4},
            { DragonTerrorStateID.AttackBreath, 10},
        };

        const float RunMoveSpeed = 16;
        const float WalkMoveSpeed = 6;
        const float Acceleration = 16;

        public DragonTerrorMove(DragonTerrorCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // 攻撃タイプを抽選
            selectedAttackState = core.AttackSelector.ChooseAttack();

            foreach (var attackRange in attackStartDistances)
            {
                if (selectedAttackState == attackRange.Key)
                {
                    targetDistance = attackRange.Value;
                }
            }

            // 移動速度、加速度、止まる距離を設定
            core.NavMeshAgent.acceleration = Acceleration;

            if (core.DistanceToPlayer() > targetDistance)
            {
                core.NavMeshAgent.speed = RunMoveSpeed;
                core.NavMeshAgent.stoppingDistance = targetDistance;
                animationName = "RunFront";
                targetPos = core.playerTransform.position;
            }
            else
            {
                core.NavMeshAgent.speed = WalkMoveSpeed;
                core.NavMeshAgent.stoppingDistance = 2;
                animationName = "WalkBack";

                targetPos = core.playerTransform.position - core.transform.forward * targetDistance;

                if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, targetDistance, NavMesh.AllAreas))
                {
                    targetPos = hit.position;
                }
            }

            if (core.NavMeshAgent.enabled)
            {
                core.NavMeshAgent.SetDestination(targetPos);
            }
            core.Animator.CrossFade(animationName, 0.1f);
        }

        public void Update()
        {
            core.LookAtPlayer();

            if (Vector3.Distance(targetPos, core.transform.position) <= core.NavMeshAgent.stoppingDistance)
            {
                core.NavMeshAgent.ResetPath();
                core.stateMachine.ChangeState(selectedAttackState);
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.NavMeshAgent.speed = 0;
            core.NavMeshAgent.acceleration = 0;
            core.NavMeshAgent.velocity = Vector3.zero;
        }
    }
}


