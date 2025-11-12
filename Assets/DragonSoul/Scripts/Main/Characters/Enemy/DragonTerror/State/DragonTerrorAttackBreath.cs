namespace Enemy
{
    public class DragonTerrorAttackBreath : IState<DragonTerrorStateID>
    {
        const float TransitionDuration = 0.1f;  // アニメーションの遷移継続時間
        const float TransitionTime = 1f;        // アニメーションを遷移させる時間
        const float TrackingDuration = 0.4f;    // プレイヤーを追従する継続時間

        readonly DragonTerrorCore core;

        public DragonTerrorAttackBreath(DragonTerrorCore core)
        {
            this.core = core;
        }

        public DragonTerrorStateID StateID => DragonTerrorStateID.AttackBreath;

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
                    core.stateMachine.ChangeState(DragonTerrorStateID.Move);
                }
            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {

        }
    }
}

