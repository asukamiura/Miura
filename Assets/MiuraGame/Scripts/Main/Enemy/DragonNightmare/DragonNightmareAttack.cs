using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class DragonNightmareAttack : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Attack;
        DragonNightmareCore core;
        Vector3 playerPos;

        // それぞれの攻撃番号
        const int Attack1Num = 0;
        const int Attack2Num = 1;
        const int Attack3Num = 2;
        const float PlayerEffect1NormalizedTime = 0.47f;    // エフェクト1を再生する標準時間
        const float PlayerEffect2NormalizedTime = 0.5f;     // エフェクト2を再生する標準時間
        const float PlayerEffect3NormalizedTime = 0.53f;    // エフェクト3を再生する標準時間
        const float EffectShowingTime = 2;                 // エフェクトを表示する時間

        public DragonNightmareAttack(DragonNightmareCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            switch (core.attackType)
            {
                case Attack1Num:
                    core.animator.CrossFade("Attack1", 0);
                    break;
                case Attack2Num:
                    core.animator.CrossFade("Attack2", 0);
                    break;
                case Attack3Num:
                    core.animator.CrossFade("Attack3", 0);
                    break;
            }

            core.UpdateTotalWeight(core.attackType);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2") || stateInfo.IsName("Attack3"))
            {
                if (core.isJustGuarded)
                {
                    core.navMeshAgent.speed = 0;
                    core.navMeshAgent.acceleration = 0;
                    core.navMeshAgent.velocity = Vector3.zero;
                    playerPos = core.transform.position;
                }

                if (stateInfo.IsName("Attack3"))
                {
                    if (stateInfo.normalizedTime >= PlayerEffect1NormalizedTime)
                    {
                        core.effectPlayer.PlayEffect("FireMuzzleBig1", EffectShowingTime);
                    }

                    if (stateInfo.normalizedTime >= PlayerEffect2NormalizedTime)
                    {
                        core.effectPlayer.PlayEffect("FireMuzzleBig2", EffectShowingTime);
                    }

                    if (stateInfo.normalizedTime >= PlayerEffect3NormalizedTime)
                    {
                        core.effectPlayer.PlayEffect("FireMuzzleBig3", EffectShowingTime);
                    }
                }


                if (stateInfo.normalizedTime >= 1 && !core.isJustGuarded)
                {
                    core.stateMachine.ChangeState(DragonNightmareStateID.Move);
                }
                else if (stateInfo.normalizedTime >= 0.6 && core.isJustGuarded)
                {
                    core.stateMachine.ChangeState(DragonNightmareStateID.Idle);
                }

                if (stateInfo.normalizedTime < 0.2f)
                {
                    core.LookAtPlayer();
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.ResetAttackCollider();
            core.isJustGuarded = false;
        }
    }
}

