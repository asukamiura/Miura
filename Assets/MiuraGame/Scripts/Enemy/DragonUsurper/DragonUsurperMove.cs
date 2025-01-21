using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class DragonUsurperMove : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Move;
        private DragonUsurperCore core;
        private float targetDistance;
        private Vector3 targetPos;
        private string animationName;

        private const float MoveSpeed = 8;
        private const float Acceleration = 16;
        private const float Attack1Range = 6;
        private const float Attack2Range = 8;
        private const float Attack3Range = 10;

        public DragonUsurperMove(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // 攻撃タイプを抽選
            core.attackType = core.ChooseAttack();

            // 攻撃を行う距離を設定
            targetDistance = core.attackType switch
            {
                0 => Attack1Range,
                1 => Attack2Range,
                2 => Attack3Range,
                _ => Attack1Range,
            };
           
            // 移動速度、加速度、止まる距離を設定
            core.navMeshAgent.speed = MoveSpeed;
            core.navMeshAgent.acceleration = Acceleration;
            //core.navMeshAgent.stoppingDistance = targetDistance;

            core.navMeshAgent.stoppingDistance = core.DistanceToPlayer > targetDistance ? targetDistance : 0;

            // アニメーションを設定
            animationName = core.DistanceToPlayer > targetDistance ? "RunFront" : "RunBack";
            core.animator.CrossFade(animationName, 0.1f);

            // ターゲット座標を設定
            targetPos = core.DistanceToPlayer > targetDistance ? core.playerTransform.position : core.playerTransform.position - core.transform.forward * targetDistance;
        }

        public void Update() 
        {            
            if (animationName == "RunFront")
            {
                Vector3 nextPoint = core.navMeshAgent.steeringTarget;
                Vector3 targetDirection = nextPoint - core.transform.position;

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                core.transform.rotation = Quaternion.RotateTowards(core.transform.rotation, targetRotation, 360f * Time.deltaTime);
            }

            core.navMeshAgent.SetDestination(targetPos);

            if (Vector3.Distance(targetPos, core.transform.position) <= core.navMeshAgent.stoppingDistance)
            {
                core.navMeshAgent.ResetPath();
                core.stateMachine.ChangeState(DragonUsurperStateID.Attack);
            }

            if (!NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 0, NavMesh.AllAreas))
            {
                core.navMeshAgent.ResetPath();
                core.stateMachine.ChangeState(DragonUsurperStateID.Attack);
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


