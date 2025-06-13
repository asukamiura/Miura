using Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyCoreBase : MonoBehaviour, ISlowable
{
    [SerializeField] protected PlayerCore playerCore;
    [SerializeField] protected PlayerAttackManager attackManager;
    [SerializeField] List<GameObject> attackColliders = new List<GameObject>();
    [SerializeField] Collider bodyCollider;

    protected bool isMovable = false;
    bool isRotate;
    float currentAngle;
    Quaternion targetRotation;
    float slowFactor = 1;
    float baseSpeed = 1;

    public EffectPlayer effectPlayer;
    public HealthManager healthManager;
    public NavMeshAgent navMeshAgent;
    public Transform playerTransform;
    public float rotationSpeed = 1.0f;
    public int attackType;
    public Rigidbody Rb { get; private set; }
    public Animator Animator { get; private set; }
    public AnimatorStateInfo CurrentStateInfo => Animator.GetCurrentAnimatorStateInfo(0);

    protected virtual void Awake()
    {
        SlowManager.Instance.Register(this);
        Rb = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
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

    public float AngleToPlayer()
    {
        // プレイヤー方向の角度を計算
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        return Vector3.SignedAngle(transform.forward, direction, Vector3.up);
    }

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

    public void Rotate(float targetAngle)
    {
        if (!isRotate)
        {
            currentAngle = transform.rotation.eulerAngles.y;
            targetRotation = Quaternion.Euler(0, currentAngle - targetAngle, 0);
            isRotate = true;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Mathf.Approximately(Quaternion.Angle(transform.rotation, targetRotation), 0))
        {
            transform.rotation = targetRotation;
        }
    }

    public void MoveActive(bool isActive)
    {
        navMeshAgent.isStopped = !isActive;
        isMovable = isActive;
    }

    public void ApplySlow(float factor)
    {
        slowFactor = factor;
        Animator.speed = slowFactor;
        navMeshAgent.speed = baseSpeed * slowFactor;
    }

    public void SetBaseSpeed(float speed)
    {
        baseSpeed = speed;
        navMeshAgent.speed = baseSpeed * slowFactor;
    }

    public void AttackStart(string attackColliderName)
    {
        var attackCollider = attackColliders.FirstOrDefault(attackCollider => attackCollider.name == attackColliderName);

        if (attackCollider == null) { return; }

        attackCollider.SetActive(true);
    }

    public void AttackEnd(string attackColliderName)
    {
        var attackCollider = attackColliders.FirstOrDefault(attackCollider => attackCollider.name == attackColliderName);

        if (attackCollider == null) { return; }

        attackCollider.SetActive(false);
    }

    public void ResetAttackCollider()
    {
        foreach (var attackCollider in attackColliders)
        {
            attackCollider.SetActive(false);
        }
    }
}
