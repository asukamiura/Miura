using Player;
using System.Collections.Generic;
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

    private HashSet<GameObject> hitObjs = new HashSet<GameObject>(); 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCore playerCore = other.GetComponentInParent<PlayerCore>();
            HealthManager healthManager = other.GetComponentInParent<HealthManager>();
            GameObject player = other.transform.root.gameObject;

            if (playerCore == null || healthManager == null || playerCore.isInvincible)
            {
                return;
            }

            if (hitObjs.Contains(player)) { return; }

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

            hitObjs.Add(player);
        }
    }

    private void OnDisable()
    {
        hitObjs.Clear();
    }
}
