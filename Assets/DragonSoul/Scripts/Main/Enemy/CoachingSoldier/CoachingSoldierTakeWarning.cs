using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierTakeWarning : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.TakeWarning;
        private CoachingSoldierCore core;
        private float currentTime = 0;
        private const float warningTime = 3;
        private int moveDirection;
        private const int ResetPosY = 0;
        const float MoveOffset = 2;
        const float WalkSpeed = 1;
        const float Acceleration = 50;
        const float StoppingDistance = 0;

        public CoachingSoldierTakeWarning(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {            
            core.navMeshAgent.acceleration = Acceleration;
            core.navMeshAgent.stoppingDistance = StoppingDistance;
            core.SetBaseSpeed(WalkSpeed);

            core.transform.position = new Vector3(core.transform.position.x, ResetPosY, core.transform.position.z);
            moveDirection = Random.Range(1, 3);
            switch (moveDirection)
            {
                case 1:
                    core.animator.CrossFade("WalkRight", 0);
                    break;
                case 2:
                    core.animator.CrossFade("WalkLeft", 0);
                    break;
            }
        }

        public void Update() { }
        
        public void FixedUpdate() 
        {
            currentTime += Time.deltaTime;

            if (currentTime >= warningTime)
            {
                if (!core.CanAttack1 && !core.CanAttack2)
                {
                    core.stateMachine.ChangeState(CoachingSoldierStateID.TakeWarning);               
                }
                else
                {
                    core.stateMachine.ChangeState(CoachingSoldierStateID.Move);
                }
            }
            else
            {
                Vector3 direction = (core.playerTransform.position - core.transform.position).normalized;
                Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, lookAtRotation, core.rotationSpeed * Time.deltaTime);

                switch (moveDirection)
                {                   
                    case 1:
                        core.navMeshAgent.SetDestination(core.transform.position + core.transform.right * MoveOffset);
                        break;
                    case 2:
                        core.navMeshAgent.SetDestination(core.transform.position + -core.transform.right * MoveOffset);
                        break;
                }
            }
        }

        public void Exit()
        {
            currentTime = 0;
            core.navMeshAgent.speed = 0;
            core.navMeshAgent.acceleration = 0;
            core.navMeshAgent.velocity = Vector3.zero;
        }
    }
}

