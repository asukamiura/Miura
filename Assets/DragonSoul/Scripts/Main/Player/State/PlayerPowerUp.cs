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
            core.isInvincible = true;
            core.Rb.velocity = Vector3.zero;
            core.Animator.CrossFade("PowerUp", 0);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("PowerUp"))
            {               
                if (!isEffective && stateInfo.normalizedTime >= 0.4)
                {
                    isEffective = true;
                    EffectGenerator.Instance.PlayEffect("VFX_Zap_02_Blue", core.transform.position, Quaternion.identity, 3);
                    EffectGenerator.Instance.PlayEffect("LightningBlueExplosion", core.transform.position, Quaternion.identity, 1);
                    EffectGenerator.Instance.PlayEffect("NovaLightningBlue", core.transform.position, Quaternion.Euler(-90, 0, 0), 1);
                    core.effectPlayer.PlayEffect("Lightning aura", 15);
                    core.StartCoroutine(ActiveForceField(15));
                }
                else if (stateInfo.normalizedTime >= 1)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Idle);
                }
            }
        }

        public void FixedUpdate()
        {

        }

        public void Exit()
        {
            isEffective = false;
            core.isInvincible = false;
        }

        IEnumerator ActiveForceField(float duration)
        {
            MaterialManager.Instance.IsActiveForceField = true;

            yield return new WaitForSeconds(duration);

            MaterialManager.Instance.IsDisabledForceField = true;
        }
    }
}
