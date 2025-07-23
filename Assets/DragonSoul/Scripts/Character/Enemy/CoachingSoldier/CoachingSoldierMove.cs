using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierMove : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.Move;
        CoachingSoldierCore core;

        const float WalkSpeed = 3;
        const float RunSpeed = 9;
        const float Acceleration = 50;
        const float ChangeMoveDistance = 8;
        const float AttackRange = 3;

        public CoachingSoldierMove(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // 移動速度、加速度、止まる距離を設定
            core.NavMeshAgent.acceleration = Acceleration;
            core.NavMeshAgent.stoppingDistance = AttackRange;

            if (core.DistanceToPlayer() < ChangeMoveDistance)
            {
                core.Animator.CrossFade("WalkFront", 0.1f);
                core.NavMeshAgent.speed = WalkSpeed;
            }
            else if (core.DistanceToPlayer() >= ChangeMoveDistance)
            {
                core.NavMeshAgent.speed = RunSpeed;
                core.Animator.CrossFade("RunFront", 0.1f);
            }
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("WalkFront") && core.DistanceToPlayer() > ChangeMoveDistance)
            {
                core.NavMeshAgent.speed = RunSpeed;
                core.Animator.CrossFade("RunFront", 0.1f);
            }

            core.LookAtPlayer();
            core.NavMeshAgent.SetDestination(core.playerTransform.position);

            if (core.DistanceToPlayer() <= core.NavMeshAgent.stoppingDistance)
            {
                core.NavMeshAgent.ResetPath();
                core.stateMachine.ChangeState(CoachingSoldierStateID.Attack);
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


