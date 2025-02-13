using Player;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyCoreBase : MonoBehaviour
{
    [SerializeField] float[] weights;
    [SerializeField] protected PlayerCore playerCore;
    [SerializeField] protected PlayerAttackManager attackManager;

    float totalWeight;
    bool isRotate;
    float currentAngle;
    Quaternion targetRotation;
    float currentSpeed = 1;

    public EffectPlayer effectPlayer;
    public Rigidbody rb;
    public Animator animator;
    public HealthManager healthManager;
    public NavMeshAgent navMeshAgent;
    public Transform playerTransform;
    public float rotationSpeed = 1.0f;
    public int attackType;

    public void MoveActive(bool isActive)
    {
        this.enabled = isActive;
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

    public void InitializeTotalWeight()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            totalWeight += weights[i];
        }
    }

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
