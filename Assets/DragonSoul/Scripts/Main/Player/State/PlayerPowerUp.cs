using UnityEngine;
using System.Collections;

namespace Player
{
    public class PlayerPowerUp : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.PowerUp;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;

        bool isEffective = false;

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
            if (core.CurrentStateInfo.IsName("PowerUp"))
            {               
                if (!isEffective && core.CurrentStateInfo.normalizedTime >= 0.4)
                {
                    isEffective = true;
                    EffectManager.Instance.PlayEffect("VFX_Zap_02_Blue", core.transform.position, Quaternion.identity);
                    EffectManager.Instance.PlayEffect("NovaLightningBlue", core.transform.position, Quaternion.Euler(-90, 0, 0));
                    core.effectPlayer.ShowEffect("Lightning aura", 15);
                    core.StartCoroutine(ActiveForceField(15));
                }
                else if (core.CurrentStateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Locomotion);
                }
            }
        }

        public void FixedUpdate()
        {

        }

        public void Exit()
        {
            isEffective = false;
            core.IsInvincible = false;
        }

        IEnumerator ActiveForceField(float duration)
        {
            MaterialManager.Instance.IsActiveForceField = true;

            yield return new WaitForSeconds(duration);

            MaterialManager.Instance.IsDisabledForceField = true;
        }
    }
}
