using Player;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyCoreBase : MonoBehaviour, ISlowable
{
    [SerializeField] float[] weights;
    [SerializeField] protected PlayerCore playerCore;
    [SerializeField] protected PlayerAttackManager attackManager;

    float totalWeight;
    bool isRotate;
    float currentAngle;
    Quaternion targetRotation;
    float slowFactor = 1;
    float baseSpeed = 1;

    public EffectPlayer effectPlayer;
    public Rigidbody rb;
    public Animator animator;
    public HealthManager healthManager;
    public NavMeshAgent navMeshAgent;
    public Transform playerTransform;
    public float rotationSpeed = 1.0f;
    public int attackType;

    protected virtual void Awake()
    {
        SlowManager.Instance.Register(this);
    }

    public void MoveActive(bool isActive)
    {
        this.enabled = isActive;
    }

    public void ApplySlow(float factor)
    {
        slowFactor = factor;
        animator.speed = slowFactor;
        navMeshAgent.speed = baseSpeed * slowFactor;
    }

    public void SetBaseSpeed(float speed)
    {
        baseSpeed = speed;
        navMeshAgent.speed = baseSpeed * slowFactor;
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

    /// <summary>
    /// 攻撃抽選の重さを初期化
    /// </summary>
    public void InitializeTotalWeight()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            totalWeight += weights[i];
        }
    }

    /// <summary>
    /// 重さの更新
    /// </summary>
    /// <param name="attackNum">攻撃タイプ</param>
    public void UpdateTotalWeight(int attackNum)
    {
        totalWeight = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            if (i == attackNum)
            {
                weights[i] = 10;
            }
            else
            {
                weights[i] += 10;
            }

            totalWeight += weights[i];
        }
    }

    /// <summary>
    /// 攻撃タイプの抽選
    /// </summary>
    /// <returns>攻撃タイプ</returns>
    public int ChooseAttack()
    {
        var randomPoint = Random.Range(0, totalWeight);

        var currentWeight = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            currentWeight += weights[i];

            if (randomPoint < currentWeight)
            {
                return i;
            }
        }

        return weights.Length - 1;
    }
}
