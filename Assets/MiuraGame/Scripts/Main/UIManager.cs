using Player;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

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
    [SerializeField] GameObject timingUI;
    [SerializeField] TextMeshProUGUI timingText;

    int previousJustPoints;

    void Awake()
    {
        PlayerGuard.OnJudgeGuardTiming += TimingUIShow;
        PlayerDash.OnJudgeDodgeTiming += TimingUIShow;
    }

    private void OnDisable()
    {
        PlayerGuard.OnJudgeGuardTiming -= TimingUIShow;
        PlayerDash.OnJudgeDodgeTiming -= TimingUIShow;
    }

    void Start()
    {
        previousJustPoints = justPointManager.JustPoints;

        UpdateJustPointsUI();

        timingUI.SetActive(false);
    }

    void Update()
    {
        playerHPGauge.fillAmount = playerHealthManager.HP / playerHealthManager.MaxHP;
        enemyHPGauge.fillAmount = enemyHealthManager.HP / enemyHealthManager.MaxHP;
        UltimateGauge.fillAmount = ultimateManager.ULTVal / ultimateManager.maxUltVal;

        // ジャストポイントに増減があった場合UIを更新
        if (previousJustPoints != justPointManager.JustPoints)
        {
            UpdateJustPointsUI();
        }
    }

    void UpdateJustPointsUI()
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

    public void TimingUIShow(string timing)
    {
        StartCoroutine(TimingUIChange(timing));
    }

    IEnumerator TimingUIChange(string timing)
    {
        timingText.text = timing;
        timingUI.SetActive(true);

        yield return new WaitForSeconds(1);

        timingUI.SetActive(false);
    }
}
