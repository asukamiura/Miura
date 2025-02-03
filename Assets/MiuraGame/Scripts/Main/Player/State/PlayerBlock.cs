using UnityEngine;

namespace Player
{
    public class PlayerBlock : IState<PlayerStateID>
    {
        public PlayerStateID StateID => PlayerStateID.Block;
        InputReciver Input => InputReciver.Instance;
        PlayerCore core;
        float currentTime = 0;      // 演出の効果時間計測用
        bool isEffective = false;   // ブロック演出中はtrue,それ以外はfalse
        bool isNextAttack = false;  // 特殊攻撃2を行う場合true,行わない場合false
            
        const float DefaultFOV = 70;            // 通常の視野角
        const float TargetFOV = 50;             // 演出時の視野角
        const float SpreadSpeed = 2;            // 視野角を広げる速度
        const float NarrowSpeed = 20;           // 視野角を狭める速度
        const float TargetDutch = 5;            // 演出時のカメラの角度
        const float DefaultDutch = 0;           // 通常のカメラの角度
        const float ChangeDutchSpeed1 = 20;     // カメラの角度を変える速度1
        const float ChangeDutchSpeed2 = 5;      // カメラの角度を変える速度2
        const float PerformanceAnimationSpeed = 0.3f;
        const float DefaultAnimationSpeed = 1;
        const float EffectiveTime = 1;          // 演出の効果時間
        const float TranstionAttackSpecialNormalizedTime = 0.75f;
        const float TranstionIdleNormalizedTime = 1;
        const float KnockBackPower = 10;
        const float DecelerationRate = 0.99f; // 減速率

        public PlayerBlock(PlayerCore core)
        {
            this.core = core;
        }

        public void Enter()
        {
            // プレイヤーを無敵状態にする
            core.isInvincible = true;

            core.Animator.CrossFade("Block", 0);

            // プレイヤーをノックバックさせる
            core.Rb.velocity = -core.transform.forward * KnockBackPower;

            // ブロック演出を開始
            isEffective = true;
            core.playerCameraController.RecenteringEnabled();
            core.playerCameraController.StartChangeDutch(TargetDutch, ChangeDutchSpeed1);
            core.playerCameraController.StartChangeFOV(TargetFOV, NarrowSpeed);
            core.playerCameraController.ApplyImpulse();
            core.animationController.ChangeAllAnimationSpeed(PerformanceAnimationSpeed);
        }

        public void Update()
        {          
            AnimatorStateInfo stateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Block"))
            {
                // 次攻撃の入力があった場合、特殊攻撃2に遷移
                if (stateInfo.normalizedTime >= TranstionAttackSpecialNormalizedTime && isNextAttack)
                {
                    core.stateMachine.ChangeState(PlayerStateID.AttackSpecial2);
                }
                else if (stateInfo.normalizedTime >= TranstionIdleNormalizedTime && !isNextAttack)
                {                   
                    core.stateMachine.ChangeState(PlayerStateID.Idle);
                }
                
                if (Input.AttackNormal)
                {
                    isNextAttack = true;
                }
            }

            if (isEffective)
            {
                // 効果時間が過ぎたら演出を終了
                if (currentTime >= EffectiveTime)
                {
                    core.playerCameraController.StartChangeFOV(DefaultFOV, SpreadSpeed);
                    core.playerCameraController.StartChangeDutch(DefaultDutch, ChangeDutchSpeed2);
                    core.playerCameraController.RecenteringDisabled();
                    core.animationController.ChangeAllAnimationSpeed(DefaultAnimationSpeed);
                    isEffective = false;
                }
                else
                {
                    currentTime += Time.deltaTime;
                }
            }
        }

        public void FixedUpdate() 
        {
            core.Rb.velocity *= DecelerationRate;
        }

        public void Exit()
        {            
            core.Rb.velocity = Vector3.zero;
            core.isJustGuard = false;
            core.isInvincible = false;
            currentTime = 0;
            isNextAttack = false;
        }
    }
}
