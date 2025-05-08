using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player
{
    public class PlayerAttackUltimate : AttackStateBase<PlayerStateID>
    {
        public override PlayerStateID StateID => PlayerStateID.AttackUltimate;

        public PlayerAttackUltimate(PlayerCore core) : base(core) { }

        protected override Dictionary<int, AttackAnimationConfig> AnimationData => new()
        {
            {1, new AttackAnimationConfig("AttackUltimate1", 0.1f, 0, 0, 1, AttackType.Ultimate) },
            {2, new AttackAnimationConfig("AttackUltimate2", 0, 0, 0, 1, AttackType.Ultimate) },
            {3, new AttackAnimationConfig("AttackUltimate3", 0, 0, 0, 1, AttackType.Ultimate) },
        };

        Vector3 effectPosition;

        public override void Enter()
        {
            core.playerEventManager.TriggerUltimateEnter();
            core.bodyCollider.enabled = false;
            core.IsInvincible = true;
            base.Enter();

            PostEffectManager.Instance.ChangePostEffect(PostEffectManager.ProfileNum.Ultimate, 0);
            core.animationController.ChangeAnimationSpeed("Enemy", 0);

            core.StartCoroutine(ChangeCamera());

            effectPosition = new Vector3(core.transform.position.x, core.transform.position.y, core.transform.position.z);
        }

        public override void Update()
        {
            if (core.CurrentStateInfo.normalizedTime >= AnimationData[step].nextStateTransitionTime && core.CurrentStateInfo.IsName(AnimationData[step].animationName))
            {
                step++;

                if (step > MaxStep)
                {
                    core.stateMachine.ChangeState(PlayerStateID.Locomotion);                    
                }
                else
                {
                    PlayCurrentAnimation();                                            
                }

                if (step == 3)
                {
                    CameraManager.Instance.SwitchCamera(CameraManager.CameraType.Ultimate3, 0.3f);
                    core.animationController.ChangeAnimationSpeed("Enemy", 1);
                }            
            }
            else if (core.CurrentStateInfo.normalizedTime >= 1 && core.CurrentStateInfo.IsName(AnimationData[step].animationName))
            {
                core.stateMachine.ChangeState(PlayerStateID.Locomotion);
            }
        }

        public override void FixedUpdate() { }

        public override void Exit()
        {
            base.Exit();
            core.IsInvincible = false;
            core.bodyCollider.enabled = true;
            PostEffectManager.Instance.ChangePostEffect(PostEffectManager.ProfileNum.Normal, 1);
            CameraManager.Instance.SwitchCamera(CameraManager.CameraType.Main, 1);
        }
      
        IEnumerator ChangeCamera()
        {
            CameraManager.Instance.SwitchCamera(CameraManager.CameraType.Ultimate1, 0);

            yield return new WaitForSeconds(0.1f);

            CameraManager.Instance.SwitchCamera(CameraManager.CameraType.Ultimate2, 1);
        }
    }
}
