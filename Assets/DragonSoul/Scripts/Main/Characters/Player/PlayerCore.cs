using System;
using UnityEngine;

namespace Player
{
    public class PlayerCore : MonoBehaviour, ISlowable, IPlayerDamageable
    {
        [SerializeField] PlayerParameterData parameterData;       // 初期パラメーターデータ
        [SerializeField] HealData healData;                       // ヒールアクションのデータ
        [SerializeField] PowerUpData powerUpData;                 // パワーアップアクションのデータ
        [SerializeField] Transform centerTransform;
        [SerializeField] Rigidbody rb;
        [SerializeField] HealthManager healthManager;             // HPの管理
        [SerializeField] JustPointManager justPointManager;       // ジャストポイントの管理
        [SerializeField] GameObject dodgeCollider;                // ジャスト回避を判定するコライダー
        [SerializeField] TimingJudgement timingJudgement;         // 回避とガードの行われたタイミングを判定する
        [SerializeField] DashCooldownManager dashCooldownManager; // ダッシュのクールダウンの管理
        [SerializeField] AttackPowerManager attackPowerManager;   // 攻撃力の管理
        [SerializeField] UltimateManager ultimateManager;         // 必殺技ゲージの管理
        [SerializeField] AnimationController animationController;
        [SerializeField] PlayerAttackNormalController normalAttackController;       // 通常攻撃の管理
        [SerializeField] PlayerAttackSpecial1Controller specialAttack1Controller;   // 回避反撃の管理
        [SerializeField] PlayerAttackSpecial2Controller specialAttack2Controller;   // ガード反撃の管理
        [SerializeField] PlayerAttackUltimateController ultimateAttackController;   // 必殺技の管理
        [SerializeField] AttackAssist attackAssist;               // 攻撃アシストの管理

        GameObject currentDodgeCollider;
        InputReceiver Input => InputReceiver.Instance;

        const int GetJustPoint = 1;                  // 一回でのジャストポイント獲得量

        public StateMachine<PlayerStateID> StateMachine { get; private set; }
        public float MoveSpeed => parameterData.MoveSpeed;                   // 移動速度
        public float AttackPower => attackPowerManager.CurrentAttackPower;   // 現在の攻撃力
        public Transform CenterTransform => centerTransform;
        public Rigidbody Rb => rb;
        public bool InPowerUp => attackPowerManager.InPowerUp;
        public bool IsInvincible { get; set; } = false;   // 無敵状態フラグ
        public PlayerStateID CurrentState => StateMachine.CurrentState;
        public Action OnHealed;                     // 回復したことを通知するイベント
        public Action<TimingType> OnDodgeTiming;    // どの判定されたタイミングを通知するイベント
        public Action<TimingType> OnGuardTiming;

        bool CanHeal => justPointManager.JustPoint >= healData.Cost;
        bool CanPowerUp => justPointManager.JustPoint >= powerUpData.Cost && StateMachine.CurrentState == PlayerStateID.Locomotion;
        bool CanUlt => ultimateManager.UltVal >= parameterData.UltCost
            && (StateMachine.CurrentState == PlayerStateID.Locomotion || StateMachine.CurrentState == PlayerStateID.AttackNormal1
            || StateMachine.CurrentState == PlayerStateID.AttackNormal2 || StateMachine.CurrentState == PlayerStateID.AttackNormal3);

        void Awake()
        {
            StateMachine = new StateMachine<PlayerStateID>();
            StateMachine.RegisterState(new PlayerLocomotion(this, animationController, dashCooldownManager));
            StateMachine.RegisterState(new PlayerDash(this, animationController, dashCooldownManager));
            StateMachine.RegisterState(new PlayerDodge(this, animationController));
            StateMachine.RegisterState(new PlayerGuard(this, animationController, attackAssist));
            StateMachine.RegisterState(new PlayerBlock(this, animationController));
            StateMachine.RegisterState(new PlayerAttackNormal(this, animationController, normalAttackController, attackAssist));
            StateMachine.RegisterState(new PlayerAttackSpecial1(this, animationController, specialAttack1Controller));
            StateMachine.RegisterState(new PlayerAttackSpecial2(this, animationController, specialAttack2Controller));
            StateMachine.RegisterState(new PlayerAttackUltimate(this, animationController, ultimateAttackController));
            StateMachine.RegisterState(new PlayerPowerUp(this, animationController));
            StateMachine.RegisterState(new PlayerDamage(this, animationController, attackAssist));
            StateMachine.RegisterState(new PlayerDead(this, animationController));
        }

        void Start()
        {
            SlowManager.Instance.Register(this);
            // 最初のステートを設定
            StateMachine.Initialize(PlayerStateID.Locomotion);
            // HPを設定
            healthManager.SetMaxHP(parameterData.MaxHP);
            // 攻撃力、攻撃力アップ倍率、攻撃力アップ時間を設定
            attackPowerManager.SetParameter(parameterData.AttackPower, powerUpData.Multiplier, powerUpData.Duration);
            // ジャストポイントを設定
            justPointManager.SetJustPoint(parameterData.JustPoint);
            // ダッシュのクールタイムを設定
            dashCooldownManager.SetCoolTime(parameterData.CoolTime);
        }

        void Update()
        {
            StateMachine?.UpdateState();

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
            if (Input.PowerUp && CanPowerUp && !attackPowerManager.InPowerUp)
            {
                justPointManager.UseJustPoint(powerUpData.Cost);
                attackPowerManager.ActionPowerUp();

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
            TimingType guardTiming = timingJudgement.JudgeGuard(animationController.StateInfo.normalizedTime);

            switch (guardTiming)
            {
                case TimingType.Fast:
                case TimingType.Late:
                    StateMachine.ChangeState(PlayerStateID.Block);
                    OnGuardTiming?.Invoke(guardTiming);
                    onJustGuarded?.Invoke();
                    break;
                case TimingType.Just:
                    // ジャストポイントを追加
                    justPointManager.AddJustPoint(GetJustPoint);
                    StateMachine.ChangeState(PlayerStateID.Block);
                    OnGuardTiming?.Invoke(guardTiming);
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
                    OnDodgeTiming?.Invoke(dodgeTiming);
                    break;
                case TimingType.Just:
                    // ジャストポイントを追加
                    justPointManager.AddJustPoint(GetJustPoint);
                    OnDodgeTiming?.Invoke(dodgeTiming);
                    break;
            }

            StateMachine.ChangeState(PlayerStateID.Dodge);
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
            animationController.ChangeAnimationSpeed(factor);
        }

        public void SetBaseSpeed(float speed)
        {
            animationController.ChangeAnimationSpeed(speed);
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
