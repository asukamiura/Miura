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
    [SerializeField] Image[] justPointsUI;

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
        switch (justPointManager.JustPoints)
        {
            case 0:
                justPointsUI[0].enabled = false;
                justPointsUI[1].enabled = false;
                justPointsUI[2].enabled = false;
                justPointsUI[3].enabled = false;
                justPointsUI[4].enabled = false;
                break;
            case 1:
                justPointsUI[0].enabled = true;
                justPointsUI[1].enabled = false;
                justPointsUI[2].enabled = false;
                justPointsUI[3].enabled = false;
                justPointsUI[4].enabled = false;
                break;
            case 2:
                justPointsUI[0].enabled = true;
                justPointsUI[1].enabled = true;
                justPointsUI[2].enabled = false;
                justPointsUI[3].enabled = false;
                justPointsUI[4].enabled = false;
                break;
            case 3:
                justPointsUI[0].enabled = true;
                justPointsUI[1].enabled = true;
                justPointsUI[2].enabled = true;
                justPointsUI[3].enabled = false;
                justPointsUI[4].enabled = false;
                break;
            case 4:
                justPointsUI[0].enabled = true;
                justPointsUI[1].enabled = true;
                justPointsUI[2].enabled = true;
                justPointsUI[3].enabled = true;
                justPointsUI[4].enabled = false;
                break;
            case 5:
                justPointsUI[0].enabled = true;
                justPointsUI[1].enabled = true;
                justPointsUI[2].enabled = true;
                justPointsUI[3].enabled = true;
                justPointsUI[4].enabled = true;
                break;
        }
        previousJustPoints = justPointManager.JustPoints;
    }
}
