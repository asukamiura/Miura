namespace Enemy
{
    public class DragonUsurperAttackBite : IState<DragonUsurperStateID>
    {
        public DragonUsurperStateID StateID => DragonUsurperStateID.AttackBite;
        DragonUsurperCore core;
        const float TransitionDuration = 0.1f;  // アニメーションの遷移継続時間
        const float TransitionTime = 1f;        // アニメーションを遷移させる時間
        const float TrackingDuration = 0.4f;    // プレイヤーを追従する継続時間

        public DragonUsurperAttackBite(DragonUsurperCore core)
        {
            this.core = core;
        }

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

