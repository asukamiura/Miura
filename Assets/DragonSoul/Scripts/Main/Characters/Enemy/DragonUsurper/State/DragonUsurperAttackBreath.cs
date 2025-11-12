namespace Enemy
{
    public class DragonUsurperAttackBreath : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.AttackBreath;
        readonly DragonUsurperCore core;
        const float TransitionDuration = 0;  // アニメーションの遷移継続時間
        const float TransitionTime = 1f;        // アニメーションを遷移させる時間
        const float TrackingDuration = 0.4f;    // プレイヤーを追従する継続時間

        public DragonUsurperAttackBreath(DragonUsurperCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.warningEffectManager.ShowWarningEffect(EnemyAttackType.Dodgeable);
            core.Animator.CrossFade("Attack3", TransitionDuration);
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("Attack3"))
            {
                if (core.CurrentStateInfo.normalizedTime < TrackingDuration)
                {
                    core.LookAtPlayer();
                }

                if (core.CurrentStateInfo.normalizedTime >= TransitionTime)
                {
                    core.stateMachine.ChangeState(DragonUsurperStateID.Move);
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

