using UnityEngine;

namespace Enemy
{
    public class CoachingSoldierDamage : IState<CoachingSoldierStateID>
    {
        public CoachingSoldierStateID StateID => CoachingSoldierStateID.Damage;
        CoachingSoldierCore core;
        Vector3 moveTargetPos;
        float targetDistance = 0.5f;
        float moveSpeed = 10;
        float decelerationRate = 0.99f;

        const float defaultMoveSpeed = 10;

        public CoachingSoldierDamage(CoachingSoldierCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            //core.hitCount++;
            //switch (core.hitCount)
            //{
            //    case 1:
            //        core.animator.CrossFade("Damage1", 0, 0, 0);
            //        break;
            //    case 2:
            //        core.animator.CrossFade("Damage2", 0, 0, 0);
            //        break;
            //    case 3:
            //        core.animator.CrossFade("Damage3", 0, 0, 0);
            //        break;
            //}

            core.animator.CrossFade("Damage1", 0, 0, 0);


            moveTargetPos = core.transform.position - core.transform.forward * targetDistance;
        }

        public void Update()
        {
            //moveSpeed *= decelerationRate;
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Damage1") || stateInfo.IsName("Damage2") || stateInfo.IsName("Damage3"))
            {
                if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(CoachingSoldierStateID.TakeWarning);
                }
            }
        }

        public void FixedUpdate()
        {
            //core.transform.position = Vector3.MoveTowards(core.transform.position, moveTargetPos, moveSpeed * Time.deltaTime);
        }

        public void Exit()
        {
            moveSpeed = defaultMoveSpeed;
        }
    }
}

