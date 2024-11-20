using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHit : MonoBehaviour
{
    [SerializeField] private PlayerCore playerCore;
    [SerializeField] private UltimateManager ultimateManager;
    private void OnTriggerEnter(Collider other)
    {
        HealthManager healthManager = other.GetComponent<HealthManager>();
        if (healthManager == null) { return; }
        switch(playerCore.stateMachine.StateID)
        {
            case PlayerStateID.AttackNormal1:
                healthManager.Damage(2);
                ultimateManager.IncreaseGauge(1);
                break;
            case PlayerStateID.AttackNormal2: 
                healthManager.Damage(4);
                ultimateManager.IncreaseGauge(2);
                break;
            case PlayerStateID.AttackNormal3:
                healthManager.Damage(6);
                ultimateManager.IncreaseGauge(3);
                break;
            case PlayerStateID.AttackSpecial2:
                healthManager.Damage(18);
                ultimateManager.IncreaseGauge(10);
                break;
        }
    }
}
