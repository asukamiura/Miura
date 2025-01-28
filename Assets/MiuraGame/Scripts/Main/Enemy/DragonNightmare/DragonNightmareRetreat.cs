using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class DragonNightmareRetreat : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Retreat;
        DragonNightmareCore core;
        float animationLength;
        float retreatSpeed;
        float distance;
        Vector3 retreatPointPos;
        float distanceToPlayer = 10;

        const float AnimationTransitionTime = 1;    // アニメーションが一回行われたときの時間
        const float RetreatStartTime = 0.4f;        // 動き始める時間
        const float RetreatEndTime = 0.9f;          // 動き終わる時間    

        public DragonNightmareRetreat(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {

            core.animator.CrossFade("Jump", 0);

            retreatPointPos = core.transform.position - core.transform.forward * distanceToPlayer;

            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            animationLength = stateInfo.length - (RetreatEndTime - RetreatStartTime);

            distance = Vector3.Distance(core.transform.position, retreatPointPos);

            // 移動スピードを計算
            retreatSpeed = distance / animationLength;
        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Jump"))
            {
                if (stateInfo.normalizedTime >= AnimationTransitionTime)
                {
                    core.stateMachine.ChangeState(DragonNightmareStateID.Approach);
                }
                else if (stateInfo.normalizedTime >= RetreatStartTime && stateInfo.normalizedTime <= RetreatEndTime)
                {
                    if (core.transform.position != retreatPointPos)
                    {
                        Vector3 direction = (core.playerTransform.position - core.transform.position).normalized;
                        Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
                        core.transform.rotation = Quaternion.Slerp(core.transform.rotation, lookAtRotation, core.rotationSpeed * Time.deltaTime);

                        core.transform.position = Vector3.MoveTowards(core.transform.position, retreatPointPos, retreatSpeed * Time.deltaTime);
                    }
                }
            }
        }

        public void Exit()
        {

        }
    }
}

