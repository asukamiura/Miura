using System.Globalization;
using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierApproach : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.Approach;
        private CoachingSoldierCore core;
        private float targetDistance = 1.5f;
        private float moveSpeed = 2.5f;
        private Vector3 attackTargetPos;

        public CoachingSoldierApproach(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Run", 0);

            // 攻撃開始地点を設定
            attackTargetPos = core.playerTransform.position - core.transform.forward * targetDistance;
        }

        public void Update() { }     

        public void FixedUpdate() 
        {
            if (core.transform.position == attackTargetPos)
            {
                if (core.CanAttack1)
                {
                    core.stateMachine.ChangeState(CoachingSoldierStateID.Attack1);
                }
                else if (core.CanAttack2)
                {
                    core.stateMachine.ChangeState(CoachingSoldierStateID.Attack2);
                }
            }
            else
            {
                // プレイヤーの方向を向く
                Vector3 direction = (core.playerTransform.position - core.transform.position).normalized;
                Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
                core.transform.rotation = Quaternion.Slerp(core.transform.rotation, lookAtRotation, core.rotationSpeed * Time.deltaTime);

                // 攻撃開始地点まで移動
                core.transform.position = Vector3.MoveTowards(core.transform.position, attackTargetPos, moveSpeed * Time.deltaTime);
            }
        }

        public void Exit()
        {

        }
    }

}


