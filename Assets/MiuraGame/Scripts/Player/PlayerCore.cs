using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Player
{  
    public class PlayerCore : MonoBehaviour
    {       
        [SerializeField] private Collider swordCollider;
        [SerializeField] private int healVal = 20;  // 回復量
        [SerializeField] GameObject playerCM;
        [SerializeField] GameObject justGuardCM;
        [SerializeField] private TextMeshProUGUI timingText;

        private HealthManager healthManager;
        private InputReciver Input => InputReciver.Instance;
        private const int chargeAttackCost = 1;  // チャージ攻撃に必要なジャストポイント数
        private const int healCost = 2;          // 回復に必要なジャストポイント数
        private const int powerUpCost = 3;       // パワーアップに必要なジャストポイント数
        private const int ultCost = 100;         // 必殺技に必要なゲージ量

        public StateMachine<PlayerStateID> stateMachine;
        public JustPointManager justPointManager;
        public PowerUpManager powerUpManager;
        public UltimateManager ultimateManager;
        public AttackCorrectionManager attackCorrectionManager;
        public Collider judgeDodgeCollider;
        public float MoveSpeed => powerUpManager.MoveSpeed;
        public Rigidbody Rb { get; private set; }
        public Animator Animator { get; private set; }
        public bool isJustGuard = false;
        public bool isJustDodge = false;
        public bool isInvincible = false;   // 無敵状態フラグ
        public bool CanChargeAttack => justPointManager.JustPoints >= chargeAttackCost;
        public bool CanHeal => justPointManager.JustPoints >= healCost;
        public bool CanPowerUp => justPointManager.JustPoints >= powerUpCost;
        public bool CanUlt => ultimateManager.ULTVal >= ultCost;
        public HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
        public enum Timing
        {
            Late,
            Just,
            Fast,
            None,
        }
        public Timing timing  = Timing.None;
        public int justGaurdCount = 0;
        public int justDodgeCount = 0;

        private void Awake()
        {
            stateMachine = new StateMachine<PlayerStateID>();
            stateMachine.RegisterState(new PlayerIdle(this));
            stateMachine.RegisterState(new PlayerMove(this));
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

        private void Start()
        {
            stateMachine.Initialize(PlayerStateID.Idle);
            timingText.enabled = false;
        }

        private void Update()
        {
            Debug.Log(justGaurdCount);
            stateMachine.Update();

            // アイドル状態と移動状態のアニメーション更新
            Animator.SetFloat("Speed", Rb.velocity.magnitude, 0.1f, Time.deltaTime);

            // 死亡していた場合、以降の処理を実行しない。
            if (stateMachine.StateID == PlayerStateID.Dead) { return; }

            // 死亡ステートに遷移
            if (healthManager.isDead && stateMachine.StateID != PlayerStateID.Dead)
            {
                stateMachine.ChangeState(PlayerStateID.Dead);
            }

            if (stateMachine.StateID != PlayerStateID.Guard && stateMachine.StateID != PlayerStateID.Dodge && stateMachine.StateID != PlayerStateID.Damage)
            {
                // ガードステートに遷移
                if (Input.Guard)
                {
                    stateMachine.ChangeState(PlayerStateID.Guard);
                }

                // 回避ステートに遷移
                if (Input.Dodge)
                {
                    stateMachine.ChangeState(PlayerStateID.Dodge);
                }
            }

            // 回復処理を実行
            if (Input.Heal && CanHeal && healthManager.HP < healthManager.maxHP)
            {
                justPointManager.UseJustPoints(healCost);
                healthManager.Heal(healVal);
            }

            // パワーアップ処理を実行
            if (Input.PowerUp && CanPowerUp && !powerUpManager.InPowerUp)
            {
                justPointManager.UseJustPoints(powerUpCost);
                powerUpManager.ActionPowerUp();
            }

            // 必殺技を実行
            if (Input.AttackUltimate && CanUlt)
            {
                stateMachine.ChangeState(PlayerStateID.AttackUltimate);
                ultimateManager.DecreaseGauge(ultCost);
            }
        }

        private void FixedUpdate()
        {
            stateMachine?.FixedUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (stateMachine.StateID == PlayerStateID.Dead || isJustDodge || isInvincible) { return; }

            if (other.CompareTag("EnemyAttackCanGuard"))
            {
                if (stateMachine.StateID == PlayerStateID.Guard)
                {
                    isJustGuard = true;
                }
                else
                {
                    // 攻撃を受けたらダメージステートへ遷移
                    stateMachine.ChangeState(PlayerStateID.Damage);
                }
            }

            if (other.CompareTag("EnemyAttackCanDodge") && !judgeDodgeCollider.enabled)
            {
                // 攻撃を受けたらダメージステートへ遷移
                stateMachine.ChangeState(PlayerStateID.Damage);
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
