using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class DragonUsurperTakeWarning : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.TakeWarning;
        DragonUsurperCore core;
        float targetDistance = 5;
        Vector3 targetPos;

        const float MoveSpeed = 2.5f;

        public DragonUsurperTakeWarning(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("WalkFront", 0.1f);

            // 目標地点を設定
            targetPos = core.playerTransform.position - core.transform.right * targetDistance;

            core.navMeshAgent.speed = MoveSpeed;
            core.navMeshAgent.acceleration = MoveSpeed * 2;
        }

        public void StateUpdate()
        {
            if (core.transform.position != targetPos)
            {
                //core.LookAtPlayer();

                core.navMeshAgent.SetDestination(targetPos);
            }
            else
            {
                core.stateMachine.ChangeState(DragonUsurperStateID.Move);
            }

            if (!NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 1, NavMesh.AllAreas))
            {
                core.navMeshAgent.ResetPath();
                core.stateMachine.ChangeState(DragonUsurperStateID.Move);
            }
        }

        public void StateFixedUpdate()
        {
        }

        public void Exit() { }
    }
}

