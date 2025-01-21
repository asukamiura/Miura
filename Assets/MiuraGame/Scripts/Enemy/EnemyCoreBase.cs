using Player;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCoreBase : MonoBehaviour
{
    [SerializeField] private float[] weights;
    [SerializeField] protected PlayerCore playerCore;

    private float totalWeight;

    public Rigidbody rb;
    public Animator animator;
    public HealthManager healthManager;
    public NavMeshAgent navMeshAgent;
    public Transform playerTransform;
    public float rotationSpeed = 1.0f;
    public int attackType;

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
