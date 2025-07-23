namespace Enemy
{
    public class DragonNightmareAttackBite : IState<DragonNightmareStateID>
    {
        const float TransitionDuration = 0.1f;  // アニメーションの遷移継続時間
        const float TransitionTime = 1f;        // アニメーションを遷移させる時間
        const float TrackingDuration = 0.4f;    // プレイヤーを追従する継続時間

        DragonNightmareCore core;

        public DragonNightmareAttackBite(DragonNightmareCore core)
        {
            this.core = core;
        }

        public DragonNightmareStateID StateID => DragonNightmareStateID.AttackBite;

        public void Enter()
        {
            core.effectPlayer.ShowEffect("CanDodgeEffect");
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

