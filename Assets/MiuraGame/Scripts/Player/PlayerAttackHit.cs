using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAttackHit : MonoBehaviour
    {
        [SerializeField] private PlayerCore playerCore;
        [SerializeField] private UltimateManager ultimateManager;
        [SerializeField] private PowerUpManager powerUpManager;

        private void OnTriggerEnter(Collider other)
        {
            HealthManager healthManager = other.GetComponentInParent<HealthManager>();
            
            GameObject enemy = other.transform.root.gameObject; 

            if (healthManager == null) { return; }

            if (playerCore.hitEnemies.Contains(enemy)) { return; }

            switch (playerCore.stateMachine.StateID)
            {
                case PlayerStateID.AttackNormal1:
                    healthManager.Damage(powerUpManager.AttackPower("Normal1"));
                    ultimateManager.IncreaseGauge(100);
                    break;
                case PlayerStateID.AttackNormal2:
                    healthManager.Damage(powerUpManager.AttackPower("Normal2"));
                    ultimateManager.IncreaseGauge(2);
                    break;
                case PlayerStateID.AttackNormal3:
                    healthManager.Damage(powerUpManager.AttackPower("Normal3"));
                    ultimateManager.IncreaseGauge(3);
                    break;
                case PlayerStateID.AttackSpecial1:
                    healthManager.Damage(powerUpManager.AttackPower("Special"));
                    ultimateManager.IncreaseGauge(9);
                    break;
                case PlayerStateID.AttackSpecial2:
                    healthManager.Damage(powerUpManager.AttackPower("Special"));
                    ultimateManager.IncreaseGauge(9);
                    break;
                case PlayerStateID.AttackCharge:
                    healthManager.Damage(powerUpManager.AttackPower("Charge"));
                    ultimateManager.IncreaseGauge(10);
                    break;
                case PlayerStateID.AttackUltimate:
                    healthManager.Damage(powerUpManager.AttackPower("Ultimate"));
                    break;
            }
            playerCore.hitEnemies.Add(enemy);
            Debug.Log(healthManager.HP);
        }
    }


}
