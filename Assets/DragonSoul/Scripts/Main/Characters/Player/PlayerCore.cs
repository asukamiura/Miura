using System;
using UnityEngine;

namespace Player
{
    public class PlayerCore : MonoBehaviour, ISlowable, IPlayerDamageable
    {
        [SerializeField] PlayerParameterData parameterData;
        [SerializeField] HealData healData;
        [SerializeField] PowerUpData powerUpData;
        [SerializeField] Animator animator;
        [SerializeField] Collider bodyCollider;
        [SerializeField] Rigidbody rb;
        [SerializeField] HealthManager healthManager;
        [SerializeField] JustPointManager justPointManager;
        [SerializeField] GameObject dodgeCollider;
        [SerializeField] TimingJudgement timingJudgement;
        [SerializeField] DashCooldownManager dashCooldownManager;
        [SerializeField] PowerManager powerManager;
        [SerializeField] UltimateManager ultimateManager;
        [SerializeField] PlayerAttack playerAttack;
        [SerializeField] AttackAssist attackAssist;

        GameObject currentDodgeCollider;
        InputReciver Input => InputReciver.Instance;

        const int GetJustPoint = 1;                  // 一回でのジャストポイント獲得量

        public StateMachine<PlayerStateID> StateMachine { get; private set; }
        public float MoveSpeed => parameterData.MoveSpeed;
        public Rigidbody Rb => rb;
        public Animator Animator => animator;
        public bool IsInvincible { get; set; } = false;   // 無敵状態フラグ
        public AnimatorStateInfo CurrentStateInfo { get; private set; }
        public PlayerStateID CurrentState => StateMachine.CurrentState;
        public Action OnHealed;
        public Action<string> OnDodgeTiming;
        public Action<string> OnGuardTiming;

        bool CanHeal => justPointManager.JustPoint >= healData.Cost;
        bool CanPowerUp => justPointManager.JustPoint >= powerUpData.Cost && StateMachine.CurrentState == PlayerStateID.Locomotion;
        bool CanUlt => ultimateManager.UltVal >= parameterData.UltCost
            && (StateMachine.CurrentState == PlayerStateID.Locomotion || StateMachine.CurrentState == PlayerStateID.AttackNormal1
            || StateMachine.CurrentState == PlayerStateID.AttackNormal2 || StateMachine.CurrentState == PlayerStateID.AttackNormal3);

        void Awake()
        {
            StateMachine = new StateMachine<PlayerStateID>();
            StateMachine.RegisterState(new PlayerLocomotion(this, dashCooldownManager));
            StateMachine.RegisterState(new PlayerDash(this, dashCooldownManager));
            StateMachine.RegisterState(new PlayerDodge(this));
            StateMachine.RegisterState(new PlayerGuard(this, attackAssist));
            StateMachine.RegisterState(new PlayerBlock(this));
            StateMachine.RegisterState(new PlayerAttackNormal1(this, playerAttack, attackAssist));
            StateMachine.RegisterState(new PlayerAttackNormal2(this, playerAttack, attackAssist));
            StateMachine.RegisterState(new PlayerAttackNormal3(this, playerAttack, attackAssist));
            StateMachine.RegisterState(new PlayerAttackSpecial1(this, playerAttack, attackAssist));
            StateMachine.RegisterState(new PlayerAttackSpecial2(this, playerAttack, attackAssist));
            StateMachine.RegisterState(new PlayerAttackUltimate(this, playerAttack, attackAssist));
            StateMachine.RegisterState(new PlayerPowerUp(this));
            StateMachine.RegisterState(new PlayerDamage(this, attackAssist));
            StateMachine.RegisterState(new PlayerDead(this));
        }

