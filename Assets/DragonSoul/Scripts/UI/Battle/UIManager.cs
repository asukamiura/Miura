using Player;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerEventManager playerEventManager;
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
        PlayerGuard.OnJudgeGuardTiming += ShowTimingUI;
        PlayerDash.OnJudgeDodgeTiming += ShowTimingUI;
    }

    private void OnDisable()
    {
        PlayerGuard.OnJudgeGuardTiming -= ShowTimingUI;
        PlayerDash.OnJudgeDodgeTiming -= ShowTimingUI;
    }

    void Start()
    {
        previousJustPoints = justPointManager.JustPoint;

        UpdateJustPointsUI();

        timingUI.SetActive(false);
    }

    void Update()
    {
        playerHPGauge.fillAmount = playerHealthManager.HP / playerHealthManager.MaxHP;
        enemyHPGauge.fillAmount = enemyHealthManager.HP / enemyHealthManager.MaxHP;
        UltimateGauge.fillAmount = ultimateManager.UltVal / ultimateManager.MaxUltVal;

        // ジャストポイントに増減があった場合UIを更新
        if (previousJustPoints != justPointManager.JustPoint)
        {
            UpdateJustPointsUI();
        }
    }

    // ジャストポイントUIの更新処理
    void UpdateJustPointsUI()
    {
        for (int i = 0; i < justPointUI.Length; i++)
        {
            if (i < justPointManager.JustPoint)
            {
                justPointUI[i].enabled = true;
            }
            else
            {
                justPointUI[i].enabled = false;
            }
        }
        previousJustPoints = justPointManager.JustPoint;
    }

    public void ShowTimingUI(string timing)
    {
        StartCoroutine(ChangeTimingUI(timing));
    }

    // ガード、回避のタイミングUIの表示処理
    IEnumerator ChangeTimingUI(string timing)
    {
        timingText.text = timing;
        timingUI.SetActive(true);

        yield return new WaitForSeconds(1);

        timingUI.SetActive(false);
    } 
}
