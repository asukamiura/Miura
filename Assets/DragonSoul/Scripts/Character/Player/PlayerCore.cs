using SoundSystem;
using System;
using UnityEngine;

namespace Player
{
    public class PlayerCore : MonoBehaviour, ISlowable
    {
        [SerializeField] int healVal = 50;  // 回復量
        [SerializeField] TimingJudgement timingJudgement;

        HealthManager healthManager;
        JustPointManager justPointManager;
        InputReciver Input => InputReciver.Instance;

        const int HealCost = 2;                      // 回復に必要なジャストポイント数
        const float HealEffectShowingTime = 1;       // 回復エフェクトの表示時間
        const int PowerUpCost = 3;                   // パワーアップに必要なジャストポイント数
        const int UltCost = 100;                     // 必殺技に必要なゲージ量
        const int GetJustPoint = 1;                  // 一回でのジャストポイント獲得量

        public StateMachine<PlayerStateID> stateMachine;
        public PowerManager powerManager;
        public UltimateManager ultimateManager;
        public AttackTypeHolder attackTypeHolder;
        public AttackAssist attackAssist;
        public GameSePlayer gameSePlayer;
        public EffectPlayer effectPlayer;
        public float MoveSpeed => powerManager.MoveSpeed;
        public Rigidbody Rb { get; set; }
        public Animator Animator { get; set; }
        public Collider bodyCollider;
        public bool IsInvincible { get; set; } = false;   // 無敵状態フラグ
        public AnimatorStateInfo CurrentStateInfo { get; private set; }
        public PlayerStateID CurrentState => stateMachine.CurrentState;
        public Action OnHeal;
        public Action<string> OnDodgeTiming;
        public Action<string> OnGuardTiming;

        bool CanHeal => justPointManager.JustPoint >= HealCost;
        bool CanPowerUp => justPointManager.JustPoint >= PowerUpCost && stateMachine.CurrentState == PlayerStateID.Locomotion;
        bool CanUlt => ultimateManager.UltVal >= UltCost 
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
            stateMachine.Initialize(PlayerStateID.Locomotion);
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
                effectPlayer.ShowEffect("LifeEnchant", HealEffectShowingTime);
                justPointManager.UseJustPoint(HealCost);
                healthManager.Heal(healVal);

                OnHeal?.Invoke();
            }

            // パワーアップ処理を実行
            if (Input.PowerUp && CanPowerUp && !powerManager.InPowerUp)
            {
                justPointManager.UseJustPoint(PowerUpCost);
                powerManager.ActionPowerUp();

                stateMachine.ChangeState(PlayerStateID.PowerUp);
            }

            // 必殺技を実行
            if (Input.AttackUltimate && CanUlt)
            {
                stateMachine.ChangeState(PlayerStateID.AttackUltimate);
                ultimateManager.DecreaseGauge(UltCost);
            }
        }

        void FixedUpdate()
        {
            stateMachine?.FixedUpdateState();
        }

        void OnTriggerEnter(Collider other)
        {
            if (stateMachine.CurrentState == PlayerStateID.Dead || IsInvincible) { return; }

            var attack = other.GetComponent<EnemyAttack>();

            if (attack.IsHit) { return; }

            switch (attack.attackType)
            {
                case EnemyAttack.EnemyAttackType.Dodgeable:
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
                            case Timing.None:
                                TakeDamage(attack.damageVal);
                                break;
                        }
                    }
                    else
                    {
                        TakeDamage(attack.damageVal);
                    }
                    break;

                case EnemyAttack.EnemyAttackType.Guardable:
                    if (stateMachine.CurrentState == PlayerStateID.Guard)
                    {
                        switch (timingJudgement.JudgeGuard(CurrentStateInfo.normalizedTime))
                        {
                            case Timing.Fast:
                            case Timing.Late:
                                stateMachine.ChangeState(PlayerStateID.Block);
                                OnGuardTiming?.Invoke(timingJudgement.JudgeGuard(CurrentStateInfo.normalizedTime).ToString());
                                break;
                            case Timing.Just:
                                justPointManager.AddJustPoint(GetJustPoint);
                                stateMachine.ChangeState(PlayerStateID.Block);
                                OnGuardTiming?.Invoke(timingJudgement.JudgeGuard(CurrentStateInfo.normalizedTime).ToString());
                                break;
                            case Timing.None:
                                TakeDamage(attack.damageVal);
                                break;
                        }
                    }
                    else
                    {
                        TakeDamage(attack.damageVal);
                    }
                    break;
            }            
        }

        void TakeDamage(float damageVal)
        {
            healthManager.Damage(damageVal);
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
