using UnityEngine;

namespace Enemy
{
    public class DragonTerrorAttackClaw : IState<DragonTerrorStateID>
    {
        const float TransitionDuration = 0.1f;  // アニメーションの遷移継続時間
        const float TransitionTime = 1f;        // アニメーションを遷移させる時間
        const float TrackingDuration = 0.4f;    // プレイヤーを追従する継続時間
        const float PlayEffect1NormalizedTime = 0.4f;     // 攻撃2エフェクト1を再生する標準時間
        const float PlayEffect2NormalizedTime = 0.7f;     // 攻撃2エフェクト2を再生する標準時間

        bool isPlayedEffect1 = false;
        bool isPlayedEffect2 = false;
        Quaternion effectRotation = Quaternion.Euler(-90, 0, 0);

        readonly DragonTerrorCore core;

        public DragonTerrorAttackClaw(DragonTerrorCore core)
        {
            this.core = core;
        }

        public DragonTerrorStateID StateID => DragonTerrorStateID.AttackClaw;

        public void Enter()
        {
            isPlayedEffect1 = false;
            isPlayedEffect2 = false;

            core.warningEffectManager.ShowWarningEffect(EnemyAttackType.Dodgeable);
            core.Animator.CrossFade("Attack2", TransitionDuration);
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("Attack2"))
            {
                if (core.CurrentStateInfo.normalizedTime < TrackingDuration)
                {
                    core.LookAtPlayer();
                }

                if (core.CurrentStateInfo.normalizedTime >= TransitionTime)
                {
                    core.stateMachine.ChangeState(DragonTerrorStateID.Move);
                }

                if (core.CurrentStateInfo.normalizedTime >= PlayEffect1NormalizedTime && !isPlayedEffect1)
                {
                    EffectManager.Instance.PlayEffect("EarthBlast", core.attack2EffectTransform.position, effectRotation);
                    isPlayedEffect1 = true;
                }

                if (core.CurrentStateInfo.normalizedTime >= PlayEffect2NormalizedTime && !isPlayedEffect2)
                {
                    EffectManager.Instance.PlayEffect("EarthBlast", core.attack2EffectTransform.position, effectRotation);
                    isPlayedEffect2 = true;
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.ResetAttackCollider();
        }
    }
}

