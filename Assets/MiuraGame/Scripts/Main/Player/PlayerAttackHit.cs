using SoundSystem;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerAttackHit : MonoBehaviour
    {
        [SerializeField] PlayerCore playerCore;
        [SerializeField] UltimateManager ultimateManager;
        [SerializeField] PowerManager powerManager;
        [SerializeField] ScoreManager scoreManager;
        [SerializeField] GameSePlayer gameSePlayer;
        [SerializeField] DamageUIGenerator damageUIGenerator; 

        void OnTriggerEnter(Collider other)
        {
            HealthManager healthManager = other.GetComponentInParent<HealthManager>();

            GameObject enemy = other.transform.root.gameObject;

            if (healthManager == null) { return; }

            if (playerCore.hitEnemies.Contains(enemy)) { return; }

            Vector3 closestPoint = other.ClosestPoint(transform.position);

            EffectGenerator.Instance.PlayEffect("HitEffect", transform.position, Quaternion.identity, 1);

            float damage = 0;
            switch (playerCore.stateMachine.StateID)
            {
                case PlayerStateID.AttackNormal1:
                    damage = powerManager.AttackPower("Normal1");
                    ultimateManager.IncreaseGauge(1);
                    scoreManager.AddScore("AttackNormal1");
                    break;
                case PlayerStateID.AttackNormal2:
                    damage = powerManager.AttackPower("Normal2");
                    ultimateManager.IncreaseGauge(2);
                    scoreManager.AddScore("AttackNormal2");
                    break;
                case PlayerStateID.AttackNormal3:
                    damage = powerManager.AttackPower("Normal3");
                    ultimateManager.IncreaseGauge(3);
                    scoreManager.AddScore("AttackNormal3");
                    break;
                case PlayerStateID.AttackSpecial1:
                    damage = powerManager.AttackPower("Special");
                    scoreManager.AddScore("AttackSpecial");
                    ultimateManager.IncreaseGauge(9);
                    break;
                case PlayerStateID.AttackSpecial2:
                    damage = powerManager.AttackPower("Special");
                    scoreManager.AddScore("AttackSpecial");
                    ultimateManager.IncreaseGauge(9);
                    break;
                case PlayerStateID.AttackCharge:
                    damage = powerManager.AttackPower("Charge");
                    scoreManager.AddScore("AttackCharge");
                    ultimateManager.IncreaseGauge(10);
                    break;
                case PlayerStateID.AttackUltimate:
                    damage = powerManager.AttackPower("Ultimate");
                    scoreManager.AddScore("AttackUltimate");
                    break;
            }

            damageUIGenerator.GenerateDamageUI(damage, closestPoint);
            healthManager.Damage(damage);
            playerCore.hitEnemies.Add(enemy);
        }
    }
}