        void Start()
        {
            SlowManager.Instance.Register(this);
            // 最初のステートを設定
            StateMachine.Initialize(PlayerStateID.Locomotion);
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
            StateMachine?.UpdateState();

            CurrentStateInfo = Animator.GetCurrentAnimatorStateInfo(0);

            // 死亡していた場合、以降の処理を実行しない。
            if (StateMachine.CurrentState == PlayerStateID.Dead) { return; }

            // 回復処理を実行
            if (Input.Heal && CanHeal && healthManager.CurrentHP < healthManager.MaxHP)
            {
                justPointManager.UseJustPoint(healData.Cost);
                healthManager.RestoreHP(healData.HealVal);

                OnHealed?.Invoke();
            }

            // パワーアップ処理を実行
            if (Input.PowerUp && CanPowerUp && !powerManager.InPowerUp)
            {
                justPointManager.UseJustPoint(powerUpData.Cost);
                powerManager.ActionPowerUp();

                StateMachine.ChangeState(PlayerStateID.PowerUp);
            }

            // 必殺技を実行
            if (Input.AttackUltimate && CanUlt)
            {
                StateMachine.ChangeState(PlayerStateID.AttackUltimate);
                ultimateManager.DecreaseGauge(parameterData.UltCost);
            }
        }

        void FixedUpdate()
        {
            StateMachine?.FixedUpdateState();
        }

        public void TakeDamage(float damage, EnemyAttackType attackType, Action onJustGuarded)
        {
            if (StateMachine.CurrentState == PlayerStateID.Dead || IsInvincible) { return; }

            if (attackType == EnemyAttackType.Guardable && StateMachine.CurrentState == PlayerStateID.Guard)
            {
                HandleGuard(damage, onJustGuarded);
            }
            else
            {
                SetDamage(damage);
            }
        }

        /// <summary>
        /// ガード時のタイミング判定と処理を行う
        /// </summary>
        /// <param name="damage">受けるダメージ</param>
        /// <param name="onJustGuarded">相手にガードされたことを通知</param>
        void HandleGuard(float damage, Action onJustGuarded)
        {
            TimingType guardTiming = timingJudgement.JudgeGuard(CurrentStateInfo.normalizedTime);

            switch (guardTiming)
            {
                case TimingType.Fast:
                case TimingType.Late:
                    StateMachine.ChangeState(PlayerStateID.Block);
                    OnGuardTiming?.Invoke(guardTiming.ToString());
                    onJustGuarded?.Invoke();
                    break;
                case TimingType.Just:
                    // ジャストポイントを追加
                    justPointManager.AddJustPoint(GetJustPoint);                    
                    StateMachine.ChangeState(PlayerStateID.Block);
                    OnGuardTiming?.Invoke(guardTiming.ToString());
                    onJustGuarded?.Invoke();
                    break;
                case TimingType.Miss:
                    // ダメージ処理
                    SetDamage(damage);
                    break;
            }
        }

        /// <summary>
        /// 回避のタイミング判定と処理を行う
        /// </summary>
        /// <param name="elapsedTime">回避コライダーを表示してからの経過時間</param>
        public void HandleDodge(float elapsedTime)
        {
            TimingType dodgeTiming = timingJudgement.JudgeDodge(elapsedTime);

            switch (dodgeTiming)
            {
                case TimingType.Fast:
                case TimingType.Late:
                    StateMachine.ChangeState(PlayerStateID.Dodge);
                    OnGuardTiming?.Invoke(dodgeTiming.ToString());
                    break;
                case TimingType.Just:
                    // ジャストポイントを追加
                    justPointManager.AddJustPoint(GetJustPoint);
                    StateMachine.ChangeState(PlayerStateID.Dodge);
                    OnGuardTiming?.Invoke(dodgeTiming.ToString());
                    break;
            }
        }

        /// <summary>
        /// ダメージを受ける
        /// </summary>
        /// <param name="damage">受けるダメージ</param>
        void SetDamage(float damage)
        {
            healthManager.ReduceHP(damage);

            if (healthManager.IsDead)
            {
                StateMachine.ChangeState(PlayerStateID.Dead);
            }
            else
            {
                StateMachine.ChangeState(PlayerStateID.Damage);
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

        // Fast/Just/Late回避できるか判定するコライダーの生成
        public void CreateDodgeCollider()
        {
            currentDodgeCollider = ObjectPool.Instance.GetGameObject(dodgeCollider, transform.position, Quaternion.identity);
            currentDodgeCollider.GetComponent<JudgeDodgeController>().SetPlayerCore(this);
        }

        // Fast/Just/Late回避できるか判定するコライダーの削除
        public void DestroyDodgeCollider()
        {
            if (currentDodgeCollider == null) { return; }

            ObjectPool.Instance.ReleaseGameObject(currentDodgeCollider);
        }
    }
}
