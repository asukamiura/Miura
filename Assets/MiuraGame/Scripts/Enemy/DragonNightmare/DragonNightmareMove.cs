using UnityEngine;

namespace Enemy
{
    public class DragonNightmareMove : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Move;
        private DragonNightmareCore core;
        private float animationLength;
        private float moveSpeed;
        private float currentDistance;
        private Vector3 JumpPointPos;

        private const float AnimationTransitionTime = 1;    // アニメーションが一回行われたときの時間
        private const float JumpStartTime = 0.4f;        // 動き始める時間
        private const float JumpEndTime = 0.9f;          // 動き終わる時間
        private const float AttackRange = 3.5f;
        private const int Move1Num = 1;
        private const int Move2Num = 2;
        private const int Move3Num = 3;


        public DragonNightmareMove(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.animator.CrossFade("Jump", 0);

            JumpPointPos = core.playerTransform.position + core.playerTransform.forward * AttackRange;

            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            animationLength = stateInfo.length - (JumpEndTime - JumpStartTime);

            currentDistance = Vector3.Distance(core.transform.position, JumpPointPos);

            // 移動スピードを計算
            moveSpeed =  currentDistance / animationLength;
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Jump"))
            {
                if (stateInfo.normalizedTime >= AnimationTransitionTime)
                {
                    core.stateMachine.ChangeState(DragonNightmareStateID.Attack);
                }
                else if (stateInfo.normalizedTime >= JumpStartTime && stateInfo.normalizedTime <= JumpEndTime)
                {
                    if (core.transform.position != JumpPointPos)
                    {                        
                        core.LookAtPlayer();

                        core.transform.position = Vector3.MoveTowards(core.transform.position, JumpPointPos, moveSpeed * Time.deltaTime);
                    }
                }
            }
        }

        public void FixedUpdate()
        {
        }

        public void Exit()
        {

        }
    }
}

