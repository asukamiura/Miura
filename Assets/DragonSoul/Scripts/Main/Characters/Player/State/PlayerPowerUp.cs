using UnityEngine;

namespace Player
{
    public class PlayerPowerUp : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.PowerUp;
        readonly PlayerCore core;


        public PlayerPowerUp(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            core.IsInvincible = true;
            core.Rb.velocity = Vector3.zero;
            core.Animator.CrossFade("PowerUp", 0);
        }

        public void Update()
        {
            if (core.CurrentStateInfo.IsName("PowerUp") && core.CurrentStateInfo.normalizedTime >= 1)
            {
                core.StateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }

        public void FixedUpdate()
        {

        }

        public void Exit()
        {
            core.IsInvincible = false;
        }
    }
}
