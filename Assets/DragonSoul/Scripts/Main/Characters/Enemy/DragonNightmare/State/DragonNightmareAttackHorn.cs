namespace Enemy
{
    public class DragonNightmareAttackHorn : IState<DragonNightmareStateID>
    {
        const float TransitionDuration = 0.1f;  // アニメーションの遷移継続時間
        const float TransitionTime = 1f;        // アニメーションを遷移させる時間
        const float TrackingDuration = 0.4f;    // プレイヤーを追従する継続時間

        readonly DragonNightmareCore core;

        public DragonNightmareAttackHorn(DragonNightmareCore core)
        {
            this.core = core;
        }

        public DragonNightmareStateID StateID => DragonNightmareStateID.AttackHorn;

        public void Enter()
        {
            core.warningEffectManager.ShowWarningEffect(EnemyAttackType.Guardable);
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
                    core.stateMachine.ChangeState(DragonNightmareStateID.Move);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {

        }
    }
}

