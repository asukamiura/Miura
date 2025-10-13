using SoundSystem;
using System;
using UnityEngine;

namespace Player
{
    public class PlayerCore : MonoBehaviour, ISlowable, IPlayerDamageable
    {
        [SerializeField] PlayerParameterData parameterData;
        [SerializeField] HealData healData;
        [SerializeField] PowerUpData powerUpData;
        [SerializeField] TimingJudgement timingJudgement;

        HealthManager healthManager;
        JustPointManager justPointManager;
        InputReciver Input => InputReciver.Instance;

        const int GetJustPoint = 1;                  // 一回でのジャストポイント獲得量

        public StateMachine<PlayerStateID> stateMachine;
        public DashCooldownManager dashCooldownManager;
        public PowerManager powerManager;
        public UltimateManager ultimateManager;
        public PlayerAttack playerAttack;
        public AttackAssist attackAssist;
        public GameSePlayer gameSePlayer;
        public EffectPlayer effectPlayer;
        public float MoveSpeed => parameterData.MoveSpeed;
        public Rigidbody Rb { get; set; }
        public Animator Animator { get; set; }
        public Collider bodyCollider;
        public bool IsInvincible { get; set; } = false;   // 無敵状態フラグ
        public AnimatorStateInfo CurrentStateInfo { get; private set; }
        public PlayerStateID CurrentState => stateMachine.CurrentState;
        public Action OnHeal;
        public Action<string> OnDodgeTiming;
        public Action<string> OnGuardTiming;

        bool CanHeal => justPointManager.JustPoint >= healData.Cost;
        bool CanPowerUp => justPointManager.JustPoint >= powerUpData.Cost && stateMachine.CurrentState == PlayerStateID.Locomotion;
        bool CanUlt => ultimateManager.UltVal >= parameterData.UltCost 
            && (stateMachine.CurrentState == PlayerStateID.Locomotion || stateMachine.CurrentState == PlayerStateID.AttackNormal1 
            || stateMachine.CurrentState == PlayerStateID.AttackNormal2 || stateMachine.CurrentState == PlayerStateID.AttackNormal3);

        void Awake()
        {
            stateMachine = new StateMachine<PlayerStateID>();
            stateMachine.RegisterState(new PlayerLocomotion(this));
            stateMachine.RegisterState(new PlayerDash(this));
            stateMachine.RegisterState(new PlayerDodge(this));
            stateMachine.RegisterState(new PlayerGuard(this));
            stateMachine.RegisterState(new PlayerBlock(this));
            stateMachine.RegisterState(new PlayerAttackNormal1(this));
            stateMachine.RegisterState(new PlayerAttackNormal2(this));
            stateMachine.RegisterState(new PlayerAttackNormal3(this));
            stateMachine.RegisterState(new PlayerAttackSpecial1(this));
            stateMachine.RegisterState(new PlayerAttackSpecial2(this));
            stateMachine.RegisterState(new PlayerAttackUltimate(this));
            stateMachine.RegisterState(new PlayerPowerUp(this));
            stateMachine.RegisterState(new PlayerDamage(this));
            stateMachine.RegisterState(new PlayerDead(this));
            Rb = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();
            healthManager = GetComponent<HealthManager>();
            justPointManager = GetComponent<JustPointManager>();
        }

        void Start()
        {
            SlowManager.Instance.Register(this);
            // 最初のステートを設定
            stateMachine.Initialize(PlayerStateID.Locomotion);   
            // HPを設定
            healthManager.SetMaxHP(parameterData.MaxHP);
            // 攻撃力、攻撃力アップ倍率、攻撃力アップ時間を設定
            powerManager.SetParameter(parameterData.AttackPower, powerUpData.Multiplier, powerUpData.Duration);
            // ジャストポイントを設定
            justPointManager.SetJustPoint(parameterData.JustPoint);
            // ダッシュのクールタイムを設定
            dashCooldownManager.SetCoolTime(parameterData.CoolTime);
        }

