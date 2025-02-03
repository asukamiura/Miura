using SoundSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerCore : MonoBehaviour
    {
        [SerializeField] Collider swordCollider;
        [SerializeField] int healVal = 20;  // 回復量
        [SerializeField] TextMeshProUGUI timingText;

        HealthManager healthManager;
        InputReciver Input => InputReciver.Instance;
        PlayerEvents events;

        const int ChargeAttackCost = 1;  // チャージ攻撃に必要なジャストポイント数
        const int HealCost = 2;          // 回復に必要なジャストポイント数
        const int PowerUpCost = 3;       // パワーアップに必要なジャストポイント数
        const int UltCost = 100;         // 必殺技に必要なゲージ量

        public StateMachine<PlayerStateID> stateMachine;
        public JustPointManager justPointManager;
        public PowerManager powerManager;
        public UltimateManager ultimateManager;
        public ScoreManager scoreManager;
        public AttackAssist attackAssist;
        public AnimationController animationController;
        public GameSePlayer gameSePlayer;
        public PlayerCameraController playerCameraController;
        public float MoveSpeed => powerManager.MoveSpeed;
        public Rigidbody Rb { get; set; }
        public Animator Animator { get; set; }
        public bool isJustGuard = false;
        public bool isJustDodge = false;
        public bool isInvincible = false;   // 無敵状態フラグ
        public bool CanChargeAttack => justPointManager.JustPoints >= ChargeAttackCost;
        public bool CanHeal => justPointManager.JustPoints >= HealCost;
        public bool CanPowerUp => justPointManager.JustPoints >= PowerUpCost;
        public bool CanUlt => ultimateManager.ULTVal >= UltCost;
        public HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
        public int justGuardCount = 0;
        public int justDodgeCount = 0;

        void Awake()
        {
            stateMachine = new StateMachine<PlayerStateID>();
            stateMachine.RegisterState(new PlayerIdle(this));
            stateMachine.RegisterState(new PlayerMove(this));
            stateMachine.RegisterState(new PlayerDash(this));
            stateMachine.RegisterState(new PlayerDodge(this));
            stateMachine.RegisterState(new PlayerGuard(this));
            stateMachine.RegisterState(new PlayerBlock(this));
            stateMachine.RegisterState(new PlayerAttackNormal1(this));
            stateMachine.RegisterState(new PlayerAttackNormal2(this));
            stateMachine.RegisterState(new PlayerAttackNormal3(this));
            stateMachine.RegisterState(new PlayerAttackSpecial1(this));
            stateMachine.RegisterState(new PlayerAttackSpecial2(this));
            stateMachine.RegisterState(new PlayerAttackCharge(this));
            stateMachine.RegisterState(new PlayerAttackUltimate(this));
            stateMachine.RegisterState(new PlayerDamage(this));
            stateMachine.RegisterState(new PlayerDead(this));
            Rb = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();
            healthManager = GetComponent<HealthManager>();
            justPointManager = GetComponent<JustPointManager>();
        }

        void Start()
        {
            stateMachine.Initialize(PlayerStateID.Idle);
            timingText.enabled = false;
        }

        void Update()
        {
            stateMachine.Update();

            // アイドル状態と移動状態のアニメーション更新
            Animator.SetFloat("Speed", Mathf.Clamp(Rb.velocity.magnitude, 0, 7.5f), 0.1f, Time.deltaTime);

            // 死亡していた場合、以降の処理を実行しない。
            if (stateMachine.StateID == PlayerStateID.Dead) { return; }

            // 死亡ステートに遷移
            if (healthManager.isDead && stateMachine.StateID != PlayerStateID.Dead)
            {
                stateMachine.ChangeState(PlayerStateID.Dead);
            }

            // ガード、ブロック、回避、ダメージ状態でなければ実行可能
            if (stateMachine.StateID != PlayerStateID.Guard && stateMachine.StateID != PlayerStateID.Block 
                && stateMachine.StateID != PlayerStateID.Dash && stateMachine.StateID != PlayerStateID.Dodge 
                && stateMachine.StateID != PlayerStateID.Damage)
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
            if (Input.Heal && CanHeal && healthManager.HP < healthManager.maxHP)
            {
                justPointManager.UseJustPoints(HealCost);
                healthManager.Heal(healVal);
            }

            // パワーアップ処理を実行
            if (Input.PowerUp && CanPowerUp && !powerManager.InPowerUp)
            {
                justPointManager.UseJustPoints(PowerUpCost);
                powerManager.ActionPowerUp();
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
            stateMachine?.FixedUpdate();
        }

        void OnTriggerEnter(Collider other)
        {
            if (stateMachine.StateID == PlayerStateID.Dead || isJustDodge || isInvincible) { return; }

            var enemyAttackHit = other.GetComponent<EnemyAttackHit>();

            if (other.CompareTag("EnemyAttackCanGuard") || other.CompareTag("EnemyAttackCanDodge"))
            {
                if (other.CompareTag("EnemyAttackCanGuard") && stateMachine.StateID == PlayerStateID.Guard)
                {
                    isJustGuard = true;
                }
                else if (other.CompareTag("EnemyAttackCanDodge") && stateMachine.StateID == PlayerStateID.Dash)
                {
                    isJustDodge = true;
                }
                else
                {
                    // 攻撃を受けたらダメージステートへ遷移
                    stateMachine.ChangeState(PlayerStateID.Damage);
                    scoreManager.SubtractScore((int)enemyAttackHit.damageVal);
                }
            }
        }

        public void AttackStart()
        {
            if (swordCollider != null)
            {
                swordCollider.enabled = true;
            }
        }

        public void AttackEnd()
        {
            if (swordCollider != null)
            {
                swordCollider.enabled = false;
                hitEnemies.Clear();
            }
        }

        public void TimingUIShow(string timing)
        {
            StartCoroutine(TimingUIChange(timing));
        }

        IEnumerator TimingUIChange(string timing)
        {
            timingText.text = timing;
            timingText.enabled = true;
            yield return new WaitForSeconds(1);
            timingText.enabled = false;
        }
    }
}
