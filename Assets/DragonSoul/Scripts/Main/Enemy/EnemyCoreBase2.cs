using UnityEngine;
using UnityEngine.AI;

public class EnemyCoreBase2 : MonoBehaviour
{
    [SerializeField] float senserDistance = 5;
    float playerStayTimer = 0;
    bool isPlayerNear = false;

    public EffectPlayer effectPlayer;
    public HealthManager healthManager;
    public NavMeshAgent navMeshAgent;
    public Transform playerTransform;
    public Rigidbody Rb { get; private set; }
    public Animator Animator { get; private set; }

    public AnimatorStateInfo CurrentStateInfo => Animator.GetCurrentAnimatorStateInfo(0);

    public float DistanceToPlayer => Vector3.Distance(transform.position, playerTransform.position);

    void OnDrawGizmos()
    {
        if (transform == null) { return; }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, senserDistance); // アシスト範囲を表示                                                             
    }

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (!isPlayerNear && DistanceToPlayer <= senserDistance)
        {
            playerStayTimer += Time.deltaTime;
        }
    }
}
