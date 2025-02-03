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
            core.navMeshAgent.acceleration = Acceleration;
            core.navMeshAgent.stoppingDistance = AttackRange;

            if (core.DistanceToPlayer < ChangeMoveDistance)
            {
                core.animator.CrossFade("WalkFront", 0.1f);
                core.navMeshAgent.speed = WalkSpeed;
            }
            else if (core.DistanceToPlayer >= ChangeMoveDistance)
            {
                core.navMeshAgent.speed = RunSpeed;
                core.animator.CrossFade("RunFront", 0.1f);
            }
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("WalkFront") && core.DistanceToPlayer > ChangeMoveDistance)
            {
                core.navMeshAgent.speed = RunSpeed;
                core.animator.CrossFade("RunFront", 0.1f);
            }

            core.LookAtPlayer();
            core.navMeshAgent.SetDestination(core.playerTransform.position);

            if (core.DistanceToPlayer <= core.navMeshAgent.stoppingDistance)
            {
                core.navMeshAgent.ResetPath();
                core.stateMachine.ChangeState(CoachingSoldierStateID.Attack);
            }
        }

        public void FixedUpdate()
        {
        }

        public void Exit()
        {
            core.navMeshAgent.speed = 0;
            core.navMeshAgent.acceleration = 0;
            core.navMeshAgent.velocity = Vector3.zero;
        }
    }
}


