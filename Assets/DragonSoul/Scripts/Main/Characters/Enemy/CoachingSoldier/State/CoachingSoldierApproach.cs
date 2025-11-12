using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierApproach : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.Approach;
        readonly CoachingSoldierCore core;
        Vector3 attackTargetPos;
        const float TargetDistance = 1.5f;
        const float MoveSpeed = 2.5f;

        public CoachingSoldierApproach(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.Animator.CrossFade("Run", 0);

            // 攻撃開始地点を設定
            attackTargetPos = core.playerTransform.position - core.transform.forward * TargetDistance;
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
                core.transform.position = Vector3.MoveTowards(core.transform.position, attackTargetPos, MoveSpeed * Time.deltaTime);
            }
        }

        public void Exit()
        {

        }
    }

}


