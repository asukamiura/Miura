using UnityEngine;

namespace Enemy
{
    public class DragonUsurperTakeWarning : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.TakeWarning;
        private DragonUsurperCore core;
        private float currentTime = 0;
        private const float restTime = 3;
        private int moveDirection;
        private float moveSpeed = 1;

        public DragonUsurperTakeWarning(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            moveDirection = Random.Range(0, 3);
            switch (moveDirection)
            {
                case 0:
                    core.animator.CrossFade("WalkBack", 0.1f);
                    break;
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

            if (currentTime >= restTime)
            {
                core.stateMachine.ChangeState(DragonUsurperStateID.Search);
            }
            else
            {
                Vector3 direction = (core.playerTransform.position - core.transform.position).normalized;
                Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, lookAtRotation, core.rotationSpeed * Time.deltaTime);

                switch (moveDirection)
                {
                    case 0:
                        core.transform.position -= core.transform.forward * moveSpeed * Time.deltaTime;
                        break;
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

