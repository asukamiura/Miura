using Player;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHit : MonoBehaviour
{
    enum AttackType
    {
        CanGuard,
        CanDodge,
    }
    public int damageVal = 0;
    [SerializeField] AttackType attackType;

    HashSet<GameObject> hitObjs = new HashSet<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCore playerCore = other.GetComponentInParent<PlayerCore>();
            HealthManager healthManager = other.GetComponentInParent<HealthManager>();
            GameObject player = other.transform.root.gameObject;

            if (playerCore == null || healthManager == null || playerCore.IsInvincible)
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
                    if (playerCore.stateMachine.StateID == PlayerStateID.Dash) { return; }
                    break;
            }

            healthManager.Damage(damageVal);

            hitObjs.Add(player);
        }
    }

    void OnDisable()
    {
        hitObjs.Clear();
    }
}
