using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierTakeWarning : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.TakeWarning;
        readonly CoachingSoldierCore core;
        float currentTime = 0;
        int moveDirection;
        const float WarningTime = 3;
        const int ResetPosY = 0;
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
            core.NavMeshAgent.acceleration = Acceleration;
            core.NavMeshAgent.stoppingDistance = StoppingDistance;
            core.SetBaseSpeed(WalkSpeed);

            core.transform.position = new Vector3(core.transform.position.x, ResetPosY, core.transform.position.z);
            moveDirection = Random.Range(1, 3);
            switch (moveDirection)
            {
                case 1:
                    core.Animator.CrossFade("WalkRight", 0);
                    break;
                case 2:
                    core.Animator.CrossFade("WalkLeft", 0);
                    break;
            }
        }

        public void Update() { }
        
        public void FixedUpdate() 
        {
            currentTime += Time.deltaTime;

            if (currentTime >= WarningTime)
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
                        core.NavMeshAgent.SetDestination(core.transform.position + core.transform.right * MoveOffset);
                        break;
                    case 2:
                        core.NavMeshAgent.SetDestination(core.transform.position + -core.transform.right * MoveOffset);
                        break;
                }
            }
        }

        public void Exit()
        {
            currentTime = 0;
            core.NavMeshAgent.speed = 0;
            core.NavMeshAgent.acceleration = 0;
            core.NavMeshAgent.velocity = Vector3.zero;
        }
    }
}

