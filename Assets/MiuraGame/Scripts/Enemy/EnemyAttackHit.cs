using Player;
using UnityEngine;

public class EnemyAttackHit : MonoBehaviour
{
    [SerializeField] int damageVal = 0;
    private enum AttackType
    {
        CanGuard,
        CanDodge,
    }
    [SerializeField] AttackType attackType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCore playerCore = other.GetComponent<PlayerCore>();
            HealthManager healthManager = other.GetComponent<HealthManager>();
            if (playerCore == null || healthManager == null)
            {
                return;
            }

            switch (attackType)
            {
                case AttackType.CanGuard:
                    if (playerCore.stateMachine.StateID == PlayerStateID.Guard) { return; }
                    break;
                case AttackType.CanDodge:
                    if (playerCore.judgeDodgeCollider.enabled == true || playerCore.isJustDodge) { return; }
                    break;
            }

            healthManager.Damage(damageVal);
        }
    }
}
