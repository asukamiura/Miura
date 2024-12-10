using JetBrains.Annotations;
using UnityEngine;

namespace Enemy
{
    public class DragonUsurperTakeWarning : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.TakeWarning;
        private DragonUsurperCore core;
        private int moveDirection;
        private float moveSpeed = 2.5f;
        private float targetDistance = 5;
        private float rotationSpeed = 1;
        private Vector3 moveTargetPos;

        public DragonUsurperTakeWarning(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("WalkFront", 0.1f);

            // 目標地点を設定
            moveTargetPos = core.playerTransform.position - core.transform.right * targetDistance;
        }

        public void Update() { }
        
        public void FixedUpdate() 
        {          
            if (core.transform.position == moveTargetPos)
            {
                core.stateMachine.ChangeState(DragonUsurperStateID.Search);
            }
            else
            {
                // 移動方向を向く
                Vector3 direction = (moveTargetPos - core.transform.position).normalized;
                Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, lookAtRotation, rotationSpeed * Time.deltaTime);

                core.transform.position = Vector3.MoveTowards(core.transform.position, moveTargetPos, moveSpeed * Time.deltaTime);
            }
        }

        public void Exit() { }       
    }
}

