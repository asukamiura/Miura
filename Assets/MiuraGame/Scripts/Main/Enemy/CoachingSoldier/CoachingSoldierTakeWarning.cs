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
        private float moveSpeed = 0.5f;
        private const int resetPosY = 0;

        public CoachingSoldierTakeWarning(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.transform.position = new Vector3(core.transform.position.x, resetPosY, core.transform.position.z);
            core.hitCount = 0;
            moveDirection = Random.Range(1, 3);
            switch (moveDirection)
            {
                case 1:
                    core.animator.CrossFade("WalkRight", 0.1f);
                    break;
                case 2:
                    core.animator.CrossFade("WalkLeft", 0.1f);
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
                        core.transform.position += core.transform.right * moveSpeed * Time.deltaTime;
                        break;
                    case 2:
                        core.transform.position -= core.transform.right * moveSpeed * Time.deltaTime;
                        break;
                }
            }
        }

        public void Exit()
        {
            currentTime = 0;
        }
    }
}

