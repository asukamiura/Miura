using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyCoreBase : MonoBehaviour, ISlowable, IEnemyDamageable
{
    [SerializeField] protected EnemyData enemyData;
    [SerializeField] List<GameObject> attackColliders = new List<GameObject>();
    [SerializeField] Collider bodyCollider;

    protected bool isMovable = false;   // 動けるかどうか
    protected bool isFlinch = false;    // ひるむかどうか
    float slowFactor = 1;
    float baseSpeed = 1;

    public float rotationSpeed = 1.0f;
    public bool isJustGuarded = false;  // ジャストガードされたかどうか
    public WarningEffectController warningEffectManager;
    public HealthManager healthManager;
    public Transform playerTransform;
    public NavMeshAgent NavMeshAgent { get; private set; }
    public Rigidbody Rb { get; private set; }
    public Animator Animator { get; private set; }
    public AnimatorStateInfo CurrentStateInfo => Animator.GetCurrentAnimatorStateInfo(0);

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        NavMeshAgent = GetComponent<NavMeshAgent>();

        // 各攻撃コライダーに、この敵自身(ジャストガード対象)を登録する
        foreach (GameObject collider in attackColliders)
        {
            collider.GetComponent<EnemyAttack>().OnJustGuarded += HandleJustGuarded;
        }
    }

    protected virtual void Start()
    {
        SlowManager.Instance.Register(this);

        // HPを設定
        healthManager.SetMaxHP(enemyData.MaxHP);
    }

    protected virtual void Update()
    {
        if (Animator.IsInTransition(0))
        {
            ResetAttackCollider();
        }

        if (healthManager.IsDead)
        {
            bodyCollider.enabled = false;
        }
    }

    // 攻撃がジャストガードされた際の処理
    void HandleJustGuarded()
    {
        isJustGuarded = true;
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage">受けるダメージ量</param>
    /// <param name="canFlinch">ひるむ攻撃を受けたのか</param>
    public void TakeDamage(float damage, bool canFlinch)
    {
        healthManager.ReduceHP(damage);

        switch (enemyData.FlinchType)
        {
            case FlinchType.Always:
                isFlinch = true;
                break;
            case FlinchType.Conditional:
                isFlinch = canFlinch;
                break;
        }
    }

    /// <summary>
    /// プレイヤーとの距離を計算
    /// </summary>
    /// <returns>プレイヤーとの距離</returns>
    public float DistanceToPlayer()
    {
        // プレイヤーとの距離を計算
        Vector3 playerPosition = playerTransform.position;
        playerPosition.y = transform.position.y;
        return Vector3.Distance(transform.position, playerPosition);
    }

    /// <summary>
    /// プレイヤーの方向を向く
    /// </summary>
    public void LookAtPlayer()
    {
        // プレイヤ－の方向を向く
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookAtRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, rotationSpeed * Time.deltaTime);
    }

    public void MoveActive(bool isActive)
    {
        if (NavMeshAgent != null)
        {
            NavMeshAgent.isStopped = !isActive;
        }

        isMovable = isActive;
    }

    public SlowTargetType Type => SlowTargetType.Enemy;

    public void ApplySlow(float factor)
    {
        slowFactor = factor;
        Animator.speed = slowFactor;
        NavMeshAgent.speed = baseSpeed * slowFactor;
    }

    public void SetBaseSpeed(float speed)
    {
        baseSpeed = speed;
        NavMeshAgent.speed = baseSpeed * slowFactor;
    }

    /// <summary>
    /// 攻撃判定を有効にする
    /// </summary>
    /// <param name="attackColliderName">有効にするコライダーオブジェクトの名前</param>
    public void AttackStart(string attackColliderName)
    {
        var attackCollider = attackColliders.FirstOrDefault(attackCollider => attackCollider.name == attackColliderName);

        if (attackCollider == null) { return; }

        attackCollider.SetActive(true);
    }

    /// <summary>
    /// 攻撃判定を無効にする
    /// </summary>
    /// <param name="attackColliderName">無効にするコライダーオブジェクトの名前</param>
    public void AttackEnd(string attackColliderName)
    {
        var attackCollider = attackColliders.FirstOrDefault(attackCollider => attackCollider.name == attackColliderName);

        if (attackCollider == null) { return; }

        attackCollider.SetActive(false);
    }

    /// <summary>
    /// 攻撃判定をすべて無効にする
    /// </summary>
    public void ResetAttackCollider()
    {
        foreach (var attackCollider in attackColliders)
        {
            attackCollider.SetActive(false);
        }
    }
}
