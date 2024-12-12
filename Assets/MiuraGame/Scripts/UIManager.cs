using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] HealthManager playerHealthManager;
    [SerializeField] HealthManager enemyHealthManager;
    [SerializeField] UltimateManager ultimateManager;
    [SerializeField] JustPointManager justPointManager;
    [SerializeField] Image playerHPGauge;
    [SerializeField] Image enemyHPGauge;
    [SerializeField] Image UltimateGauge;
    [SerializeField] Image[] justPointUI;

    private int previousJustPoints;

    private void Start()
    {
        previousJustPoints = justPointManager.JustPoints;

        UpdateJustPointsUI();
    }

    private void Update()
    {
        playerHPGauge.fillAmount = playerHealthManager.HP / playerHealthManager.maxHP;
        enemyHPGauge.fillAmount = enemyHealthManager.HP / enemyHealthManager.maxHP;
        UltimateGauge.fillAmount = ultimateManager.ULTVal / ultimateManager.maxUltVal;

        if (previousJustPoints != justPointManager.JustPoints)
        {
            UpdateJustPointsUI();
        }
    }

    private void UpdateJustPointsUI()
    {
        for (int i = 0; i < justPointUI.Length; i++)
        {
            if (i < justPointManager.JustPoints)
            {
                justPointUI[i].enabled = true;
            }
            else
            {
                justPointUI[i].enabled = false;
            }
        }
        previousJustPoints = justPointManager.JustPoints;
    }
}
