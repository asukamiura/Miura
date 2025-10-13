using UnityEngine;

namespace Enemy
{
    public class DragonUsurperAttackClaw : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.AttackClaw;

        DragonUsurperCore core;
        Vector3 targetPos;
        bool isPlayedEffect = false;
        Quaternion effectRotation = Quaternion.Euler(-90, 0, 0);

        const float TransitionDuration = 0.1f;  // アニメーションの遷移継続時間
        const float DefaultTransitionTime = 1f;        // アニメーションを遷移させる時間
        const float BlockedTransitionTime = 0.6f;        // アニメーションを遷移させる時間
        const float TrackingDuration = 0.26f;    // プレイヤーを追従する継続時間
        const float Acceleration = 100;
        const float TargetOffset = 2;    //攻撃開始範囲
        const float MoveTime = 0.13f;
        const float MoveStartTime = 0.4f;      // 移動を始める標準時間
        const float MoveEndTime = 0.54f;       // 移動を終える標準時間
        const float PlayEffectNormalizedTime = 0.5f;     // エフェクトを再生する標準時間

        public DragonUsurperAttackClaw(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            isPlayedEffect = false;

            core.effectPlayer.ShowEffect("CanGuardEffect");
            core.Animator.CrossFade("Attack2", TransitionDuration);

            targetPos = core.playerTransform.position - core.transform.forward * TargetOffset;
            core.NavMeshAgent.acceleration = Acceleration;
            core.NavMeshAgent.speed = Vector3.Distance(core.playerTransform.position, core.transform.position) / MoveTime;
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("Attack2"))
            {
                if (core.CurrentStateInfo.normalizedTime < TrackingDuration)
                {
                    core.LookAtPlayer();
                }

                if (core.CurrentStateInfo.normalizedTime >= MoveStartTime && core.CurrentStateInfo.normalizedTime <= MoveEndTime)
                {
                    core.NavMeshAgent.SetDestination(targetPos);
                }

                if (core.CurrentStateInfo.normalizedTime >= PlayEffectNormalizedTime && !isPlayedEffect)
                {
                    EffectManager.Instance.PlayEffect("EarthBlast", core.attack2EffectTransform.position, effectRotation);
                    isPlayedEffect = true;
                }

                if (core.isJustGuarded)
                {
                    core.NavMeshAgent.speed = 0;
                    core.NavMeshAgent.acceleration = 0;
                    core.NavMeshAgent.velocity = Vector3.zero;
                    targetPos = core.transform.position;
                }

                if (core.CurrentStateInfo.normalizedTime >= DefaultTransitionTime)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Move);
                }

                if (core.isJustGuarded && core.CurrentStateInfo.normalizedTime >= BlockedTransitionTime)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Idle);
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

