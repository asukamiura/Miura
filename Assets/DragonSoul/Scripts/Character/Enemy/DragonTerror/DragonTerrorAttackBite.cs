namespace Enemy
{
    public class DragonTerrorAttackBite : IState<DragonTerrorStateID>
    {
        const float TransitionDuration = 0.1f;  // アニメーションの遷移継続時間
        const float TransitionTime = 1f;        // アニメーションを遷移させる時間
        const float BlockedTransitionTime = 0.8f;        // アニメーションを遷移させる時間
        const float TrackingDuration = 0.4f;    // プレイヤーを追従する継続時間

        DragonTerrorCore core;

        public DragonTerrorAttackBite(DragonTerrorCore core)
        {
            this.core = core;
        }

        public DragonTerrorStateID StateID => DragonTerrorStateID.AttackBite;

        public void Enter()
        {
            core.effectPlayer.ShowEffect("CanGuardEffect");
            core.Animator.CrossFade("Attack1", TransitionDuration);
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("Attack1"))
            {
                if (core.CurrentStateInfo.normalizedTime < TrackingDuration)
                {
                    core.LookAtPlayer();
                }

                if (core.isJustGuarded && core.CurrentStateInfo.normalizedTime >= BlockedTransitionTime)
                {
                    core.stateMachine.ChangeState(DragonTerrorStateID.Idle);
                }

                if (!core.isJustGuarded && core.CurrentStateInfo.normalizedTime >= TransitionTime)
                {
                    core.stateMachine.ChangeState(DragonTerrorStateID.Move);
                }

            }
        }

        public void FixedUpdate() { }

        public void Exit()
        {
            core.isJustGuarded = false;
        }
    }
}

