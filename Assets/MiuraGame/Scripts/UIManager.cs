using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] HealthManager playerHealthManager;
    [SerializeField] HealthManager enemyHealthManager;
    [SerializeField] UltimateManager ultimateManager; 
    [SerializeField] Image playerHPGauge;
    [SerializeField] Image enemyHPGauge;
    [SerializeField] Image UltimateGauge;

    private void Update()
    {
        playerHPGauge.fillAmount = playerHealthManager.HP / playerHealthManager.maxHP;
        enemyHPGauge.fillAmount = enemyHealthManager.HP / enemyHealthManager.maxHP;
        UltimateGauge.fillAmount = ultimateManager.ULTVal / ultimateManager.maxUltVal;
    }
}
