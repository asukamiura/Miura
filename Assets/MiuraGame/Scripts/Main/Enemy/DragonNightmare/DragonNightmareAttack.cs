using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class DragonNightmareAttack : IState<DragonNightmareStateID>
    {
        public DragonNightmareStateID StateID => DragonNightmareStateID.Attack;
        DragonNightmareCore core;
        Vector3 playerPos;
        bool isPlayedEffect1 = false;
        bool isPlayedEffect2 = false;
        bool isPlayedEffect3 = false;
        Quaternion effectRotation = Quaternion.Euler(-90, 0, 0);

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
                // ガードされた場合、速度を変更
                if (core.isJustGuarded)
                {
                    core.navMeshAgent.speed = 0;
                    core.navMeshAgent.acceleration = 0;
                    core.navMeshAgent.velocity = Vector3.zero;
                    playerPos = core.transform.position;
                }

                // エフェクトの再生
                if (stateInfo.IsName("Attack3"))
                {
                    if (stateInfo.normalizedTime >= PlayerEffect1NormalizedTime && !isPlayedEffect1)
                    {
                        EffectGenerator.Instance.PlayEffect("FireMuzzleBig", core.attack3EffectTransform.position, effectRotation, EffectShowingTime);
                        isPlayedEffect1 = true;
                    }

                    if (stateInfo.normalizedTime >= PlayerEffect2NormalizedTime && !isPlayedEffect2)
                    {
                        EffectGenerator.Instance.PlayEffect("FireMuzzleBig", core.attack3EffectTransform.position, effectRotation, EffectShowingTime);
                        isPlayedEffect2 = true;
                    }

                    if (stateInfo.normalizedTime >= PlayerEffect3NormalizedTime && !isPlayedEffect3)
                    {
                        EffectGenerator.Instance.PlayEffect("FireMuzzleBig", core.attack3EffectTransform.position, effectRotation, EffectShowingTime);
                        isPlayedEffect3 = true;
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
            isPlayedEffect1 = false;
            isPlayedEffect2 = false;
            isPlayedEffect3 = false;
        }
    }
}

