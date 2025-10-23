using UnityEngine;

namespace Enemy
{
    public class DragonNightmareMove : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Move;
        DragonNightmareCore core;

        const float WalkSpeed = 3;
        const float RunSpeed = 40;
        const float Acceleration = 50;
        const float ChangeMoveDistance = 8;
        const float TargetDistance = 4;

        public DragonNightmareMove(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // 移動速度、加速度、止まる距離を設定
            core.NavMeshAgent.acceleration = Acceleration;
            core.NavMeshAgent.stoppingDistance = TargetDistance;

            if (core.DistanceToPlayer() < ChangeMoveDistance)
            {
                core.Animator.CrossFade("WalkFront", 0);
                core.NavMeshAgent.speed = WalkSpeed;
            }
            else if (core.DistanceToPlayer() >= ChangeMoveDistance)
            {
                core.NavMeshAgent.speed = RunSpeed;
                core.Animator.CrossFade("RunFront", 0);
            }
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("WalkFront") && core.DistanceToPlayer() > ChangeMoveDistance)
            {
                core.NavMeshAgent.speed = RunSpeed;
                core.Animator.CrossFade("RunFront", 0);
            }

            core.LookAtPlayer();
            core.NavMeshAgent.SetDestination(core.playerTransform.position);

            if (core.DistanceToPlayer() <= core.NavMeshAgent.stoppingDistance)
            {
                core.NavMeshAgent.ResetPath();
                core.stateMachine.ChangeState(core.AttackSelector.ChooseAttack());
            }
        }

        public void FixedUpdate()
        {
        }

        public void Exit()
        {
            core.NavMeshAgent.speed = 0;
            core.NavMeshAgent.acceleration = 0;
            core.NavMeshAgent.velocity = Vector3.zero;
        }
    }
}


