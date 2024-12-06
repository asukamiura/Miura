using Player;
using UnityEngine;

public class EnemyAttackHit : MonoBehaviour
{
    private enum AttackType
    {
        CanGuard,
        CanDodge,
    }
    [SerializeField] private int damageVal = 0;
    [SerializeField] private AttackType attackType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCore playerCore = other.GetComponentInParent<PlayerCore>();
            HealthManager healthManager = other.GetComponentInParent<HealthManager>();
            if (playerCore == null || healthManager == null || playerCore.isInvincible)
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
