using SoundSystem;
using UnityEngine;

namespace Player
{
    public class PlayerAttackHit : MonoBehaviour
    {
        [SerializeField] PlayerCore playerCore;
        [SerializeField] UltimateManager ultimateManager;
        [SerializeField] PowerManager powerUpManager;
        [SerializeField] ScoreManager scoreManager;
        [SerializeField] GameSePlayer gameSePlayer;

        void OnTriggerEnter(Collider other)
        {
            HealthManager healthManager = other.GetComponentInParent<HealthManager>();

            GameObject enemy = other.transform.root.gameObject;

            if (healthManager == null) { return; }

            if (playerCore.hitEnemies.Contains(enemy)) { return; }

            EffectGenerator.Instance.PlayEffect("HitEffect", transform.position, Quaternion.identity, 1);

            switch (playerCore.stateMachine.StateID)
            {
                case PlayerStateID.AttackNormal1:
                    healthManager.Damage(powerUpManager.AttackPower("Normal1"));
                    ultimateManager.IncreaseGauge(1);
                    scoreManager.AddScore("AttackNormal1");
                    gameSePlayer.PlaySe("AttackNormal1");
                    break;
                case PlayerStateID.AttackNormal2:
                    healthManager.Damage(powerUpManager.AttackPower("Normal2"));
                    ultimateManager.IncreaseGauge(2);
                    scoreManager.AddScore("AttackNormal2");
                    gameSePlayer.PlaySe("AttackNormal2");
                    break;
                case PlayerStateID.AttackNormal3:
                    healthManager.Damage(powerUpManager.AttackPower("Normal3"));
                    ultimateManager.IncreaseGauge(3);
                    scoreManager.AddScore("AttackNormal3");
                    gameSePlayer.PlaySe("AttackNormal3");
                    break;
                case PlayerStateID.AttackSpecial1:
                    healthManager.Damage(powerUpManager.AttackPower("Special"));
                    scoreManager.AddScore("AttackSpecial");
                    gameSePlayer.PlaySe("AttackNormal3");
                    ultimateManager.IncreaseGauge(9);
                    break;
                case PlayerStateID.AttackSpecial2:
                    healthManager.Damage(powerUpManager.AttackPower("Special"));
                    scoreManager.AddScore("AttackSpecial");
                    gameSePlayer.PlaySe("AttackNormal1");
                    ultimateManager.IncreaseGauge(9);
                    break;
                case PlayerStateID.AttackCharge:
                    healthManager.Damage(powerUpManager.AttackPower("Charge"));
                    scoreManager.AddScore("AttackCharge");
                    gameSePlayer.PlaySe("AttackNormal3");
                    ultimateManager.IncreaseGauge(10);
                    break;
                case PlayerStateID.AttackUltimate:
                    healthManager.Damage(powerUpManager.AttackPower("Ultimate"));
                    scoreManager.AddScore("AttackUltimate");
                    gameSePlayer.PlaySe("AttackNormal3");
                    break;
            }
            playerCore.hitEnemies.Add(enemy);
        }
    }


}
