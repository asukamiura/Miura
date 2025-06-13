using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierApproach : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.Approach;
        CoachingSoldierCore core;
        float targetDistance = 1.5f;
        float moveSpeed = 2.5f;
        Vector3 attackTargetPos;

        public CoachingSoldierApproach(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.CrossFade("Run", 0);

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
                core.LookAtPlayer();

                // 攻撃開始地点まで移動
                core.transform.position = Vector3.MoveTowards(core.transform.position, attackTargetPos, moveSpeed * Time.deltaTime);
            }
        }

        public void Exit()
        {

        }
    }

}