        void Update()
        {
            stateMachine?.UpdateState();

            CurrentStateInfo = Animator.GetCurrentAnimatorStateInfo(0);

            // 死亡していた場合、以降の処理を実行しない。
            if (stateMachine.CurrentState == PlayerStateID.Dead) { return; }            

            // 回復処理を実行
            if (Input.Heal && CanHeal && healthManager.CurrentHP < healthManager.MaxHP)
            {
                effectPlayer.ShowEffect("LifeEnchant");
                justPointManager.UseJustPoint(healData.Cost);
                healthManager.RestoreHP(healData.HealVal);

                OnHeal?.Invoke();
            }

            // パワーアップ処理を実行
            if (Input.PowerUp && CanPowerUp && !powerManager.InPowerUp)
            {
                justPointManager.UseJustPoint(powerUpData.Cost);
                powerManager.ActionPowerUp();

                stateMachine.ChangeState(PlayerStateID.PowerUp);
            }

            // 必殺技を実行
            if (Input.AttackUltimate && CanUlt)
            {
                stateMachine.ChangeState(PlayerStateID.AttackUltimate);
                ultimateManager.DecreaseGauge(parameterData.UltCost);
            }
        }

        void FixedUpdate()
        {
            stateMachine?.FixedUpdateState();
        }
    
        public void TakeDamage(float damage, EnemyAttackType attackType, IJustGuardable justGuardable)
        {
            if (stateMachine.CurrentState == PlayerStateID.Dead || IsInvincible) { return; }

            switch (attackType)
            {
                case EnemyAttackType.Dodgeable:
                    if (stateMachine.CurrentState == PlayerStateID.Dash)
                    {
                        switch (timingJudgement.JudgeDodge(CurrentStateInfo.normalizedTime))
                        {
                            case Timing.Fast:
                            case Timing.Late:
                                stateMachine.ChangeState(PlayerStateID.Dodge);
                                OnDodgeTiming?.Invoke(timingJudgement.JudgeDodge(CurrentStateInfo.normalizedTime).ToString());
                                break;
                            case Timing.Just:
                                justPointManager.AddJustPoint(GetJustPoint);
                                stateMachine.ChangeState(PlayerStateID.Dodge);
                                OnDodgeTiming?.Invoke(timingJudgement.JudgeDodge(CurrentStateInfo.normalizedTime).ToString());
                                break;
                            case Timing.Miss:
                                Damage(damage);
                                break;
                        }
                    }
                    else
                    {
                        Damage(damage);
                    }
                    break;

                case EnemyAttackType.Guardable:
                    if (stateMachine.CurrentState == PlayerStateID.Guard)
                    {
                        switch (timingJudgement.JudgeGuard(CurrentStateInfo.normalizedTime))
                        {
                            case Timing.Fast:
                            case Timing.Late:
                                stateMachine.ChangeState(PlayerStateID.Block);
                                OnGuardTiming?.Invoke(timingJudgement.JudgeGuard(CurrentStateInfo.normalizedTime).ToString());
                                justGuardable.OnJustGuarded();
                                break;
                            case Timing.Just:
                                justPointManager.AddJustPoint(GetJustPoint);
                                stateMachine.ChangeState(PlayerStateID.Block);
                                OnGuardTiming?.Invoke(timingJudgement.JudgeGuard(CurrentStateInfo.normalizedTime).ToString());
                                justGuardable.OnJustGuarded();
                                break;
                            case Timing.Miss:
                                Damage(damage);
                                break;
                        }
                    }
                    else
                    {
                        Damage(damage);
                    }
                    break;
            }
        }

        void Damage(float damageVal)
        {
            healthManager.ReduceHP(damageVal);
            if (healthManager.IsDead)
            {
                stateMachine.ChangeState(PlayerStateID.Dead);
            }
            else
            {
                stateMachine.ChangeState(PlayerStateID.Damage);
            }
        }

        public SlowTargetType Type => SlowTargetType.Player;

        public void ApplySlow(float factor)
        {
            Animator.speed = factor;
        }

        public void SetBaseSpeed(float speed)
        {
            Animator.speed = speed;
        }
    }
}
