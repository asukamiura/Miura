using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class DragonTerrorAttack : IState<DragonTerrorStateID>
    {
        public DragonTerrorStateID StateID => DragonTerrorStateID.Attack;
        DragonTerrorCore core;
        bool isPlayedEffect1 = false;
        bool isPlayedEffect2 = false;
        Quaternion effectRotation = Quaternion.Euler(-90, 0, 0);

        // それぞれの攻撃のナンバー
        const int Attack1Num = 0;
        const int Attack2Num = 1;
        const int Attack3Num = 2;
        const float PlayerEffect1NormalizedTime = 0.4f;    // エフェクト1を再生する標準時間
        const float PlayerEffect2NormalizedTime = 0.7f;     // エフェクト2を再生する標準時間
        const float EffectShowingTime = 1f;

        public DragonTerrorAttack(DragonTerrorCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.navMeshAgent.stoppingDistance = 0;
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
                }

                if (stateInfo.IsName("Attack2")) 
                {
                    if (stateInfo.normalizedTime >= PlayerEffect1NormalizedTime && !isPlayedEffect1)
                    {
                        EffectGenerator.Instance.PlayEffect("EarthBlast", core.attack2EffectTransform.position, effectRotation, EffectShowingTime);
                        isPlayedEffect1 = true;
                    }

                    if (stateInfo.normalizedTime >= PlayerEffect2NormalizedTime && !isPlayedEffect2)
                    {
                        EffectGenerator.Instance.PlayEffect("EarthBlast", core.attack2EffectTransform.position, effectRotation, EffectShowingTime);
                        isPlayedEffect2 = true;
                    }
                }

                if (stateInfo.normalizedTime >= 1 && !core.isJustGuarded)
                {
                    core.stateMachine.ChangeState(DragonTerrorStateID.Move);
                }
                else if (stateInfo.normalizedTime >= 0.6 && core.isJustGuarded)
                {
                    core.stateMachine.ChangeState(DragonTerrorStateID.Idle);
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
            isPlayedEffect1 = false;
            isPlayedEffect2 = false;
            core.ResetAttackCollider();
            core.isJustGuarded = false;
        }
    }
}

