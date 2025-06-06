using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class DragonNightmareMove : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Move;
        DragonNightmareCore core;
        float targetDistance;

        const float WalkSpeed = 3;
        const float RunSpeed = 40;
        const float Acceleration = 50;
        const float ChangeMoveDistance = 8;
        readonly float[] attackRanges = { 4f, 4f, 4f };

        public DragonNightmareMove(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // 攻撃タイプを抽選
            core.attackType = core.ChooseAttack();

            // 攻撃を行う距離を設定
            targetDistance = attackRanges[core.attackType];

            // 移動速度、加速度、止まる距離を設定
            core.navMeshAgent.acceleration = Acceleration;
            core.navMeshAgent.stoppingDistance = targetDistance;

            if (core.DistanceToPlayer < ChangeMoveDistance)
            {
                core.animator.CrossFade("WalkFront", 0);
                core.navMeshAgent.speed = WalkSpeed;
            }
            else if (core.DistanceToPlayer >= ChangeMoveDistance)
            {
                core.navMeshAgent.speed = RunSpeed;
                core.animator.CrossFade("RunFront", 0);
            }
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("WalkFront") && core.DistanceToPlayer > ChangeMoveDistance)
            {
                core.navMeshAgent.speed = RunSpeed;
                core.animator.CrossFade("RunFront", 0);
            }

            core.LookAtPlayer();
            core.navMeshAgent.SetDestination(core.playerTransform.position);

            if (core.DistanceToPlayer <= core.navMeshAgent.stoppingDistance)
            {
                core.navMeshAgent.ResetPath();
                core.stateMachine.ChangeState(DragonNightmareStateID.Attack);
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


