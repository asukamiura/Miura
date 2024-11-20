using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHit : MonoBehaviour
{
    [SerializeField] int damageVal = 0;
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

            if (playerCore.stateMachine.StateID == PlayerStateID.Guard || playerCore.stateMachine.StateID == PlayerStateID.Dodge)
            {
                return;
            }
            
            healthManager.Damage(damageVal);
        }
    }
}
