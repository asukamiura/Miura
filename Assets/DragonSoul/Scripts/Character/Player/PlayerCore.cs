using SoundSystem;
using System;
using UnityEngine;

namespace Player
{
    public class PlayerCore : MonoBehaviour
    {
        [SerializeField] int healVal = 50;  // 回復量

        HealthManager healthManager;
        InputReciver Input => InputReciver.Instance;

        const int HealCost = 2;                      // 回復に必要なジャストポイント数
        const float HealEffectShowingTime = 1;       // 回復エフェクトの表示時間
        const int PowerUpCost = 3;                   // パワーアップに必要なジャストポイント数
        const int UltCost = 100;                     // 必殺技に必要なゲージ量

        public StateMachine<PlayerStateID> stateMachine;
        public JustPointManager justPointManager;
        public PowerManager powerManager;
        public UltimateManager ultimateManager;
        public ScoreManager scoreManager;
        public PlayerAttackManager attackManager;
        public AttackTypeHolder attackTypeHolder;
        public AttackAssist attackAssist;
        public GameSePlayer gameSePlayer;
        public EffectPlayer effectPlayer;
        public float MoveSpeed => powerManager.MoveSpeed;
        public Rigidbody Rb { get; set; }
        public Animator Animator { get; set; }
        public Collider bodyCollider;
        public bool IsJustGuard { get; set; } = false;
        public bool IsJustDodge { get; set; } = false;
        public bool IsInvincible { get; set; } = false;   // 無敵状態フラグ
        public AnimatorStateInfo CurrentStateInfo { get; private set; }
        public PlayerStateID CurrentState => stateMachine.CurrentState;
        public Action OnHeal;

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
            stateMachine.Initialize(PlayerStateID.Locomotion);
        }

        void Update()
        {
            stateMachine.UpdateState();

            CurrentStateInfo = Animator.GetCurrentAnimatorStateInfo(0);

            // 死亡していた場合、以降の処理を実行しない。
            if (stateMachine.CurrentState == PlayerStateID.Dead) { return; }

            // 死亡ステートに遷移
            if (healthManager.IsDead && stateMachine.CurrentState != PlayerStateID.Dead)
            {
                stateMachine.ChangeState(PlayerStateID.Dead);
            }

            // ガード、ブロック、回避、特殊攻撃1、特殊攻撃2、ダメージ状態でなければ実行可能
            if (stateMachine.CurrentState != PlayerStateID.Guard && stateMachine.CurrentState != PlayerStateID.Block
                && stateMachine.CurrentState != PlayerStateID.Dash && stateMachine.CurrentState != PlayerStateID.Dodge
                && stateMachine.CurrentState != PlayerStateID.Damage && stateMachine.CurrentState != PlayerStateID.AttackSpecial1
                && stateMachine.CurrentState != PlayerStateID.AttackSpecial2)
            {
                // ガードステートに遷移
                if (Input.Guard)
                {
                    stateMachine.ChangeState(PlayerStateID.Guard);
                }

                // 回避ステートに遷移
                if (Input.Dash)
                {
                    stateMachine.ChangeState(PlayerStateID.Dash);
                }
            }

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
            if (stateMachine.CurrentState == PlayerStateID.Dead || IsJustDodge || IsInvincible) { return; }

            var enemyAttackHit = other.GetComponent<EnemyAttackHit>();

            if (other.CompareTag("EnemyAttackCanGuard") || other.CompareTag("EnemyAttackCanDodge"))
            {
                if (other.CompareTag("EnemyAttackCanGuard") && stateMachine.CurrentState == PlayerStateID.Guard)
                {
                    IsJustGuard = true;                    
                }
                else if (other.CompareTag("EnemyAttackCanDodge") && stateMachine.CurrentState == PlayerStateID.Dash)
                {
                    IsJustDodge = true;
                }
                else
                {
                    // 攻撃アシストの位置補正処理を止める                   
                    attackAssist.StopAssist();

                    // 攻撃を受けたらダメージステートへ遷移
                    stateMachine.ChangeState(PlayerStateID.Damage);
                    scoreManager.SubtractScore((int)enemyAttackHit.damageVal);
                }
            }
        }
    }
}
