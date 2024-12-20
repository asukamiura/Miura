using UnityEngine;

namespace Enemy
{
    public class DragonUsurperLeave : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Leave;
        private DragonUsurperCore core;
        private float targetDistance = 3;
        private int attackType;
        private float leaveSpeed = 5;
        private Vector3 attackTargetPos;

        public DragonUsurperLeave(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("RunBack", 0);

            attackType = Random.Range((int)DragonUsurperStateID.Attack1, (int)DragonUsurperStateID.Attack3 + 1);
            switch (attackType)
            {
                case (int)DragonUsurperStateID.Attack1:
                    targetDistance = 5;
                    break;
                case (int)DragonUsurperStateID.Attack2:
                    targetDistance = 8;
                    break;
                case (int)DragonUsurperStateID.Attack3:
                    targetDistance = 10;
                    break;
                default:
                    break;
            }

            // 移動目標地点を設定
            attackTargetPos = core.playerTransform.position - core.transform.forward * targetDistance;
        }

        public void Update() { }

        public void FixedUpdate() 
        {
            if (core.transform.position == attackTargetPos)
            {
                switch (attackType)
                {
                    case (int)DragonUsurperStateID.Attack1:
                        core.stateMachine.ChangeState(DragonUsurperStateID.Attack1);
                        break;
                    case (int)DragonUsurperStateID.Attack2:
                        core.stateMachine.ChangeState(DragonUsurperStateID.Attack2);
                        break;
                    case (int)DragonUsurperStateID.Attack3:
                        core.stateMachine.ChangeState(DragonUsurperStateID.Attack3);
                        break;
                }
            }
            else
            {
                Vector3 direction = (core.playerTransform.position - core.transform.position).normalized;
                Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, lookAtRotation, core.rotationSpeed * Time.deltaTime);

                core.transform.position = Vector3.MoveTowards(core.transform.position, attackTargetPos, leaveSpeed * Time.deltaTime);
            }
        }

        public void Exit()
        {

        }
    }
}

