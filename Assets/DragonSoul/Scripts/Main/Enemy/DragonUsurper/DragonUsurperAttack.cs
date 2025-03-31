using UnityEngine;

namespace Enemy
{
    public class DragonUsurperAttack : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.Attack;
        DragonUsurperCore core;
        Vector3 playerPos;
        bool isPlayedEffect = false;
        Quaternion effectRotation = Quaternion.Euler(-90, 0, 0);

        // それぞれの攻撃番号
        const int Attack1Num = 0;
        const int Attack2Num = 1;
        const int Attack3Num = 2;
        const float AttackRange = 2;    //攻撃開始範囲
        const float Attack2Acceleration = 100;
        const float MoveTime = 0.13f;
        const float MoveStartNormalizedTime = 0.4f;      // 移動を始める標準時間
        const float MoveEndNormalizedTime = 0.54f;       // 移動を終える標準時間
        const float PlayEffectNormalizedTime = 0.5f;     // エフェクトを再生する標準時間
        const float IndicateEffectShowingTime = 1;       // 攻撃を知らせるエフェクトを表示する時間

        public DragonUsurperAttack(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.navMeshAgent.stoppingDistance = 0;
            switch (core.attackType)
            {
                case Attack1Num:
                    core.effectPlayer.ShowEffect("CanGuardEffect", IndicateEffectShowingTime);
                    core.animator.CrossFade("Attack1", 0);
                    break;
                case Attack2Num:
                    core.effectPlayer.ShowEffect("CanGuardEffect", IndicateEffectShowingTime);
                    playerPos = core.playerTransform.position - core.transform.forward * AttackRange;
                    core.navMeshAgent.acceleration = Attack2Acceleration;
                    core.navMeshAgent.speed = Vector3.Distance(core.playerTransform.position, core.transform.position) / MoveTime;
                    core.animator.CrossFade("Attack2", 0);
                    break;
                case Attack3Num:
                    core.effectPlayer.ShowEffect("CanDodgeEffect", IndicateEffectShowingTime);
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
                if (stateInfo.IsName("Attack2"))
                {
                    if (stateInfo.normalizedTime >= MoveStartNormalizedTime && stateInfo.normalizedTime <= MoveEndNormalizedTime)
                    {
                        core.navMeshAgent.SetDestination(playerPos);
                    }

                    if (stateInfo.normalizedTime >= PlayEffectNormalizedTime && !isPlayedEffect)
                    {
                        EffectManager.Instance.PlayEffect("EarthBlast", core.attack2EffectTransform.position, effectRotation);
                        isPlayedEffect = true;
                    }
                }

                if (core.isJustGuarded)
                {
                    core.navMeshAgent.speed = 0;
                    core.navMeshAgent.acceleration = 0;
                    core.navMeshAgent.velocity = Vector3.zero;
                    playerPos = core.transform.position;
                }

                if (stateInfo.normalizedTime >= 1 && !core.isJustGuarded)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Move);
                }
                else if (stateInfo.normalizedTime >= 0.6 && core.isJustGuarded)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Idle);
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
            isPlayedEffect = false;
            core.ResetAttackCollider();
            core.isJustGuarded = false;
        }
    }
}

